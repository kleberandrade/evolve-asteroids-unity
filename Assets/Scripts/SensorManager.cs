using System;
using System.Text;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    [SerializeField]
    private DistanceSensor m_DistanceSensorPrefab = null;

    [SerializeField]
    private int m_DistanceSensorCount = 8;

    private DistanceSensor[] m_Sensors = null;

    public void Start()
    {
        float angle = 360 / (float)m_DistanceSensorCount;
        for (int i = 0; i < m_DistanceSensorCount; i++)
        {
            DistanceSensor sensor = Instantiate(m_DistanceSensorPrefab);
            sensor.transform.position = transform.position;
            sensor.transform.rotation = Quaternion.Euler(0, 0, angle * i);
            sensor.transform.parent = transform;
        }

        m_Sensors = GetComponentsInChildren<DistanceSensor>();
    }

    public int[] ToArray()
    {
        int[] array = new int[m_Sensors.Length];
        for (int i = 0; i < m_Sensors.Length; i++)
        {
            array[i] = m_Sensors[i].Detected ? 1 : 0;
        }

        return array;
    }

    public int ToDecimal()
    {
        int[] array = ToArray();
        int value = 0;

        for (int i = 0; i < array.Length; i++)
        {
            int exp = array.Length - i - 1;
            value += (int)Math.Pow(2, exp) * array[i];
        }

        return value;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        Array.ForEach(ToArray(), x => builder.Append(x));
        builder.Append(" = ").Append(ToDecimal());
        return builder.ToString();
    }

    public void LateUpdate()
    {
        Debug.Log(ToString());
    }
}
