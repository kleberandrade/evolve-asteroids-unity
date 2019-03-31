using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    public float m_MaxDistance;
    public LayerMask m_LayerMask;
    public Color m_RayColorWithCollision = Color.red;
    public Color m_RayColorWithoutCollision = Color.green;
    public float m_Distance;

    public bool Detected => m_Distance < m_MaxDistance;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), m_MaxDistance, m_LayerMask);
        m_Distance = hit.collider ? hit.distance : m_MaxDistance;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * m_Distance, Detected ? m_RayColorWithCollision : m_RayColorWithoutCollision);
    }
}