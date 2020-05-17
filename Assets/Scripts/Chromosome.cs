using System;

public class Chromosome : IComparable<Chromosome>, IEquatable<Chromosome>, ICloneable
{
    public float[] Genes { get; private set; }

    public float this[int index]
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
        Genes = new float[length];
        for (int i = 0; i < Length; i++)
        {
            Genes[i] = Helper.Random(0.0f, 1.0f);
        }

        Fitness = 0.0f;
        Survived = 0;
    }

    public Chromosome(Chromosome chromosome)
    {
        Genes = new float[chromosome.Length];
        Array.Copy(chromosome.Genes, Genes, Length);
        Fitness = chromosome.Fitness;
        Survived = chromosome.Survived;
    }

    public Chromosome Crossover(Chromosome otherParent, float alpha)
    {
        Chromosome child = new Chromosome(Length);
        for (int i = 0; i < child.Length; i++)
            child[i] = Genes[i] + alpha * (otherParent[i] - Genes[i]);

        return child;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < Length; i++)
            Genes[i] = Helper.Random() < mutationRate ? Helper.Random(0.0f, 1.0f) : Genes[i];
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