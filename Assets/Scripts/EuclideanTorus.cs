using UnityEngine;

public class EuclideanTorus : MonoBehaviour
{
    private Bounds m_Bounds;

    private void Start()
    {
        m_Bounds = Camera.main.OrthographicBounds();
    }

    private void Update()
    {
        if (transform.position.x > m_Bounds.max.x)
        {
            transform.position = new Vector3(m_Bounds.min.x, transform.position.y, 0);
        }
        else if (transform.position.x < m_Bounds.min.x)
        {
            transform.position = new Vector3(m_Bounds.max.x, transform.position.y, 0);
        }
        else if (transform.position.y > m_Bounds.max.y)
        {
            transform.position = new Vector3(transform.position.x, m_Bounds.min.y, 0);
        }
        else if (transform.position.y < m_Bounds.min.y)
        {
            transform.position = new Vector3(transform.position.x, m_Bounds.max.y, 0);
        }
    }
}