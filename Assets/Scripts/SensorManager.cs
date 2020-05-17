using UnityEngine;

public class SensorManager : MonoBehaviour
{
    public DistanceSensor m_DistanceSensorPrefab;

    public int m_Number = 8;

    private DistanceSensor[] m_Sensors;

    public void Start()
    {
        float angle = 360 / (float)m_Number;
        for (int i = 0; i < m_Number; i++)
        {
            DistanceSensor sensor = Instantiate(m_DistanceSensorPrefab);
            sensor.transform.position = transform.position;
            sensor.transform.rotation = Quaternion.Euler(0, 0, angle * i);
            sensor.transform.parent = transform;
        }

        m_Sensors = GetComponentsInChildren<DistanceSensor>();
    }

    public float[] ToArray()
    {
        float[] array = new float[m_Sensors.Length];
        for (int i = 0; i < m_Sensors.Length; i++)
            array[i] = m_Sensors[i].Distance;

        return array;
    }
}
