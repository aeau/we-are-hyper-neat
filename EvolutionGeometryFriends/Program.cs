using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EvolutionGeometryFriends
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log4NetController.Log("TEST", Log4NetController.LogLevel.Debug);
            try
            {
                string arguments = @"--no-rendering --speed 75 -st 0 3 -a Agents/GeometryFriendsAgents.dll";
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
    }
}
