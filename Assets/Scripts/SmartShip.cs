using System;
using UnityEngine;

public class SmartShip : MonoBehaviour
{
    public Chromosome m_Chromosome { get; set; }

    private Ship m_Ship;
    private SensorManager m_SensorManager;

    [SerializeField]
    private bool UseSmartControl;

    [Header("Chromosome")]
    [SerializeField]
    private int m_ChromosomeSize;

    [SerializeField]
    private int m_ControlSize;

    private int[] m_Data;

    public void Start()
    {
        m_SensorManager = GetComponent<SensorManager>();
        m_Ship = GetComponent<Ship>();

        m_Chromosome = new Chromosome(m_ChromosomeSize);
        
        m_Data = new int[m_ControlSize];

        m_Ship.UseSmartControl = UseSmartControl;
    }

    public void Update()
    {
        if (UseSmartControl)
        {
            int sensor = m_SensorManager.ToDecimal();

            m_Data = Helper.DecimalToBinary(sensor, m_ControlSize);

            m_Ship.Fire = m_Data[0] == 1 ? true : false;
            m_Ship.Horizontal = m_Data[1] - m_Data[2];
            m_Ship.Vertical = m_Data[3] - m_Data[4];
        }
    }
}
