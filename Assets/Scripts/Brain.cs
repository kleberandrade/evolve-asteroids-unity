using UnityEngine;

public class Brain : MonoBehaviour
{
    public Chromosome Chromosome { get; set; }

    private SensorManager m_Input;
    private ShipController m_Output;

    private float m_Horizontal;
    private float m_Vertical;
    private float m_Fire;

    public void Awake()
    {
        m_Input = GetComponent<SensorManager>();
        m_Output = GetComponent<ShipController>();
    }

    public void Update()
    {
        if (Chromosome != null)
        {
            m_Vertical = m_Horizontal = m_Fire = 0.0f;
            float[] sensors = m_Input.ToArray();
            for (int i = 0; i < sensors.Length; i++)
            {
                m_Vertical += sensors[i] * Chromosome[i];
                m_Horizontal += sensors[i] * Chromosome[i + sensors.Length];
                m_Fire += sensors[i] * Chromosome[i + sensors.Length * 2];
            }

            m_Output.Vertical = m_Vertical;
            m_Output.Horizontal = m_Horizontal * 2.0f - 1.0f;
            m_Output.Fire = m_Fire > 0.5f;
        }
    }
}
