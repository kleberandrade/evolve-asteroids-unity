using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField]
    private Text m_ScoreText;
    [SerializeField]
    private Text m_HighscoreText;
    [SerializeField]
    private Text m_WaveText;
    [SerializeField]
    private Text m_TimeText;
    [SerializeField]
    private Text m_GenerationText;
    [SerializeField]
    private Text m_ChromosomeText;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject m_Asteroid;
    [SerializeField]
    private GameObject m_Ship;

    [Header("Gameplay")]
    [SerializeField]
    private float m_MaxTime = 15.0f;
    [SerializeField]
    private int m_IncreaseEachWave = 8;
    [SerializeField]
    private float m_RespawnDistance = 2.5f;
    [SerializeField]
    private float m_RespawnTime = 2.0f;

    [Header("Genetic Properties")]
    [SerializeField]
    private int m_PopulationSize = 100;
    [Range(0, 10)]
    [SerializeField]
    private int m_TournamentSelectionSize = 3;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float m_CrossoverRate = 0.5f;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float m_MutationRate = 0.02f;
    [SerializeField]
    private int m_MaxGeneration = 10;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float m_ElitismRate = 0.05f;
    [SerializeField]
    private string m_FileName = "teste";

    private List<Chromosome> m_Population = new List<Chromosome>();
    private int m_CurrentChromosome;
    private int m_CurrentGeneration;
    private float m_Time;
    private int m_Score;
    private int m_Highscore;
    private int m_Wave;
    private int m_AsteroidsRemaining;
    private bool m_Run;

    private readonly int k_ChromosomeLength = 256;
    private readonly string k_HighscoreKey = "highscore";

    private IEnumerator m_ResetCoroutine;

    public void Start()
    {
        m_Population = PopulationRandomInitialize();

        m_ResetCoroutine = ResetGame(false);
        StartCoroutine(m_ResetCoroutine);
    }

    public List<Chromosome> PopulationRandomInitialize()
    {
        List<Chromosome> population = new List<Chromosome>();
        while (population.Count < m_PopulationSize)
        {
            Chromosome chromosome = new Chromosome(k_ChromosomeLength);
            population.Add(chromosome);
        }
        return population;
    }

    public Chromosome TournamentSelection()
    {
        List<Chromosome> chromosomes = new List<Chromosome>();
        for (int i = 0; i < m_TournamentSelectionSize; i++)
        {
            int index = Helper.NextInt(m_PopulationSize);
            Chromosome chromosome = m_Population[index].Clone() as Chromosome;
            chromosomes.Add(chromosome);
        }

        chromosomes.Sort();
        return chromosomes[0];
    }

    public IEnumerator ResetGame(bool useRespawnTime)
    {
        if (useRespawnTime)
        {
            yield return new WaitForSeconds(m_RespawnTime);
        }

        m_Highscore = PlayerPrefs.GetInt(k_HighscoreKey, 0);
        m_Score = 0;
        m_Wave = 1;
        m_Time = 0;

        UpdateHud();

        SpawnShip();

        SpawnAsteroids();

        m_Run = true;

        yield return null;
    }

    public void UpdateHud()
    {
        m_ScoreText.text = $"SCORE: {m_Score}";
        m_HighscoreText.text = "HIGHSCORE: " + m_Highscore;
        m_WaveText.text = string.Format("WAVE: {0}", m_Wave);
        m_TimeText.text = $"TIME {(m_MaxTime - m_Time).ToString("0")}";
        m_GenerationText.text = $"GENERATION {m_CurrentGeneration + 1} / {m_MaxGeneration}";
        m_ChromosomeText.text = $"CHROMOSOME {m_CurrentChromosome + 1} / {m_PopulationSize}";
    }

    public List<Chromosome> Elitism()
    {
        m_Population.Sort();


        for (int i = 0; i < m_PopulationSize; i++)
        {
            Debug.Log(m_Population[i].Fitness);
        }

        int length = (int)(m_PopulationSize * m_ElitismRate);

        List<Chromosome> chromosomes = new List<Chromosome>();
        for (int i = 0; i < length; i++)
        {
            chromosomes.Add(m_Population[i].Clone() as Chromosome);
        }

        return chromosomes;
    }

    public void Save(bool append)
    {
        using (StreamWriter file = new StreamWriter(m_FileName + ".xls", append))
        {
            double bestFitness = 0.0;
            double averageFitness = 0.0;
            for (int i = 0; i < m_PopulationSize; i++)
            {
                averageFitness += m_Population[i].Fitness;
                if (bestFitness < m_Population[i].Fitness)
                {
                    bestFitness = m_Population[i].Fitness;
                }
            }

            averageFitness /= (double)m_PopulationSize;
            file.WriteLine("{0}\t{1}", averageFitness, bestFitness);
        }
    }

    public IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(m_RespawnTime);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public bool IsContinue => m_CurrentGeneration < m_MaxGeneration;

    public void NextGeneration()
    {
        Save(m_CurrentGeneration > 0);
        m_CurrentGeneration++;
        if (m_CurrentGeneration == m_MaxGeneration)
        {
            StartCoroutine(QuitGame());
        }
        else
        {
            List<Chromosome> newPopulation = Elitism();
            while (newPopulation.Count < m_PopulationSize)
            {
                Chromosome parent1 = TournamentSelection();
                Chromosome parent2 = TournamentSelection();

                Chromosome offspring = parent1.Crossover(parent2, m_CrossoverRate);
                offspring.Mutate(m_MutationRate);

                newPopulation.Add(offspring);
            }
            m_Population = new List<Chromosome>(newPopulation);
            m_CurrentChromosome = 0;
        }
    }

    public void Update()
    {
        if (m_Run)
        {
            m_Time = Mathf.Clamp(m_Time + Time.deltaTime, 0, m_MaxTime);
            UpdateHud();

            if (m_Time >= m_MaxTime)
            {
                m_Run = false;
                GameObject ship = GameObject.FindGameObjectWithTag("Ship");
                ShipController shipController = ship.GetComponent<ShipController>();
                shipController.Kill();
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

        int count = 0;
        while (count < m_AsteroidsRemaining)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            if (Vector3.Distance(new Vector3(x, y, 0), Vector3.zero) > m_RespawnDistance)
            {
                count++;
                float angle = Random.Range(0.0f, 360.0f);
                Instantiate(m_Asteroid, new Vector3(x, y, 0), Quaternion.Euler(0, 0, angle));
            }
        }

        UpdateHud();
    }

    private void DestroyExistingObjects()
    {
        DestroyExistingObjects("Big Asteroid");
        DestroyExistingObjects("Small Asteroid");
        DestroyExistingObjects("Bullet");
    }

    private void DestroyExistingObjects(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects)
        {
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

    public double EvaluationFitness()
    {
        return m_Score;
    }

    public void DecrementLives()
    {
        m_Run = false;
        m_Population[m_CurrentChromosome].Fitness = EvaluationFitness();

        m_CurrentChromosome++;
        if (m_CurrentChromosome == m_PopulationSize)
        {
            NextGeneration();
            if (m_CurrentGeneration < m_MaxGeneration)
            {
                StopCoroutine(m_ResetCoroutine);
                m_ResetCoroutine = ResetGame(true);
                StartCoroutine(m_ResetCoroutine);
            }
        }
        else
        {
            StopCoroutine(m_ResetCoroutine);
            m_ResetCoroutine = ResetGame(true);
            StartCoroutine(m_ResetCoroutine);
        }
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
