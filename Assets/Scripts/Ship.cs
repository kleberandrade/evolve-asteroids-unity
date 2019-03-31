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
    private GameObject m_Bullet;

    private Rigidbody2D m_Rigidbody;

    public void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * m_RotationSpeed * Time.deltaTime);

        m_Rigidbody.AddForce(transform.up * m_ThrustForce * Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
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
