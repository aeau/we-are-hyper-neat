using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using SharpNeat.Phenomes;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.HyperNeat;
using SharpNeat.DistanceMetrics;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using SharpNeat.SpeciationStrategies;
using System.IO;

namespace EvolutionGeometryFriends
{
    public class GeometryFriendsEvaluator :  IPhenomeEvaluator<IBlackBox>
    {
        private ulong _evalCount;
        private bool _stopConditionSatisfied;

        string filename = Environment.CurrentDirectory +
                                    "/../../../GeometryFriendsGame/Release/gflink";

        const string FITNESS_FILE = "/../../../GeometryFriendsGame/Release/fitness.txt";
        const string ACTUAL_NETWORK_FILE = "/../../../GeometryFriendsGame/Release/Agents/neural_network_params/current_circle_network.xml";

        #region IPhenomeEvaluator<IBlackBox> Members

        /// <summary>
        /// Gets the total number of evaluations that have been performed.
        /// </summary>
        public ulong EvaluationCount
        {
            get { return _evalCount; }
        }

        /// <summary>
        /// Gets a value indicating whether some goal fitness has been achieved and that
        /// the the evolutionary algorithm/search should stop. This property's value can remain false
        /// to allow the algorithm to run indefinitely.
        /// </summary>
        public bool StopConditionSatisfied
        {
            get { return _stopConditionSatisfied; }
        }

        public FitnessInfo Evaluate(int index)
        {
            FitnessInfo fi = new FitnessInfo(Program.fitness_values[index], Program.fitness_values[index]);
            _evalCount++;

            return fi;
        }


        /// <summary>
        /// Evaluate the provided IBlackBox against the random tic-tac-toe player and return its fitness score.
        /// Each network plays 10 games against the random player and two games against the expert player.
        /// Half of the games are played as circle and half are played as x.
        /// 
        /// A win is worth 10 points, a draw is worth 1 point, and a loss is worth 0 points.
        /// </summary>
        public FitnessInfo Evaluate(IBlackBox box)
        {
            double fitness;
            //SaveNeuralNetwork(box);

            //We run the game with the neural network
            Process firstProc = new Process();
            firstProc.StartInfo.FileName = filename;
            firstProc.Start();
            firstProc.WaitForExit();

            using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + FITNESS_FILE))
            {
                fitness = Double.Parse(sr.ReadLine());
            }

            Console.WriteLine("fitness is: " + fitness);
            return new FitnessInfo(fitness, fitness);


            /*
            if( Program.fitness_counter >= Program.fitness_values.Count)
            {
                _evalCount++;
                return new FitnessInfo(0.0, 0.0);                
            }

            FitnessInfo fi = new FitnessInfo(Program.fitness_values[Program.fitness_counter], Program.fitness_values[Program.fitness_counter]);
            _evalCount++;
            Program.fitness_counter++;

            return fi;
            */
        }

        public void SaveNeuralNetwork(IBlackBox brain)
        {
            string filename = Environment.CurrentDirectory + ACTUAL_NETWORK_FILE;
            var doc = NeatGenomeXmlIO.SaveComplete(
                                     new List<NeatGenome>() { (NeatGenome)brain},
                                     false);

            doc.Save(filename);
        }

        /*

        /// <summary>
        /// Returns the score for a game. Scoring is 10 for a win, 1 for a draw
        /// and 0 for a loss. Note that scores cannot be smaller than 0 because
        /// NEAT requires the fitness score to be positive.
        /// </summary>
        private int getScore(SquareTypes winner, SquareTypes neatSquareType)
        {
            if (winner == neatSquareType)
                return 10;
            if (winner == SquareTypes.N)
                return 1;
            return 0;
        }
        */
        /// <summary>
        /// Reset the internal state of the evaluation scheme if any exists.
        /// Note. The TicTacToe problem domain has no internal state. This method does nothing.
        /// </summary>
        public void Reset()
        {
        }

        #endregion

    }
}
