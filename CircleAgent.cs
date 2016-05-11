﻿using System;
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
        const string INDEX_FILE_PATH = "/Agents/neural_network_params/index_file.txt";
        const string FITNESS_FILE = "/fitness.txt";


        System.IO.StreamWriter writer;
        int index_id;
        //System.IO.StreamReader reader;

        
        public CircleAgent()
        {

            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + INDEX_FILE_PATH))
            {
                index_id = Int32.Parse(sr.ReadLine());
            }

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
                    genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, ngf)[index_id]; // this is the index to change
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

            //Feed input neuron with each obstacle
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

            //Feed input neurons with agent information
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.X, area_width);
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.Y, area_height);
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.VelocityX, 15.0f); //mark
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(circleInfo.VelocityY, area_height); //mark
            id++;

            //Feed input neurons with first uncaught collectible information
            Brain.InputSignalArray[id] = NormalizeValue(uncaughtCollectibles[0].X, area.Width);
            id++;
            Brain.InputSignalArray[id] = NormalizeValue(uncaughtCollectibles[0].Y, area.Height);
            
            //Evaluate neural network
            Brain.Activate();

            //Get most excited output neuron
            for (int i = 0; i < 3; i++)
            {
                double current_score = Brain.OutputSignalArray[i];

                if (current_score > score)
                {
                    score = current_score;
                    index = i;
                }
            }

            //Select action with output neuron
            currentAction = possibleMoves[index];

        }

        public float NormalizeValue(float value, float max)
        {
            return value/(max);
        }

        public double NormalizeValue(double value, double max)
        {
            return value / (max);
        }

        public double Distance(float x1, float y1, float x2, float y2)
        {
            double dist = (Math.Abs(x2 - x1)) + (Math.Abs(y2 - y1));
            dist = Math.Sqrt(dist);
            return dist;
        }

        public double NormalizedDistance(float x1, float y1, float x2, float y2)
        {
            double dist = (Math.Abs(x2 - x1)) + (Math.Abs(y2 - y1));
            dist = Math.Sqrt(dist);
            return NormalizeValue(dist, Math.Sqrt(Math.Pow(area.Width, 2.0) + Math.Pow(area.Height, 2.0)));
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

            float normalized_collectibles = NormalizeValue((float)collectiblesCaught, (float)numbersInfo.CollectiblesCount);
            double normalized_distance = NormalizedDistance(circleInfo.X, circleInfo.Y, uncaughtCollectibles[0].X, uncaughtCollectibles[0].Y);
            float normalized_time = NormalizeValue((float)timeElapsed, 55.0f);

            float weighted_sum = (100.0f * normalized_collectibles) + 
                                (float)(10.0 * normalized_distance) + 
                                normalized_time;

            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + FITNESS_FILE, true))
            {
                sw.WriteLine(weighted_sum);
            }

            /*
            //Debug purpouse
            writer.WriteLine("Collectibles caught: " + collectiblesCaught + ", Collectibles count: " + numbersInfo.CollectiblesCount);
            writer.WriteLine("Normalized colleectables: " + normalized_collectibles);
            writer.WriteLine("X: " + circleInfo.X + ", Y: " + circleInfo.Y + ", X2: " + uncaughtCollectibles[0].X + ", Y2: " + uncaughtCollectibles[0].Y);
            writer.WriteLine("Normalized distance: " + normalized_distance);
            writer.WriteLine("Time: " + timeElapsed + ", Normalized time: " + normalized_time);
            writer.WriteLine();
             */

            index_id++;

            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + INDEX_FILE_PATH, false))
            {
                sw.WriteLine(index_id);
            }
        }

        //implements abstract circle interface: gets the debug information that is to be visually represented by the agents manager
        public override DebugInformation[] GetDebugInformation()
        {
            return debugInfo;
        }


    }


}
