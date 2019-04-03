using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour
{
    [SerializeField]
    private float m_RotationSpeed = 100.0f;

    [SerializeField]
    private float m_ThrustForce = 3.0f;

    [SerializeField]
    private AudioClip m_CrashAudioClip;

    [SerializeField]
    private AudioClip m_ShootAudioClip;

    [SerializeField]
    private GameObject m_BulletPrefab;

    [SerializeField]
    private GameObject m_ExplosionParticleSystem;

    [SerializeField]
    private float m_FireRate = 0.1f;

    private float m_NextFire;

    private Rigidbody2D m_Rigidbody;

    public bool UseSmartControl { get; set; }

    public float Horizontal { get; set; }

    public float Vertical { get; set; }

    public bool Fire { get; set; }

    public void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (!UseSmartControl)
        {
            Horizontal = -Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            Fire = Input.GetButtonDown("Fire1");
        }

        transform.Rotate(0, 0, Horizontal * m_RotationSpeed * Time.deltaTime);
        m_Rigidbody.AddForce(transform.up * m_ThrustForce * Vertical);

        if (Fire && Time.time > m_NextFire)
        {
            m_NextFire = Time.time + m_FireRate;
            ShootBullet();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            return;
        }

        AudioSource.PlayClipAtPoint(m_CrashAudioClip, Camera.main.transform.position);
        Instantiate(m_ExplosionParticleSystem, transform.position, Quaternion.identity);

        transform.position = Vector3.zero;
        m_Rigidbody.velocity = Vector3.zero;

        GameManager.Instance.DecrementLives();
        Destroy(gameObject);
    }

    private void ShootBullet()
    {
        Vector3 position = transform.position + transform.TransformDirection(0, 0.5f, 0);
        Instantiate(m_BulletPrefab, position, transform.rotation);
        AudioSource.PlayClipAtPoint(m_ShootAudioClip, Camera.main.transform.position);
    }
}
