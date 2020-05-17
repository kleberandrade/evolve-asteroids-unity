using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("UI (User Interface)")]
    public Text m_ScoreText;
    public Text m_HighscoreText;
    public Text m_WaveText;
    public Text m_TimeText;
    public Text m_GenerationText;
    public Text m_ChromosomeText;

    [Header("Prefabs")]
    public GameObject m_Asteroid;
    public GameObject m_Ship;

    [Header("Gameplay")]
    public int m_IncreaseEachWave = 8;
    public float m_RespawnDistance = 2.5f;
    public float m_MaxTime = 15.0f;
    public int m_RandomSeed = 13;

    [Header("Genetic Properties")]
    public string m_FileName;
    public int m_PopulationSize = 100;
    public int m_ChromosomeLength = 24;
    public int m_MaxGeneration = 10;
    [Range(0, 10)]
    public int m_TournamentSize = 3;
    [Range(0.0f, 1.0f)]
    public float m_Alpha = 0.5f;
    [Range(0.0f, 1.0f)]
    public float m_MutationRate = 0.02f;
    [Range(0.0f, 1.0f)]
    public float m_ElitismRate = 0.05f;

    private List<Chromosome> m_Population = new List<Chromosome>();
    private int m_CurrentChromosome;
    private int m_CurrentGeneration;
    private float m_Time;
    private int m_Score;
    private int m_Highscore;
    private int m_Wave;
    private int m_AsteroidsRemaining;
    private readonly string k_HighscoreKey = "highscore";
    private bool m_DestroyedShip;

    public void Start()
    {
        PlayerPrefs.DeleteAll();
        Helper.ResetRandomNumber(m_RandomSeed);
        m_Population = PopulationRandomInitialize();
        ResetGame();
    }

    public void ResetGame()
    {
        m_Highscore = PlayerPrefs.GetInt(k_HighscoreKey, 0);
        m_Score = 0;
        m_Wave = 1;
        m_Time = 0;
        m_DestroyedShip = false;

        UpdateHud();
        SpawnShip();
        SpawnAsteroids();
    }

    public List<Chromosome> PopulationRandomInitialize()
    {
        List<Chromosome> population = new List<Chromosome>();
        while (population.Count < m_PopulationSize)
        {
            Chromosome chromosome = new Chromosome(m_ChromosomeLength);
            if (!population.Contains(chromosome))
            {
                population.Add(chromosome);
            }
        }
        return new List<Chromosome>(population);
    }

    public Chromosome TournamentSelection()
    {
        List<Chromosome> chromosomes = new List<Chromosome>();
        for (int i = 0; i < m_TournamentSize; i++)
        {
            int index = Helper.Random(m_PopulationSize);
            Chromosome chromosome = m_Population[index].Clone() as Chromosome;
            chromosomes.Add(chromosome);
        }

        chromosomes.Sort();
        return chromosomes[0];
    }

    public void UpdateHud()
    {
        m_ScoreText.text = $"SCORE: {m_Score}";
        m_HighscoreText.text = "HIGHSCORE: " + m_Highscore;
        m_WaveText.text = string.Format("WAVE: {0}", m_Wave);
        m_TimeText.text = $"TIME {m_Time:0} / {m_MaxTime}";
        m_GenerationText.text = $"GENERATION {m_CurrentGeneration + 1} / {m_MaxGeneration}";
        m_ChromosomeText.text = $"CHROMOSOME {m_CurrentChromosome + 1} / {m_PopulationSize}";
    }

    public List<Chromosome> Elitism()
    {
        m_Population.Sort();
        int length = (int)(m_PopulationSize * m_ElitismRate);

        List<Chromosome> chromosomes = new List<Chromosome>();
        for (int i = 0; i < length; i++)
        {
            Chromosome chromosome = m_Population[i].Clone() as Chromosome;
            chromosome.Survived++;
            chromosomes.Add(chromosome);
        }

        return chromosomes;
    }

    public void Save(bool append)
    {
        if (string.IsNullOrEmpty(m_FileName))
        {
            return;
        }

        using (StreamWriter file = new StreamWriter(m_FileName + ".xls", append))
        {
            float bestFitness = m_Population.Max(x => x.Fitness);
            float averageFitness = m_Population.Average(x => x.Fitness);
            file.WriteLine("{0}\t{1}", averageFitness, bestFitness);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public bool IsContinue => m_CurrentGeneration < m_MaxGeneration;

    public void NextGeneration()
    {
        m_Population.Sort();

        Save(m_CurrentGeneration > 0);
        
        m_CurrentGeneration++;
        if (m_CurrentGeneration >= m_MaxGeneration)
        {
            QuitGame();
        }
        else
        {
            List<Chromosome> newPopulation = new List<Chromosome>(Elitism());
            while (newPopulation.Count < m_PopulationSize)
            {
                Chromosome parent1 = TournamentSelection();
                Chromosome parent2 = TournamentSelection();

                Chromosome offspring = parent1.Crossover(parent2, m_Alpha);
                offspring.Mutate(m_MutationRate);

                if (!newPopulation.Contains(offspring))
                    newPopulation.Add(offspring);
            }

            m_Population = new List<Chromosome>(newPopulation);
            m_CurrentChromosome = 0;
        }
    }

    public void Update()
    {
        if (m_DestroyedShip)
        {
            m_CurrentChromosome++;
            if (m_CurrentChromosome == m_PopulationSize)
                NextGeneration();

            ResetGame();
        } 
        else 
        {
            m_Time += Time.deltaTime;
            UpdateHud();

            if (m_Time >= m_MaxTime)
            {
                GameObject ship = GameObject.FindGameObjectWithTag("Ship");
                if (ship){
                    ShipController shipController = ship.GetComponent<ShipController>();
                    shipController.Kill();
                }
            }
        }
    }

    private void SpawnShip()
    {
        GameObject ship = Instantiate(m_Ship, Vector3.zero, Quaternion.identity);
        Brain brain = ship.GetComponent<Brain>();
        brain.Chromosome = m_Population[m_CurrentChromosome];
    }

    private void SpawnAsteroids()
    {
        DestroyExistingObjects();

        m_AsteroidsRemaining = m_Wave * m_IncreaseEachWave;
        Bounds bounds = Camera.main.OrthographicBounds();

        var ship = GameObject.FindGameObjectWithTag("Ship");

        int count = 0;
        while (count < m_AsteroidsRemaining)
        {
            float x = Helper.Random(bounds.min.x, bounds.max.x);
            float y = Helper.Random(bounds.min.y, bounds.max.y);

            if (Vector3.Distance(new Vector3(x, y, 0), ship.transform.position) > m_RespawnDistance)
            {
                count++;
                float angle = Helper.Random(0.0f, 360.0f);
                Instantiate(m_Asteroid, new Vector3(x, y, 0), Quaternion.Euler(0, 0, angle));
            }
        }

        UpdateHud();
    }

    private void DestroyExistingObjects()
    {
        DestroyExistingObjects("Bullet");
        DestroyExistingObjects("Big Asteroid");
        DestroyExistingObjects("Small Asteroid");
    }

    private void DestroyExistingObjects(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        if (objects == null)
            return;

        if (objects.Length == 0)
            return;

        foreach (var obj in objects)
        {
            if (obj)
                Destroy(obj);
        }
    }

    public void SplitAsteroid(int size)
    {
        m_AsteroidsRemaining += size;
    }

    public void DecrementAsteroids()
    {
        m_AsteroidsRemaining--;
    }

    public float EvaluationFitness()
    {
        return m_Score;
    }

    public void DecrementLives()
    {
        m_Population[m_CurrentChromosome].Fitness = EvaluationFitness();
        m_DestroyedShip = true;
    }

    public void IncrementScore(int score)
    {
        m_Score += score;
        if (m_Score > m_Highscore)
        {
            PlayerPrefs.SetInt(k_HighscoreKey, m_Score);
            PlayerPrefs.Save();
        }

        UpdateHud();

        if (m_AsteroidsRemaining == 0)
        {
            m_Wave++;
            SpawnAsteroids();
        }
    }
}
