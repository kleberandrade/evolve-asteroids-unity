using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_DestroyAudioClip = null;

    [SerializeField]
    private int m_Score = 20;

    [SerializeField]
    private GameObject m_SmallAsteroid = null;

    [SerializeField]
    private int m_Count = 3;

    private Rigidbody2D m_Rigidbody = null;

    public void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.AddForce(transform.up * Random.Range(-360.0f, 360.0f));
        m_Rigidbody.angularVelocity = Random.Range(0.0f, 180.0f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            return;
        }
        
        Destroy(collision.gameObject);
        GameManager.Instance.DecrementAsteroids();

        if (m_SmallAsteroid)
        {
            for (int i = 0; i < m_Count; i++)
            {
                Instantiate(m_SmallAsteroid, transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
            }

            GameManager.Instance.SplitAsteroid(m_Count);
        }

        AudioSource.PlayClipAtPoint(m_DestroyAudioClip, Camera.main.transform.position);
        Destroy(gameObject);

        GameManager.Instance.IncrementScore(m_Score);
    }
}
