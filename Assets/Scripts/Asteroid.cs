using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_DestroyAudioClip;

    [SerializeField]
    private int m_Score = 20;

    [SerializeField]
    private GameObject m_SmallAsteroid;

    [SerializeField]
    private int m_Count = 3;

    [SerializeField]
    private float m_MinLinearVelocity = 30.0f;

    [SerializeField]
    private float m_MaxLinearVelocity = 100.0f;

    [SerializeField]
    private float m_MinAngularVelocity = -180.0f;

    [SerializeField]
    private float m_MaxAngularVelocity = 180.0f;

    public void Start()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.up * Random.Range(m_MinLinearVelocity, m_MaxLinearVelocity));
        rigidbody.angularVelocity = Random.Range(m_MinAngularVelocity, m_MaxAngularVelocity);
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
                Instantiate(m_SmallAsteroid, transform.position, Quaternion.Euler(0, 0, Random.Range(m_MinAngularVelocity, m_MaxAngularVelocity)));
            }

            GameManager.Instance.SplitAsteroid(m_Count);
        }

        AudioSource.PlayClipAtPoint(m_DestroyAudioClip, Camera.main.transform.position);
        Destroy(gameObject);

        GameManager.Instance.IncrementScore(m_Score);
    }
}
