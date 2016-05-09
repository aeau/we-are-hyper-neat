using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
using System.IO;
using System.Xml;
using log4net.Config;

namespace EvolutionGeometryFriends
{
    public class Program
    {

        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        static GeometryFriendsEvolutionaryAlgorithm<NeatGenome> _gfea;

        public static List<double> fitness_values = new List<double>();

        protected CyclicNetwork neural_network;
        protected NeatGenomeFactory genome_factory;
        protected NeatGenome genome;
        protected List<NeatGenome> genome_list = new List<NeatGenome>();
        private Random rnd;

        List<Neuron> nodes = new List<Neuron>();
        List<Connection> connections = new List<Connection>();

        public static int current_index;

        //SHARPNEAT Objects
        const string NEURAL_NETWORK_FILE = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/circle_neural_network.xml";
        const string CIRCLE_CHAMPION_FILE = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/circle_champion.xml";

        const string FITNESS_FILE = "/../../../GeometryFriendsGame/Release/fitness.txt";
        System.IO.StreamReader reader;
        System.IO.StreamWriter writer;

        static void Main(string[] args)
        {
            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo(Environment.CurrentDirectory + "/../../../lib/log4net.properties"));

            //log4net.Config.XmlConfigurator.Configure();

            //We set up the experiment & the evolutionary algorithm.
            GeometryFriendsExperiment experiment = new GeometryFriendsExperiment();
            XmlDocument xml_config = new XmlDocument();

            xml_config.Load(Environment.CurrentDirectory +
                                    "/../../../lib/geometryfriends.config.xml");

            experiment.Initialize("GeometryFriends", xml_config.DocumentElement);

            //_ea = experiment.CreateEvolutionAlgorithm();
            //_ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
            //_ea.StartContinue();

            _gfea = experiment.CreateGFEvolutionAlgorithm();

            Console.WriteLine("genome count = " + _gfea._genomeList.Count);

            _gfea.FirstEvaluation();
            _gfea.UpdateEvent += new EventHandler(ea_UpdateEvent);
 
            //_gfea.PerformOneStep();


            
            string INDEX_FILE_PATH = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/index_file.txt";
            System.IO.StreamWriter index_file;

            //Program main_program = new Program(26,3);
            //main_program.CreateNeuralNetwork();
            //main_program.SaveNeuralNetwork();

            try
            {
                //We need to be able to change the the argument of the simulations.
                string filename = Environment.CurrentDirectory +
                                    "/../../../GeometryFriendsGame/Release/gflink";

                SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);

                for (int i = 0; i < 5; i++)
                {
                    index_file = new StreamWriter(Environment.CurrentDirectory + INDEX_FILE_PATH, false);
                    index_file.WriteLine("0");
                    index_file.Close();

                    //We start the Game
                    Process firstProc = new Process();
                    firstProc.StartInfo.FileName = filename;
                    firstProc.Start();
                    firstProc.WaitForExit();

                    if (fitness_values != null)
                        fitness_values.Clear();

                    using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + FITNESS_FILE))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(sr.ReadLine());
                            fitness_values.Add(Double.Parse(sr.ReadLine()));
                        }
                    }

                    _gfea.Evaluate();
                    _gfea.CreateOffsprings();
                    SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);

                    System.IO.File.WriteAllText(Environment.CurrentDirectory + FITNESS_FILE, string.Empty);

                }
                
                //You may want to perform different actions depending on the exit code.
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred!!!: " + ex.Message);
                return;
            }
        }

        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("gen={0:N0} bestFitness={1:N6}", _gfea.CurrentGeneration, _gfea.Statistics._maxFitness));

            // Save the best genome to file
            var doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { _gfea.CurrentChampGenome }, false);
            doc.Save(Environment.CurrentDirectory + CIRCLE_CHAMPION_FILE);
        }

        public static void SaveNeuralNetwork(List<NeatGenome> gl)
        {
            string filename = Environment.CurrentDirectory + NEURAL_NETWORK_FILE;
            var doc = NeatGenomeXmlIO.SaveComplete(
                                     gl,
                                     false);

            doc.Save(filename);
        }



        public Program(int input_count, int output_count)
        {
            GeometryFriendsExperiment experiment = new GeometryFriendsExperiment();
            XmlDocument xml_config = new XmlDocument();

            xml_config.Load(Environment.CurrentDirectory +
                                    "/../../../lib/geometryfriends.config.xml");
            experiment.Initialize("GeometryFriends", xml_config.DocumentElement);

            NeatGenomeParameters _neatGenomeParams = new NeatGenomeParameters();
            _neatGenomeParams.AddConnectionMutationProbability = 0.1;
            _neatGenomeParams.AddNodeMutationProbability = 0.01;
            _neatGenomeParams.ConnectionWeightMutationProbability = 0.89;
            _neatGenomeParams.InitialInterconnectionsProportion = 0.05;

            genome_factory = new NeatGenomeFactory(input_count, output_count, _neatGenomeParams);
        }

        public void CreateNeuralNetwork()
        {
            //genome = genome_factory.CreateGenome(0);
            genome_list = genome_factory.CreateGenomeList(100, 0);
        }



        
    }
}
