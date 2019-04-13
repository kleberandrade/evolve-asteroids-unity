using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_TimeToDestroy = 1.0f;

    [SerializeField]
    private float m_Speed = 400.0f;

    public void Start()
    {
        Destroy(gameObject, m_TimeToDestroy);
        GetComponent<Rigidbody2D>().AddForce(transform.up * m_Speed);
    }
}
