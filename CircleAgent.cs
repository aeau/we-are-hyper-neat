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

using GeometryFriends.AI.Interfaces;
using System.Drawing;
using GeometryFriends.AI.ActionSimulation;
using GeometryFriends.AI;
using GeometryFriends.AI.Debug;
using GeometryFriends.AI.Perceptions.Information;

using System.Runtime.InteropServices;

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
        protected CyclicNetwork nn;
        List<Neuron> nodes = new List<Neuron>();

        //SHARPNEAT Objects
        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        const string NEURAL_NETWORK_FILE = "circle_neural_network.xml";

        System.IO.StreamWriter name;
        
        public CircleAgent()
        {
            /*
            NeatGenome genome = null;

            // Try to load the genome from the XML document.
            try
            {
                using (XmlReader xr = XmlReader.Create(NEURAL_NETWORK_FILE))
                    genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false)[0];
            }
            catch (Exception e1)
            {
                return;
            }

            // Get a genome decoder that can convert genomes to phenomes.
            var genomeDecoder = _experiment.CreateGenomeDecoder();

            // Decode the genome into a phenome (neural network).
            var phenome = genomeDecoder.Decode(genome);

            // Set the NEAT player's brain to the newly loaded neural network.
            Brain = phenome;

            Brain.InputSignalArray[0] = 0;
            */
            
           

            /*
             //SharpNEAT values

            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo("log4net.properties"));

            GeometryFriendsExperiment experiment = new GeometryFriendsExperiment();

            //Load config XML
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("geometryfriends.config.xml");
            experiment.Initialize("GeometryFriends", xmlConfig.DocumentElement);

            // Create evolution algorithm and attach update event.
            _ea = experiment.CreateEvolutionAlgorithm();
            _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);

            // Start algorithm (it will run on a background thread).
            _ea.StartContinue();

            

            NeatEvolutionAlgorithmParameters eaParams = new NeatEvolutionAlgorithmParameters();
            //Brain = new 
            //IBlackBox _b;
            */

            //console
            //AllocConsole();


            rnd = new Random();
            Linear l = new Linear();
            SteepenedSigmoid ss = new SteepenedSigmoid();
            uint id = 0;
            
            Neuron n = new Neuron(id, NodeType.Bias, ss, null);
            nodes.Add(n);

            for(uint i = 0; i < 26; i++, id++)
            {
                n = new Neuron(id, NodeType.Input, ss, null);
                nodes.Add(n);
            }

            for (uint i = 0; i < 4; i++, id++)
            {
                n = new Neuron(id, NodeType.Output, ss, null);
                nodes.Add(n);
            }

            List<Connection> connections = new List<Connection>();

            for(int i = 1; i < 27; i++)
            {
                for(int k = 27; k < 31; k++)
                {
                    Connection cn = new Connection(nodes[i], nodes[k], rnd.NextDouble());
                    connections.Add(cn);
                }
            }

            nn = new CyclicNetwork(nodes, connections, 26, 4, 2);
            

            /*
            // Save the best genome to file
            var doc = NeatGenomeXmlIO.SaveComplete(
                                     new List<NeatGenome>() { _ea.CurrentChampGenome },
                                     false);
            doc.Save(NEURAL_NETWORK_FILE);
            */

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
            possibleMoves.Add(Moves.GROW);

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
                nn.InputSignalArray[id] = NormalizeValue(ob_re.X, area_width);
                id++;
                nn.InputSignalArray[id] = NormalizeValue(ob_re.Y, area_height);
                id++;
                nn.InputSignalArray[id] = NormalizeValue(ob_re.Width, area_width);
                id++;
                nn.InputSignalArray[id] = NormalizeValue(ob_re.Height, area_height);
                id++;
            }

            //for oscar
            nn.InputSignalArray[id] = NormalizeValue(circleInfo.X, area_width);
            id++;
            nn.InputSignalArray[id] = NormalizeValue(circleInfo.Y, area_height);
            id++;
            nn.InputSignalArray[id] = NormalizeValue(circleInfo.VelocityX, 15.0f); //mark
            id++;
            nn.InputSignalArray[id] = NormalizeValue(circleInfo.VelocityY, area_height); //mark
            id++;

            /*
            name = new StreamWriter("C:/Users/Embajador/shit.txt", true);

            name.WriteLine("Velocity Y = " + circleInfo.VelocityY);
            name.WriteLine("Velocity X = " + circleInfo.VelocityX);

            name.Close();*/
            DebugSensorsInfo();
            //just chilling oscar

            Console.WriteLine("banana martin");

            nn.InputSignalArray[id] = NormalizeValue(uncaughtCollectibles[0].X, area.Width);
            id++;
            nn.InputSignalArray[id] = NormalizeValue(uncaughtCollectibles[0].Y, area.Height);
            nn.Activate();

            //double a = nn.InputSignalArray[0];
            //double b = nn.InputSignalArray[1];

            for (int i = 0; i < 4; i++)
            {
                double actual_score = nn.OutputSignalArray[i];

                if (actual_score > score)
                {
                    score = actual_score;
                    index = i;
                }
            }
            currentAction = possibleMoves[index];
        }

        public float NormalizeValue(float value, float max)
        {
            return value/(max);
        }

        //implements abstract circle interface: GeometryFriends agents manager gets the current action intended to be actuated in the enviroment for this agent
        public override Moves GetAction()
        {
            

            return currentAction;
        }

        //implements abstract circle interface: updates the agent state logic and predictions
        public override void Update(TimeSpan elapsedGameTime)
        {
            //Every second one new action is choosen
            if (lastMoveTime == 60)
                lastMoveTime = 0;

            if ((lastMoveTime) <= (DateTime.Now.Second) && (lastMoveTime < 60))
            {
                if (!(DateTime.Now.Second == 59))
                {
                    RandomAction();
                    lastMoveTime = lastMoveTime + 1;
                    //DebugSensorsInfo();                    
                }
                else
                    lastMoveTime = 60;
            }

            //check if any collectible was caught
            lock (remaining)
            {
                if (remaining.Count > 0)
                {
                    List<CollectibleRepresentation> toRemove = new List<CollectibleRepresentation>();
                    foreach (CollectibleRepresentation item in uncaughtCollectibles)
                    {
                        if (!remaining.Contains(item))
                        {
                            caughtCollectibles.Add(item);
                            toRemove.Add(item);
                        }
                    }
                    foreach (CollectibleRepresentation item in toRemove)
                    {
                        uncaughtCollectibles.Remove(item);
                    }
                }
            }

            //predict what will happen to the agent given the current state and current action
            if (predictor != null) //predictions are only possible where the agents manager provided
            {
                /*
                 * 1) simulator can only be properly used when the Circle and Rectangle characters are ready, this must be ensured for smooth simulation
                 * 2) in this implementation we only wish to simulate a future state when whe have a fresh simulator instance, i.e. the generated debug information is empty
                */
                if (predictor.CharactersReady() && predictor.SimulationHistoryDebugInformation.Count == 0)
                {
                    List<CollectibleRepresentation> simCaughtCollectibles = new List<CollectibleRepresentation>();
                    //keep a local reference to the simulator so that it can be updated even whilst we are performing simulations
                    ActionSimulator toSim = predictor;

                    //prepare the desired debug information (to observe this information during the game press F1)
                    toSim.DebugInfo = true;
                    //you can also select the type of debug information generated by the simulator to circle only, rectangle only or both as it is set by default
                    //toSim.DebugInfoSelected = ActionSimulator.DebugInfoMode.Circle;

                    //setup the current circle action in the simulator
                    toSim.AddInstruction(currentAction);

                    //register collectibles that are caught during simulation
                    toSim.SimulatorCollectedEvent += delegate(Object o, CollectibleRepresentation col) { simCaughtCollectibles.Add(col); };

                    //simulate 2 seconds (predict what will happen 2 seconds ahead)
                    toSim.Update(2);

                    //prepare all the debug information to be passed to the agents manager
                    List<DebugInformation> newDebugInfo = new List<DebugInformation>();
                    //clear any previously passed debug information (information passed to the manager is cumulative unless cleared in this way)
                    newDebugInfo.Add(DebugInformationFactory.CreateClearDebugInfo());
                    //add all the simulator generated debug information about circle/rectangle predicted paths
                    newDebugInfo.AddRange(toSim.SimulationHistoryDebugInformation);
                    //create additional debug information to visualize collectibles that have been predicted to be caught by the simulator
                    foreach (CollectibleRepresentation item in simCaughtCollectibles)
                    {
                        newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(item.X - debugCircleSize / 2, item.Y - debugCircleSize / 2), debugCircleSize, GeometryFriends.XNAStub.Color.Red));
                        newDebugInfo.Add(DebugInformationFactory.CreateTextDebugInfo(new PointF(item.X, item.Y), "Predicted catch!", GeometryFriends.XNAStub.Color.White));
                    }
                    //create additional debug information to visualize collectibles that have already been caught by the agent
                    foreach (CollectibleRepresentation item in caughtCollectibles)
                    {
                        newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(item.X - debugCircleSize / 2, item.Y - debugCircleSize / 2), debugCircleSize, GeometryFriends.XNAStub.Color.GreenYellow));
                    }
                    //set all the debug information to be read by the agents manager
                    debugInfo = newDebugInfo.ToArray();
                }
            }
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
        }

        //implements abstract circle interface: gets the debug information that is to be visually represented by the agents manager
        public override DebugInformation[] GetDebugInformation()
        {
            return debugInfo;
        }


    }


}
