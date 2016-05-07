using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using log4net.Config;
using System.Xml;

using SharpNeat.Genomes.Neat;
using SharpNeat.Decoders.Neat;

using SharpNeat.Phenomes;
using SharpNeat.Phenomes.NeuralNets;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.HyperNeat;
using SharpNeat.DistanceMetrics;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Network;
using SharpNeat.SpeciationStrategies;
using SharpNeat.Domains;

using GeometryFriends.AI.Interfaces;
using System.Drawing;
using GeometryFriends.AI.ActionSimulation;
using GeometryFriends.AI;
using GeometryFriends.AI.Debug;
using GeometryFriends.AI.Perceptions.Information;

using System.Runtime.InteropServices;
using EvolutionGeometryFriends;

namespace GeometryFriendsAgents
{
    class CircleAgent : AbstractCircleAgent
    {
        //agent implementation specificiation
        private bool implementedAgent;
        private string agentName = "RandPredictorCircle";

        //auxiliary variables for agent action
        private Moves currentAction;
        private List<Moves> possibleMoves;
        private long lastMoveTime;
        private Random rnd;

        //predictor of actions for the circle
        private ActionSimulator predictor = null;
        private DebugInformation[] debugInfo = null;
        private int debugCircleSize = 20;

        //debug agent predictions and history keeping
        private List<CollectibleRepresentation> caughtCollectibles;
        private List<CollectibleRepresentation> uncaughtCollectibles;
        private object remainingInfoLock = new Object();
        private List<CollectibleRepresentation> remaining;

        //Sensors Information and level state
        private CountInformation numbersInfo;
        private RectangleRepresentation rectangleInfo;
        private CircleRepresentation circleInfo;
        private ObstacleRepresentation[] obstaclesInfo;
        private ObstacleRepresentation[] rectanglePlatformsInfo;
        private ObstacleRepresentation[] circlePlatformsInfo;
        private CollectibleRepresentation[] collectiblesInfo;

        private int nCollectiblesLeft;

        //Area of the game screen
        private Rectangle area;
    
        //neural network
        protected IBlackBox Brain;
        //protected FastAcyclicNetwork Brain;
        List<Neuron> nodes = new List<Neuron>();

        //SHARPNEAT Objects
        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        const string NEURAL_NETWORK_FILE = "/Agents/neural_network_params/circle_neural_network.xml";
        const string NEURAL_NETWORK_CONFIG = "/geometryfriends.config.xml";
        const string FITNESS_FILE = "/fitness.txt";


        System.IO.StreamWriter name;
        
        public CircleAgent()
        {
            //log4net.Config.XmlConfigurator.Configure();
            //Log4NetController.Log("Value from Program: " , Log4NetController.LogLevel.Debug);
            NeatGenome genome = null;

            NeatGenomeParameters _neatGenomeParams = new NeatGenomeParameters();
            _neatGenomeParams.AddConnectionMutationProbability = 0.1;
            _neatGenomeParams.AddNodeMutationProbability = 0.01;
            _neatGenomeParams.ConnectionWeightMutationProbability = 0.89;
            _neatGenomeParams.InitialInterconnectionsProportion = 0.05;

            NeatGenomeFactory ngf = new NeatGenomeFactory(26, 3, _neatGenomeParams);

            // Try to load the genome from the XML document.
            try
            {
                using (XmlReader xr = XmlReader.Create(Environment.CurrentDirectory + NEURAL_NETWORK_FILE))
                    genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, ngf)[0];
            }
            catch (Exception e1)
            {
                return;
            }

            // Get a genome decoder that can convert genomes to phenomes.
            XmlDocument config = new XmlDocument();
            config.Load(Environment.CurrentDirectory + NEURAL_NETWORK_CONFIG);

            // Get root activation element.
            XmlNodeList nodeList = config.DocumentElement.GetElementsByTagName("Activation", "");
            if (nodeList.Count != 1)
            {
                throw new ArgumentException("Missing or invalid activation XML config setting.");
            }

            XmlElement xmlActivation = nodeList[0] as XmlElement;
            string schemeStr = XmlUtils.TryGetValueAsString(xmlActivation, "Scheme");

            
            //NetworkActivationScheme nes = ExperimentUtils.CreateActivationScheme(config.DocumentElement, "Activation");
            NetworkActivationScheme nes = NetworkActivationScheme.CreateAcyclicScheme();

            var genomeDecoder = new NeatGenomeDecoder(nes);

            // Decode the genome into a phenome (neural network).
            var phenome = genomeDecoder.Decode(genome);

            // Set the NEAT player's brain to the newly loaded neural network.
            Brain = phenome;

            //Change flag if agent is not to be used
            implementedAgent = true;

            //setup for action updates
            lastMoveTime = DateTime.Now.Second;
            currentAction = Moves.NO_ACTION;

            //prepare the possible moves  
            possibleMoves = new List<Moves>();
            possibleMoves.Add(Moves.ROLL_LEFT);
            possibleMoves.Add(Moves.ROLL_RIGHT);
            possibleMoves.Add(Moves.JUMP);

            //history keeping
            uncaughtCollectibles = new List<CollectibleRepresentation>();
            caughtCollectibles = new List<CollectibleRepresentation>();
            remaining = new List<CollectibleRepresentation>();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("gen={0:N0} bestFitness={1:N6}", _ea.CurrentGeneration, _ea.Statistics._maxFitness));

            // Save the best genome to file
            var doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { _ea.CurrentChampGenome }, false);
            doc.Save(NEURAL_NETWORK_FILE);
        }

        //implements abstract circle interface: used to setup the initial information so that the agent has basic knowledge about the level
        public override void Setup(CountInformation nI, RectangleRepresentation rI, CircleRepresentation cI, ObstacleRepresentation[] oI, ObstacleRepresentation[] rPI, ObstacleRepresentation[] cPI, CollectibleRepresentation[] colI, Rectangle area, double timeLimit)
        {
            numbersInfo = nI;
            nCollectiblesLeft = nI.CollectiblesCount;
            rectangleInfo = rI;
            circleInfo = cI;
            obstaclesInfo = oI;
            rectanglePlatformsInfo = rPI;
            circlePlatformsInfo = cPI;
            collectiblesInfo = colI;
            uncaughtCollectibles = new List<CollectibleRepresentation>(collectiblesInfo);
            this.area = area;

            //DebugSensorsInfo();
        }

        //implements abstract circle interface: registers updates from the agent's sensors that it is up to date with the latest environment information
        /*WARNING: this method is called independently from the agent update - Update(TimeSpan elapsedGameTime) - so care should be taken when using complex 
         * structures that are modified in both (e.g. see operation on the "remaining" collection)      
         */
        public override void SensorsUpdated(int nC, RectangleRepresentation rI, CircleRepresentation cI, CollectibleRepresentation[] colI)
        {
            nCollectiblesLeft = nC;

            rectangleInfo = rI;
            circleInfo = cI;
            collectiblesInfo = colI;
            lock (remaining)
            {
                remaining = new List<CollectibleRepresentation>(collectiblesInfo);
            }
        }

        //implements abstract circle interface: provides the circle agent with a simulator to make predictions about the future level state
        public override void ActionSimulatorUpdated(ActionSimulator updatedSimulator)
        {
            predictor = updatedSimulator;
        }

        //implements abstract circle interface: signals if the agent is actually implemented or not
        public override bool ImplementedAgent()
        {
            return implementedAgent;
        }

        //implements abstract circle interface: provides the name of the agent to the agents manager in GeometryFriends
        public override string AgentName()
        {
            return agentName;
        }

        //simple algorithm for choosing a random action for the circle agent
        private void RandomAction()
        {
            /*
             Circle Actions
             ROLL_LEFT = 1      
             ROLL_RIGHT = 2
             JUMP = 3
             GROW = 4
            */
            int index = 40;
            double score = -1000000.0;
            int id = 0;
            float area_width = area.Width;
            float area_height = area.Height;

            foreach (ObstacleRepresentation ob_re in obstaclesInfo)
            {
                Brain.InputSignalArray[id] = NormalizeValue(ob_re.X, area_width);
                id++;
                Brain.InputSignalArray[id] = NormalizeValue(ob_re.Y, area_height);
                id++;
                Brain.InputSignalArray[id] = NormalizeValue(ob_re.Width, area_width);
                id++;
                Brain.InputSignalArray[id] = NormalizeValue(ob_re.Height, area_height);
                id++;
            }

            //for oscar
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.X, area_width);
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.Y, area_height);
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.VelocityX, 15.0f); //mark
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.VelocityY, area_height); //mark
            id++;

            /*
            name = new StreamWriter("C:/Users/Embajador/shit.txt", true);

            name.WriteLine("Velocity Y = " + circleInfo.VelocityY);
            name.WriteLine("Velocity X = " + circleInfo.VelocityX);

            name.Close();*/
            DebugSensorsInfo();
            //just chilling oscar

            //Console.WriteLine("banana martin");

            Brain.InputSignalArray[id] = NormalizeValue(uncaughtCollectibles[0].X, area.Width);
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(uncaughtCollectibles[0].Y, area.Height);
            Brain.Activate();

            //double a = nn.InputSignalArray[0];
            //double b = nn.InputSignalArray[1];

            for (int i = 0; i < 3; i++)
            {
                double actual_score = Brain.OutputSignalArray[i];

                if (actual_score > score)
                {
                    score = actual_score;
                    index = i;
                }
            }
            currentAction = possibleMoves[index];
            //double dist = Distance(circleInfo.X, circleInfo.Y, uncaughtCollectibles[0].X, uncaughtCollectibles[0].Y);
            name = new StreamWriter(Environment.CurrentDirectory + FITNESS_FILE, false);
            //name.WriteLine(uncaughtCollectibles.Count + ";" + dist);
            //name.Close();
        }

        public float NormalizeValue(float value, float max)
        {
            return value/(max);
        }

        public double Distance(float x1, float y1, float x2, float y2)
        {
            double dist = (Math.Abs(x2 - x1)) + (Math.Abs(y2 - y1));
            dist = Math.Sqrt(dist);
            return dist;
        }

        //implements abstract circle interface: GeometryFriends agents manager gets the current action intended to be actuated in the enviroment for this agent
        public override Moves GetAction()
        {
            RandomAction();
            return currentAction;
        }

        //implements abstract circle interface: updates the agent state logic and predictions
        public override void Update(TimeSpan elapsedGameTime)
        {
           RandomAction();
        }

        //typically used console debugging used in previous implementations of GeometryFriends
        protected void DebugSensorsInfo()
        {
            Console.WriteLine("Circle Agent - " + numbersInfo.ToString());

            Console.WriteLine("Circle Agent - " + rectangleInfo.ToString());

            Console.WriteLine("Circle Agent - " + circleInfo.ToString());

            foreach (ObstacleRepresentation i in obstaclesInfo)
            {
                Console.WriteLine("Circle Agent - " + i.ToString("Obstacle"));
            }

            foreach (ObstacleRepresentation i in rectanglePlatformsInfo)
            {
                Console.WriteLine("Circle Agent - " + i.ToString("Rectangle Platform"));
            }

            foreach (ObstacleRepresentation i in circlePlatformsInfo)
            {
                Console.WriteLine("Circle Agent - " + i.ToString("Circle Platform"));
            }

            foreach (CollectibleRepresentation i in collectiblesInfo)
            {
                Console.WriteLine("Circle Agent - " + i.ToString());
            }
        }

        //implements abstract circle interface: signals the agent the end of the current level
        public override void EndGame(int collectiblesCaught, int timeElapsed)
        {
            Console.WriteLine("CIRCLE - Collectibles caught = {0}, Time elapsed - {1}", collectiblesCaught, timeElapsed);

            name = new StreamWriter(Environment.CurrentDirectory + FITNESS_FILE, false);
            name.WriteLine(collectiblesCaught + "," + timeElapsed);
            name.Close();
        }

        //implements abstract circle interface: gets the debug information that is to be visually represented by the agents manager
        public override DebugInformation[] GetDebugInformation()
        {
            return debugInfo;
        }


    }


}
