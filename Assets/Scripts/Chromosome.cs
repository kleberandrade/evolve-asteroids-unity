using System;
using System.Text;

public class Chromosome : IComparable<Chromosome>, IEquatable<Chromosome>, ICloneable
{
    private static readonly int k_MaxAllele = 12;

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

    public float Fitness { get; set; }

    public int Survived { get; set; }

    public Chromosome(int length)
    {
        Genes = new int[length];
        for (int i = 0; i < Length; i++)
        {
            Genes[i] = Helper.Random(k_MaxAllele);
        }

        Fitness = 0.0f;
        Survived = 0;
    }

    public Chromosome(Chromosome chromosome)
    {
        Genes = new int[chromosome.Length];
        Array.Copy(chromosome.Genes, Genes, Length);
        Fitness = chromosome.Fitness;
        Survived = chromosome.Survived;
    }

    public Chromosome Crossover(Chromosome otherParent, float crossoverRate)
    {
        Chromosome child = new Chromosome(Length);
        for (int i = 0; i < child.Length; i++)
        {
            child[i] = Helper.Random() < crossoverRate ? Genes[i] : otherParent[i];
        }

        return child;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < Length; i++)
        {
            Genes[i] = Helper.Random() < mutationRate ? Helper.Random(k_MaxAllele) : Genes[i];
        }
    }

    public object Clone()
    {
        return new Chromosome(this);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (int alelo in Genes)
            {
                hash = hash * alelo.GetHashCode();
            }

            return hash;
        }
    }

    public int CompareTo(Chromosome other)
    {
        if (Fitness > other.Fitness)
        {
            return -1;
        }

        if (Fitness < other.Fitness)
        {
            return 1;
        }

        return 0;
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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Chromosome chromosome = (Chromosome)obj;
        for (int i = 0; i < Length; i++)
        {
            if (!Genes[i].Equals(chromosome[i]))
            {
                return false;
            }
        }

        return true;
    }

    public bool Equals(Chromosome other)
    {
        for (int i = 0; i < Length; i++)
        {
            if (!Genes[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }
}