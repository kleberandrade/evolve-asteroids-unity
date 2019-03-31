using UnityEngine;

public class Ship : MonoBehaviour
{
    public float m_RotationSpeed = 100.0f;
    public float m_ThrustForce = 3f;

    public AudioClip m_CrashAudioClip;
    public AudioClip m_ShootAudioClip;

    public GameObject m_Bullet;

    private Rigidbody2D m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * m_RotationSpeed * Time.deltaTime);

        m_Rigidbody.AddForce(transform.up * m_ThrustForce * Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            return;
        }

        transform.position = new Vector3(0, 0, 0);
        m_Rigidbody.velocity = new Vector3(0, 0, 0);
        AudioSource.PlayClipAtPoint(m_CrashAudioClip, Camera.main.transform.position);

        GameManager.Instance.DecrementLives();
        Destroy(gameObject);
    }

    private void ShootBullet()
    {
        Vector3 position = transform.position + transform.TransformDirection(0, 0.5f, 0);
        Instantiate(m_Bullet, position, transform.rotation);
        AudioSource.PlayClipAtPoint(m_ShootAudioClip, Camera.main.transform.position);
    }
}
