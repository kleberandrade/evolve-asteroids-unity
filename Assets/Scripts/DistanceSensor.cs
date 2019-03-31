using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    [SerializeField]
    private float m_MaxDistance = 4.0f;

    [SerializeField]
    private LayerMask m_LayerMask;

    [SerializeField]
    private Color m_RayColorWithCollision = Color.red;

    [SerializeField]
    private Color m_RayColorWithoutCollision = Color.green;

    private float m_Distance;

    public bool Detected => m_Distance < m_MaxDistance;

    public void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), m_MaxDistance, m_LayerMask);
        m_Distance = hit.collider ? hit.distance : m_MaxDistance;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * m_Distance, Detected ? m_RayColorWithCollision : m_RayColorWithoutCollision);
    }
}