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

namespace EvolutionGeometryFriends
{
    class Program
    {

        protected CyclicNetwork neural_network;
        protected NeatGenomeFactory genome_factory;
        protected NeatGenome genome;
        private Random rnd;

        List<Neuron> nodes = new List<Neuron>();
        List<Connection> connections = new List<Connection>();

        //SHARPNEAT Objects
        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        const string NEURAL_NETWORK_FILE = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/circle_neural_network.xml";

        static void Main(string[] args)
        {
            Program main_program = new Program(26,3);
            main_program.CreateNeuralNetwork();
            main_program.SaveNeuralNetwork();

            log4net.Config.XmlConfigurator.Configure();
            Log4NetController.Log("TEST", Log4NetController.LogLevel.Debug);

            try
            {
                string arguments = "--speed 75 -st 0 3 -a Agents/GeometryFriendsAgents.dll";
                string filename = Environment.CurrentDirectory + 
                                    "/../../../GeometryFriendsGame/Release/GeometryFriends.exe";

                //Process.Start("http://google.com/search?q=" + "cat pictures");

                Process firstProc = new Process();
                firstProc.StartInfo.FileName = filename;
                firstProc.StartInfo.Arguments = arguments;
                firstProc.EnableRaisingEvents = true;
                firstProc.Start();

                firstProc.WaitForExit();

                //You may want to perform different actions depending on the exit code.
                Console.WriteLine("First process exited: " + firstProc.ExitCode);
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred!!!: " + ex.Message);
                return;
            }
        }

        public Program(int input_count, int output_count)
        {
            NeatGenomeParameters _neatGenomeParams = new NeatGenomeParameters();
            _neatGenomeParams.AddConnectionMutationProbability = 0.1;
            _neatGenomeParams.AddNodeMutationProbability = 0.01;
            _neatGenomeParams.ConnectionWeightMutationProbability = 0.89;
            _neatGenomeParams.InitialInterconnectionsProportion = 0.05;

            genome_factory = new NeatGenomeFactory(input_count, output_count, _neatGenomeParams);
        }

        public void CreateNeuralNetwork()
        {
            genome = genome_factory.CreateGenome(0); 
        }

        public void SaveNeuralNetwork()
        {
            string filename = Environment.CurrentDirectory + NEURAL_NETWORK_FILE;
            var doc = NeatGenomeXmlIO.SaveComplete(
                                     new List<NeatGenome>() { genome },
                                     false);

            doc.Save(filename);
        }

        
    }
}
