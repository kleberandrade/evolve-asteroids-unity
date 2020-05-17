using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    public float m_RotationSpeed = 100.0f;
    public float m_ThrustForce = 3.0f;
    public AudioClip m_CrashAudioClip;
    public AudioClip m_ShootAudioClip;
    public GameObject m_BulletPrefab;
    public float m_FireRate = 0.1f;
    private float m_NextFire;

    private Rigidbody2D m_Rigidbody;

    public float Horizontal { get; set; }

    public float Vertical { get; set; }

    public bool Fire { get; set; }

    public void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        transform.Rotate(0, 0, Horizontal * m_RotationSpeed * Time.deltaTime);
        m_Rigidbody.AddForce(transform.up * m_ThrustForce * Vertical);

        if (Fire && Time.time > m_NextFire)
        {
            m_NextFire = Time.time + m_FireRate;
            ShootBullet();
        }
    }

    public void Kill()
    {
        AudioSource.PlayClipAtPoint(m_CrashAudioClip, Camera.main.transform.position);
        GameManager.Instance.DecrementLives();
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Kill();   
    }

    private void ShootBullet()
    {
        var position = transform.position + transform.TransformDirection(0, 0.5f, 0);
        Instantiate(m_BulletPrefab, position, transform.rotation);
        AudioSource.PlayClipAtPoint(m_ShootAudioClip, Camera.main.transform.position);
    }
}
