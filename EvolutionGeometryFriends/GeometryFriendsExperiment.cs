using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


using SharpNeat.Domains;
using System.Threading.Tasks;
using SharpNeat.Core;
using SharpNeat.Decoders.Neat;
using System.Xml;

namespace EvolutionGeometryFriends
{
    public class GeometryFriendsExperiment : INeatExperiment
    {
        NeatEvolutionAlgorithmParameters _eaParams;
        NeatGenomeParameters _neatGenomeParams;
        string _name;
        int _populationSize;
        int _specieCount;
        NetworkActivationScheme _activationScheme;
        string _complexityRegulationStr;
        int? _complexityThreshold;
        string _description;
        ParallelOptions _parallelOptions;

        public GeometryFriendsEvaluator PhenomeEvaluator
        {
            get { return new GeometryFriendsEvaluator(); }
        }

        /// <summary>
        /// Defines the number of input nodes in the neural network.
        /// The network has one input for each square on the board,
        /// so it has 9 inputs total.
        /// </summary>
        public int InputCount
        {
            get { return 26; }
        }

        /// <summary>
        /// Defines the number of output nodes in the neural network.
        /// The network has one output for each square on the board,
        /// so it has 9 outputs total.
        /// </summary>
        public int OutputCount
        {
            get { return 3; }
        }

        /// <summary>
        /// Defines whether all networks should be evaluated every
        /// generation, or only new (child) networks. For Tic-Tac-Toe,
        /// we're evolving against a random player, so it's a good
        /// idea to evaluate individuals again every generation,
        /// to make sure it wasn't just luck.
        /// </summary>
        public bool EvaluateParents
        {
            get { return false; }
        }



        #region INeatExperiment Members
        public string Description
        {
            get { return _description; }
        }

        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the default population size to use for the experiment.
        /// </summary>
        public int DefaultPopulationSize
        {
            get { return _populationSize; }
        }

        /// <summary>
        /// Gets the NeatEvolutionAlgorithmParameters to be used for the experiment. Parameters on this object can be 
        /// modified. Calls to CreateEvolutionAlgorithm() make a copy of and use this object in whatever state it is in 
        /// at the time of the call.
        /// </summary>
        public NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters
        {
            get { return _eaParams; }
        }

        /// <summary>
        /// Gets the NeatGenomeParameters to be used for the experiment. Parameters on this object can be modified. Calls
        /// to CreateEvolutionAlgorithm() make a copy of and use this object in whatever state it is in at the time of the call.
        /// </summary>
        public NeatGenomeParameters NeatGenomeParameters
        {
            get { return _neatGenomeParams; }
        }

        /// <summary>
        /// Initialize the experiment with some optional XML configutation data.
        /// </summary>
        public void Initialize(string name, XmlElement xmlConfig)
        {
            _name = name;
            _populationSize = XmlUtils.GetValueAsInt(xmlConfig, "PopulationSize");
            _specieCount = XmlUtils.GetValueAsInt(xmlConfig, "SpecieCount");
            //_activationScheme = ExperimentUtils.CreateActivationScheme(xmlConfig, "Activation"); //Don't work.
            _activationScheme = NetworkActivationScheme.CreateAcyclicScheme();
            _complexityRegulationStr = XmlUtils.TryGetValueAsString(xmlConfig, "ComplexityRegulationStrategy");
            _complexityThreshold = XmlUtils.TryGetValueAsInt(xmlConfig, "ComplexityThreshold");
            _description = XmlUtils.TryGetValueAsString(xmlConfig, "Description");
            _parallelOptions = ExperimentUtils.ReadParallelOptions(xmlConfig);

            _eaParams = new NeatEvolutionAlgorithmParameters();
            _eaParams.SpecieCount = _specieCount;
            _neatGenomeParams = new NeatGenomeParameters();
        }

        /// <summary>
        /// Load a population of genomes from an XmlReader and returns the genomes in a new list.
        /// The genome2 factory for the genomes can be obtained from any one of the genomes.
        /// </summary>
        public List<NeatGenome> LoadPopulation(XmlReader xr)
        {
            return NeatGenomeUtils.LoadPopulation(xr, false, this.InputCount, this.OutputCount);
        }

        /// <summary>
        /// Save a population of genomes to an XmlWriter.
        /// </summary>
        public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
        {
            // Writing node IDs is not necessary for NEAT.
            NeatGenomeXmlIO.WriteComplete(xw, genomeList, false);
        }

        /// <summary>
        /// Create a genome2 factory for the experiment.
        /// Create a genome2 factory with our neat genome2 parameters object and the appropriate number of input and output neuron genes.
        /// </summary>
        public IGenomeFactory<NeatGenome> CreateGenomeFactory()
        {
            return new NeatGenomeFactory(InputCount, OutputCount, _neatGenomeParams);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload requires no parameters and uses the default population size.
        /// </summary>
        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
        {
            return CreateEvolutionAlgorithm(DefaultPopulationSize);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload accepts a population size parameter that specifies how many genomes to create in an initial randomly
        /// generated population.
        /// </summary>
        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
        {
            // Create a genome2 factory with our neat genome2 parameters object and the appropriate number of input and output neuron genes.
            IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

            // Create an initial population of randomly generated genomes.
            List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

            // Create evolution algorithm.
            return CreateEvolutionAlgorithm(genomeFactory, genomeList);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload accepts a pre-built genome2 population and their associated/parent genome2 factory.
        /// </summary>
        public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
        {
            // Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weigth difference.
            IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            ISpeciationStrategy<NeatGenome> speciationStrategy = new ParallelKMeansClusteringStrategy<NeatGenome>(distanceMetric, _parallelOptions);

            // Create complexity regulation strategy.
            IComplexityRegulationStrategy complexityRegulationStrategy = ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

            // Create the evolution algorithm.
            NeatEvolutionAlgorithm<NeatGenome> ea = new NeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

            // Create genome2 decoder.
            IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = new NeatGenomeDecoder(_activationScheme);

            // Create a genome2 list evaluator. This packages up the genome2 decoder with the genome2 evaluator.
            IGenomeListEvaluator<NeatGenome> genomeListEvaluator = new ParallelGenomeListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, PhenomeEvaluator, _parallelOptions);

            // Wrap the list evaluator in a 'selective' evaulator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
            // that were in the population in previous generations (elite genomes). This is determiend by examining each genome2's evaluation info object.
            if (!EvaluateParents)
                genomeListEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(genomeListEvaluator,
                                         SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());

            // Initialize the evolution algorithm.
            ea.Initialize(genomeListEvaluator, genomeFactory, genomeList);

            // Finished. Return the evolution algorithm
            return ea;
        }

        public GeometryFriendsEvolutionaryAlgorithm<NeatGenome> CreateGFEvolutionAlgorithm()
        {
            return CreateGFEvolutionAlgorithm(DefaultPopulationSize);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload accepts a population size parameter that specifies how many genomes to create in an initial randomly
        /// generated population.
        /// </summary>
        public GeometryFriendsEvolutionaryAlgorithm<NeatGenome> CreateGFEvolutionAlgorithm(int populationSize)
        {
            // Create a genome2 factory with our neat genome2 parameters object and the appropriate number of input and output neuron genes.
            IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

            // Create an initial population of randomly generated genomes.
            List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

            // Create evolution algorithm.
            return CreateGFEvolutionAlgorithm(genomeFactory, genomeList);
        }

        /// <summary>
        /// Create and return a NeatEvolutionAlgorithm object ready for running the NEAT algorithm/search. Various sub-parts
        /// of the algorithm are also constructed and connected up.
        /// This overload accepts a pre-built genome2 population and their associated/parent genome2 factory.
        /// </summary>
        public GeometryFriendsEvolutionaryAlgorithm<NeatGenome> CreateGFEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
        {
            // Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weigth difference.
            IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
            ISpeciationStrategy<NeatGenome> speciationStrategy = new ParallelKMeansClusteringStrategy<NeatGenome>(distanceMetric, _parallelOptions);

            // Create complexity regulation strategy.
            IComplexityRegulationStrategy complexityRegulationStrategy = ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

            // Create the evolution algorithm.
            GeometryFriendsEvolutionaryAlgorithm<NeatGenome> ea = new GeometryFriendsEvolutionaryAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

            // Create genome2 decoder.
            IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = new NeatGenomeDecoder(_activationScheme);

            // Create a genome2 list evaluator. This packages up the genome2 decoder with the genome2 evaluator.
            //IGenomeListEvaluator<NeatGenome> genomeListEvaluator = new SerialGenomeListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, PhenomeEvaluator);
            IGenomeListEvaluator<NeatGenome> genomeListEvaluator = new GeometryFriendsGenomeListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, PhenomeEvaluator);

            // Wrap the list evaluator in a 'selective' evaulator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
            // that were in the population in previous generations (elite genomes). This is determiend by examining each genome2's evaluation info object.
            if (!EvaluateParents)
                genomeListEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(genomeListEvaluator,
                                         SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());

            // Initialize the evolution algorithm.
            ea.Initialize(genomeListEvaluator, genomeFactory, genomeList);

            // Finished. Return the evolution algorithm
            return ea;
        }

        /// <summary>
        /// Creates a new genome decoder that can be used to convert a genome into a phenome.
        /// </summary>
        public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder()
        {
            return new NeatGenomeDecoder(_activationScheme);
        }

        #endregion
    }
}
