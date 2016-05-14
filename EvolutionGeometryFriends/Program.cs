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
        #region variables
        private static GeometryFriendsEvolutionaryAlgorithm<NeatGenome> _gfea;
        public static List<double> fitness_values = new List<double>();

        private static StreamWriter streamError;
        private static bool errorsWritten = false;

        private static Process geometry_friends_process;
        private static bool stop = false;
        #endregion

        #region filePaths
        static string NEURAL_NETWORK_FILE = "/../../../neural_network_params/circle_neural_network.xml";
        static string CIRCLE_CHAMPION_FILE = "/../../../neural_network_params/circle_champion.xml";       
        static string TOTAL_FITNESS_FILE = "/../../../neural_network_params/full_fitness.txt";
        static string EA_CONFIG_FILE = "/../../../neural_network_params/geometryfriends.config.xml";

        static string ERROR_FILE = Environment.CurrentDirectory + "/../../../error-log/error.txt";
        static string SIMULATION_EXECUTABLE_FILENAME = Environment.CurrentDirectory + "/../../../GeometryFriendsGame/Release/GeometryFriends.exe";
        static string INDEX_FILE_PATH = Environment.CurrentDirectory + "/../../../neural_network_params/index_file.txt";
        static string FITNESS_FILE = Environment.CurrentDirectory + "/../../../neural_network_params/fitness.txt";
        static string LOG4NET_FILE = Environment.CurrentDirectory + "/../../../lib/log4net.properties";
        #endregion

        

        #region Interface

        public static void StopEvolution()
        {
            stop = true;
        }

        public static void RunIndividual(int index, int speed_value)
        {
            //We clean the data that will be written and read by the agent.
            ClearFiles(index);

            //We start the Game
            try
            {
                if (geometry_friends_process != null)
                    geometry_friends_process.Close();

                geometry_friends_process = new Process();
                geometry_friends_process.StartInfo.FileName = SIMULATION_EXECUTABLE_FILENAME;
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

        public static void RunEvolution(int speed, int nGenerations)
        {            
            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo(LOG4NET_FILE));


            // Write first header for Total fitness file
            using (StreamWriter sw = new StreamWriter(TOTAL_FITNESS_FILE, true))
            {
                sw.WriteLine("Generation;MaxFitness;MeanFitness");
            }

            //We set up the experiment & the evolutionary algorithm.
            GeometryFriendsExperiment experiment = new GeometryFriendsExperiment();
            XmlDocument xml_config = new XmlDocument();

            xml_config.Load(EA_CONFIG_FILE);

            // Flag used to stop evolution press from UI
            stop = false;

            experiment.Initialize("GeometryFriends", xml_config.DocumentElement);


            _gfea = experiment.CreateGFEvolutionAlgorithm();
            _gfea.UpdateEvent += new EventHandler(ea_UpdateEvent);
            SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);

            RunSimulation(speed, experiment.DefaultPopulationSize);
            _gfea.FirstEvaluation();

            //We need to be able to change the the argument of the simulations.

            for (int i = 0; i < nGenerations; i++)
            {
                // Stop the evolution if the flag is set
                if (stop)
                {
                    break;
                }
                // Create offspring based on fitness
                try
                {
                    _gfea.PerformGeneration();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error creating offsprings & evaluating" + ex.StackTrace);
                    throw new Exception("An error creating offsprings & evaluating", ex);
                }
                // Update neural network configurations in xml
                SaveNeuralNetwork((List<NeatGenome>)_gfea.GenomeList);
                // Run game to get fitness for new population
                RunSimulation(speed, experiment.DefaultPopulationSize);                
            }

         //   ApplicationForm.Instance.ChangeState();
        }

        public static void RunSimulation(int speed, int populationSize)
        {
            //We clean the data that will be written and read by the agent.
            ClearFiles(0);
            

            //We start the Game
            try
            {
                if(geometry_friends_process != null)
                    geometry_friends_process.Close();

                geometry_friends_process = new Process();
                geometry_friends_process.StartInfo.FileName = SIMULATION_EXECUTABLE_FILENAME;
                // Set UseShellExecute to false for redirection.
                geometry_friends_process.StartInfo.UseShellExecute = false;
                geometry_friends_process.EnableRaisingEvents = true;
                geometry_friends_process.StartInfo.Arguments = "--log-to-file --no-rendering --disable-fixed-time-step --speed " + speed + " --simulations " + populationSize + " -st 0 3 -a Agents/GeometryFriendsAgent.dll";
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
                using (StreamReader sr = new StreamReader(FITNESS_FILE))
                {
                    using (StreamWriter sw = new StreamWriter(TOTAL_FITNESS_FILE, true))
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

        public static void SetProjectPath(string folderPath)
        {
            NEURAL_NETWORK_FILE = folderPath + "/circle_neural_network.xml";
            CIRCLE_CHAMPION_FILE = folderPath + "/circle_champion.xml";
            TOTAL_FITNESS_FILE = folderPath + "/full_fitness.txt";
            EA_CONFIG_FILE = folderPath + "/geometryfriends.config.xml";
        }
        #endregion
        #region private
        private static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("gen={0:N0} bestFitness={1:N6}", _gfea.CurrentGeneration, _gfea.Statistics._maxFitness));

            try
            {
                // Save the best genome to file
                var doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { _gfea.CurrentChampGenome }, false);
                doc.Save(CIRCLE_CHAMPION_FILE);
            }
            catch (Exception e1)
            {
                MessageBox.Show("Problem when saving the chamapion network" + e1.Message);
                throw new Exception("Problem when saving the chamapion network", e1);
            }

        }

        private static void SaveNeuralNetwork(List<NeatGenome> gl)
        {
            string filename = NEURAL_NETWORK_FILE;

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

        private static void ClearFiles(int index)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(INDEX_FILE_PATH, false))
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
                using (StreamWriter sw = new StreamWriter(FITNESS_FILE, false))
                {
                    sw.Write("");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem when clearing the fitness file" + e.Message);
                throw new Exception("Problem when clearing the fitness file", e);
            }
        }

        #region Helpers
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
                            streamError = new StreamWriter(ERROR_FILE, true);
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

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
        }
        #endregion
        #endregion
    }
}
