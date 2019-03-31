using System;
using System.Text;

public class Chromosome
{
    private static readonly int MAX_ALLELE = 16;

    public int[] Genes { get; private set; }

    public int this[int index]
    {
        get { return Genes[index]; }
        set { Genes[index] = value; }
    }

    public int Length
    {
        get { return Genes.Length; }
    }

    public double Fitness { get; set; }

    public Chromosome(int length, bool shouldInitGenes = true)
    {
        Genes = new int[length];
        for (int i = 0; i < Length; i++)
        {
            Genes[i] = Helper.NextInt(MAX_ALLELE);
        }
    }

    public Chromosome Crossover(Chromosome otherParent, double crossoverRate = 0.5)
    {
        Chromosome child = new Chromosome(Length, false);
        for (int i = 0; i < child.Length; i++)
        {
            child[i] = Helper.NextDouble() < crossoverRate ? Genes[i] : otherParent[i];
        }

        return child;
    }

    public void Mutate(double mutationRate)
    {
        for (int i = 0; i < Length; i++)
        {
            Genes[i] = Helper.NextDouble() < mutationRate ? Helper.NextInt(MAX_ALLELE) : Genes[i];
        }
    }

    public Chromosome Clone(Chromosome chromosome)
    {
        Chromosome clone = new Chromosome(chromosome.Length);
        Array.Copy(chromosome.Genes, clone.Genes, chromosome.Length);
        return clone;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("[");
        Array.ForEach(Genes, x => builder.Append(x.ToString("00")).Append("|"));
        builder.Remove(builder.Length - 1, 1);
        builder.Append("]");
        return builder.ToString();
    }
}