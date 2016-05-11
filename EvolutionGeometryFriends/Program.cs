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
using System.Threading;

namespace EvolutionGeometryFriends
{
    public class Program
    {

        static GeometryFriendsEvolutionaryAlgorithm<NeatGenome> _gfea;
        public static List<double> fitness_values = new List<double>();

        //SHARPNEAT Objects
        const string NEURAL_NETWORK_FILE = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/circle_neural_network.xml";
        const string CIRCLE_CHAMPION_FILE = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/circle_champion.xml";
        const string SIMULATION_EXECUTABLE_FILENAME = "/../../../GeometryFriendsGame/Release/gflink";
        const string SIMPLE_EXECUTABLE_FILENAME = "/../../../GeometryFriendsGame/Release/gflink_simple";
        const string INDEX_FILE_PATH = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/index_file.txt";
        const string FITNESS_FILE = "/../../../GeometryFriendsGame/Release/fitness.txt";

        static void Main(string[] args)
        {
            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo(Environment.CurrentDirectory + "/../../../lib/log4net.properties"));

            //We set up the experiment & the evolutionary algorithm.
            GeometryFriendsExperiment experiment = new GeometryFriendsExperiment();
            XmlDocument xml_config = new XmlDocument();

            xml_config.Load(Environment.CurrentDirectory +
                                    "/../../../lib/geometryfriends.config.xml");

            experiment.Initialize("GeometryFriends", xml_config.DocumentElement);

            if(args.Length > 1 && args[1].Equals("true"))
            {
                RunProgram();
            }
            else if (args.Length > 1 && args[1].Equals("false"))
            {
                _gfea = experiment.CreateGFEvolutionAlgorithm();
                _gfea.UpdateEvent += new EventHandler(ea_UpdateEvent);
                SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);

                PerformEvolutionProcess();
                _gfea.FirstEvaluation();

                try
                {
                    //We need to be able to change the the argument of the simulations.

                    for (int i = 0; i < int.Parse(args[0]); i++)
                    {
                        PerformEvolutionProcess();
                        _gfea.PerformGeneration();
                        SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);
                        Thread.Sleep(1000);
                    }

                    //You may want to perform different actions depending on the exit code.

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred!!!: " + ex.Message);
                    return;
                }
            }
            else
            {
                Console.WriteLine("You mistoke nig");
                Console.WriteLine("You must write 'true' for executing the game with the champion");
                Console.WriteLine("You must write 'false' for evolution");

            }
        }

        static void RunProgram()
        {
            //We clean the data that will be written and read by the agent.
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + INDEX_FILE_PATH, false))
            {
                sw.WriteLine("0");
            }

            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + FITNESS_FILE, false))
            {
                sw.Write("");
            }

            //We start the Game
            Process firstProc = new Process();
            firstProc.StartInfo.FileName = Environment.CurrentDirectory + SIMPLE_EXECUTABLE_FILENAME;
            firstProc.Start();
            firstProc.WaitForExit();
        }

        static void PerformEvolutionProcess()
        {

            //We clean the data that will be written and read by the agent.
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + INDEX_FILE_PATH, false))
            {
                sw.WriteLine("0");
            }

            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + FITNESS_FILE,false))
            {
                sw.Write("");
            }

            //We start the Game
            Process firstProc = new Process();
            firstProc.StartInfo.FileName = Environment.CurrentDirectory + SIMULATION_EXECUTABLE_FILENAME;
            firstProc.Start();
            firstProc.WaitForExit();

            if (fitness_values != null)
                fitness_values.Clear();

            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + FITNESS_FILE))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    fitness_values.Add(Double.Parse(line));
                }
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

    }
}
