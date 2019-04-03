using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Helper
{
    private static Random m_Random;

    private Helper() { }

    private static void InitializeRandomNumber()
    {
        if (m_Random == null)
        {
            m_Random = new Random();
        }
    }

    public static int NextInt(int min, int max)
    {
        InitializeRandomNumber();
        return m_Random.Next(min, max);
    }

    public static int NextInt(int max)
    {
        InitializeRandomNumber();
        return NextInt(0, max);
    }

    public static double NextDouble()
    {
        InitializeRandomNumber();
        return m_Random.NextDouble();
    }

    public static int[] DecimalToBinary(int number, int size)
    {
        List<int> binary = new List<int>();

        while (number > 0)
        {
            binary.Add(number % 2);
            number = number / 2;
        }

        while (binary.Count < size)
        {
            binary.Add(0);
        }

        binary.Reverse();
        return binary.ToArray();
    }
}