using System.Collections.Generic;

public class GeneticAlgorithm
{
    public int PopulationSize { get; set; }

    public int ChromosomeLength { get; set; }

    public List<Chromosome> PopulationRandomInitialization()
    {
        List<Chromosome> population = new List<Chromosome>();
        for (int i = 0; i < PopulationSize; i++)
        {
            population.Add(new Chromosome(ChromosomeLength));
        }

        return population;
    }
}
