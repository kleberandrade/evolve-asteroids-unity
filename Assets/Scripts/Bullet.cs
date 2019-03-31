using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_TimeToDestroy = 1.0f;

    private void Start()
    {
        Destroy(gameObject, 1.0f);
        GetComponent<Rigidbody2D>().AddForce(transform.up * 400);
    }
}
