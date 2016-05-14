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
using System.Windows.Forms;

namespace EvolutionGeometryFriends
{
    public class Program
    {

        static GeometryFriendsEvolutionaryAlgorithm<NeatGenome> _gfea;
        public static List<double> fitness_values = new List<double>();

        public static StreamWriter streamError;
        public static bool errorsWritten = false;

        public static Process geometry_friends_process;

        //Error file
        const string ERROR_FILE = "/../../../error-log/error.txt";

        //SHARPNEAT Objects
        const string NEURAL_NETWORK_FILE = "/../../../neural_network_params/circle_neural_network.xml";
        const string CIRCLE_CHAMPION_FILE = "/../../../neural_network_params/circle_champion.xml";
        const string SIMULATION_EXECUTABLE_FILENAME = "/../../../GeometryFriendsGame/Release/GeometryFriends.exe";
        const string SIMPLE_EXECUTABLE_FILENAME = "/../../../GeometryFriendsGame/Release/gflink_simple";
        const string INDEX_FILE_PATH = "/../../../neural_network_params/index_file.txt";
        const string FITNESS_FILE = "/../../../neural_network_params/fitness.txt";
        const string TOTAL_FITNESS_FILE = "/../../../neural_network_params/full_fitness.txt";


        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
        }

        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo(Environment.CurrentDirectory + "/../../../lib/log4net.properties"));

            //We set up the experiment & the evolutionary algorithm.
            GeometryFriendsExperiment experiment = new GeometryFriendsExperiment();
            XmlDocument xml_config = new XmlDocument();

            xml_config.Load(Environment.CurrentDirectory +
                                    "/../../../neural_network_params/geometryfriends.config.xml");

            experiment.Initialize("GeometryFriends", xml_config.DocumentElement);

            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + TOTAL_FITNESS_FILE, true))
            {
                sw.WriteLine("Generation;MaxFitness;MeanFitness");
            }

            if(args.Length > 1 && args[1].Equals("true"))
            {
                for (; ; )
                {
                    RunProgram(int.Parse(args[2]), 11);
                }
            }
            else if (args.Length > 1 && args[1].Equals("false"))
            {
                _gfea = experiment.CreateGFEvolutionAlgorithm();
                _gfea.UpdateEvent += new EventHandler(ea_UpdateEvent);
                SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);

                PerformEvolutionProcess();
                _gfea.FirstEvaluation();

                //We need to be able to change the the argument of the simulations.

                for (int i = 0; i < int.Parse(args[0]); i++)
                {
                    PerformEvolutionProcess();
                    try
                    {
                        _gfea.PerformGeneration();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error creating offsprings & evaluating" + ex.StackTrace);
                        throw new Exception("An error creating offsprings & evaluating", ex);
                    }

                    SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);
                    Thread.Sleep(1000);
                }
                
               
            }
            else
            {
                Console.WriteLine("You mistoke nig");
                Console.WriteLine("You must write 'true' for executing the game with the champion");
                Console.WriteLine("You must write 'false' for evolution");

            }

            Console.ReadLine();
        }

        static void RunProgram(int index, int speed_value)
        {
            //We clean the data that will be written and read by the agent.
            try
            {
                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + INDEX_FILE_PATH, false))
                {
                    sw.Write(index.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem when initializing index value" + e.Message);
                throw new Exception("Problem when initializing index value", e);
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + FITNESS_FILE, false))
                {
                    sw.Write("");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem when clearing the fitness file" + e.Message);
                throw new Exception("Problem when clearing the fitness file", e);
            }

            //We start the Game
            try
            {
                if (geometry_friends_process != null)
                    geometry_friends_process.Close();

                geometry_friends_process = new Process();
                geometry_friends_process.StartInfo.FileName = Environment.CurrentDirectory + SIMULATION_EXECUTABLE_FILENAME;
                // Set UseShellExecute to false for redirection.
                geometry_friends_process.StartInfo.UseShellExecute = false;
                geometry_friends_process.EnableRaisingEvents = true;
                geometry_friends_process.StartInfo.Arguments = "--log-to-file --disable-fixed-time-step --speed " + speed_value + " -st 0 3 -a Agents/GeometryFriendsAgent.dll";
                geometry_friends_process.Exited += new EventHandler(myProcess_Exited);

                // Redirect the error output of the net command. 
                geometry_friends_process.StartInfo.RedirectStandardError = true;
                geometry_friends_process.ErrorDataReceived += new DataReceivedEventHandler(NetErrorDataHandler);
                geometry_friends_process.Start();

                // Start the asynchronous read of the standard output stream.
                geometry_friends_process.BeginErrorReadLine();
                geometry_friends_process.WaitForExit();

                geometry_friends_process.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error executing the GeometryFriends.exe" + ex.Message);
                throw new Exception("An error executing the GeometryFriends.exe", ex);
            }
            
        }

        static void PerformEvolutionProcess()
        {
            //We clean the data that will be written and read by the agent.
            try
            {
                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + INDEX_FILE_PATH, false))
                {
                    sw.Write("0");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem when initializing index value" + e.Message);
                throw new Exception("Problem when initializing index value", e);
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + FITNESS_FILE, false))
                {
                    sw.Write("");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem when clearing the fitness file" + e.Message);
                throw new Exception("Problem when clearing the fitness file", e);
            }
            

            //We start the Game
            try
            {
                if(geometry_friends_process != null)
                    geometry_friends_process.Close();

                geometry_friends_process = new Process();
                geometry_friends_process.StartInfo.FileName = Environment.CurrentDirectory + SIMULATION_EXECUTABLE_FILENAME;
                // Set UseShellExecute to false for redirection.
                geometry_friends_process.StartInfo.UseShellExecute = false;
                geometry_friends_process.EnableRaisingEvents = true;
                geometry_friends_process.StartInfo.Arguments = "--log-to-file --disable-fixed-time-step --speed 11 --simulations 20 -st 0 3 -a Agents/GeometryFriendsAgent.dll";
                geometry_friends_process.Exited += new EventHandler(myProcess_Exited);

                // Redirect the error output of the net command. 
                geometry_friends_process.StartInfo.RedirectStandardError = true;
                geometry_friends_process.ErrorDataReceived += new DataReceivedEventHandler(NetErrorDataHandler);
                geometry_friends_process.Start();

                // Start the asynchronous read of the standard output stream.
                geometry_friends_process.BeginErrorReadLine();
                geometry_friends_process.WaitForExit();

                geometry_friends_process.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error executing the GeometryFriends.exe" + ex.Message);
                throw new Exception("An error executing the GeometryFriends.exe", ex);
            }

            if (fitness_values != null)
                fitness_values.Clear();

            try
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + FITNESS_FILE))
                {
                    using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + TOTAL_FITNESS_FILE, true))
                    {
                        sw.WriteLine(string.Format("{0:N0};{1:N6};{2:N6}", _gfea.CurrentGeneration, _gfea.Statistics._maxFitness, _gfea.Statistics._meanFitness));
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            //sw.WriteLine(line);
                            Console.WriteLine(line);
                            fitness_values.Add(Double.Parse(line));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem when reading each fitness value" + e.Message);
                throw new Exception("Problem when reading each fitness value", e);
            }

        }

        public static void NetErrorDataHandler(object sendingProcess,
            DataReceivedEventArgs errLine)
        {
            // Write the error text to the file if there is something
            // to write and an error file has been specified.

            if (!String.IsNullOrEmpty(errLine.Data))
            {
                if (!errorsWritten)
                {
                    if (streamError == null)
                    {
                        // Open the file.
                        try
                        {
                            streamError = new StreamWriter(Environment.CurrentDirectory + ERROR_FILE, true);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Could not open error file!");
                            Console.WriteLine(e.Message.ToString());
                        }
                    }

                    if (streamError != null)
                    {
                        // Write a header to the file if this is the first
                        // call to the error output handler.
                        streamError.WriteLine();
                        streamError.WriteLine(DateTime.Now.ToString());
                        streamError.WriteLine("Geoemtry Friends error output:");
                    }
                    errorsWritten = true;
                }

                if (streamError != null)
                {
                    // Write redirected errors to the file.
                    streamError.WriteLine(errLine.Data);
                    streamError.Flush();
                }
            }
        }

        // Handle Exited event and display process information.
        public static void myProcess_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("gen={0:N0} bestFitness={1:N6}", _gfea.CurrentGeneration, _gfea.Statistics._maxFitness));

            try
            {
                // Save the best genome to file
                var doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { _gfea.CurrentChampGenome }, false);
                doc.Save(Environment.CurrentDirectory + CIRCLE_CHAMPION_FILE);
            }
            catch (Exception e1)
            {
                MessageBox.Show("Problem when saving the chamapion network" + e1.Message);
                throw new Exception("Problem when saving the chamapion network", e1);
            }
            
        }

        public static void SaveNeuralNetwork(List<NeatGenome> gl)
        {
            string filename = Environment.CurrentDirectory + NEURAL_NETWORK_FILE;

            try
            {
                var doc = NeatGenomeXmlIO.SaveComplete(
                                    gl,
                                    false);

                doc.Save(filename);
            }
            catch (Exception e1)
            {
                MessageBox.Show("Problem when saving the complete list of genomes" + e1.Message);
                throw new Exception("Problem when saving the complete list of genomes", e1);
            }


           
        }

    }
}
