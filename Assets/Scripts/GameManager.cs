using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI (User Interface)")]
    [SerializeField]
    private Text m_ScoreText;

    [SerializeField]
    private Text m_WaveText;

    [SerializeField]
    private Text m_HiscoreText;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject m_Asteroid;

    [SerializeField]
    private GameObject m_Ship;

    private int m_Score;

    private int m_Highscore;

    private int m_AsteroidsRemaining;

    private int m_Wave;

    [SerializeField]
    private int m_IncreaseEachWave = 4;

    private const string k_HighscoreKey = "highscore";

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

    public void Start()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        m_Highscore = PlayerPrefs.GetInt(k_HighscoreKey, 0);
        m_Score = 0;
        m_Wave = 1;
        UpdateHud();
        SpawnShip();
        SpawnAsteroids();
    }

    public void SpawnShip()
    {
        Instantiate(m_Ship, Vector3.zero, Quaternion.identity);
    }

    public void UpdateHud()
    {
        m_ScoreText.text = $"SCORE: {m_Score}";
        m_HiscoreText.text = $"HIGHSCORE: {m_Highscore}";
        m_WaveText.text = $"WAVE: {m_Wave}";
    }

    public void SpawnAsteroids()
    {
        DestroyAllExistingAsteroids();

        m_AsteroidsRemaining = (m_Wave * m_IncreaseEachWave);

        Bounds bounds = Camera.main.OrthographicBounds();

        for (int i = 0; i < m_AsteroidsRemaining; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            Instantiate(m_Asteroid, new Vector3(x, y, 0.0f), Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
        }

        UpdateHud();
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

        if (m_AsteroidsRemaining < 1)
        {
            m_Wave++;
            SpawnAsteroids();
        }
    }

    public void DecrementLives()
    {
        RestartGame();
    }

    public void DecrementAsteroids()
    {
        m_AsteroidsRemaining--;
    }

    public void SplitAsteroid(int size)
    {
        m_AsteroidsRemaining += size;
    }

    public void DestroyAllExistingAsteroids()
    {
        DestroyExistingAsteroids("Big Asteroid");
        DestroyExistingAsteroids("Small Asteroid");
    }

    public void DestroyExistingAsteroids(string tag)
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject current in asteroids)
        {
            Destroy(current);
        }
    }
}