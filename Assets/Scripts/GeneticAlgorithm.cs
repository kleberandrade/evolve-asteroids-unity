using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm
{
    public int PopulationSize { get; set; }

    public int ChromosomeLength { get; set; }

    public List<Chromosome> PopulationRandomInitialization()
    {
        List<Chromosome> population = new List<Chromosome>();
        while (population.Count < PopulationSize)
        { 
            population.Add(new Chromosome(ChromosomeLength));
        }

        return population;
    }

    public void NextGeneration()
    {

    }
}
