using System;

public class Helper
{
    private static Random m_Random = null;

    private Helper() { }

    private static void InitializeRandomNumber()
    {
        if (m_Random != null)
            return;

        m_Random = new Random();
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
}