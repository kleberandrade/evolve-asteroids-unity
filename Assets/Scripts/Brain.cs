using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public static readonly Dictionary<int, int[]> Commands = new Dictionary<int, int[]>()
    {
        [0]  = new int[] { 0, 0, 0 },
        [1]  = new int[] { 1, 0, 0 },
        [2]  = new int[] { 0, 1, 0 },
        [3]  = new int[] { 1, 1, 0 },
        [4]  = new int[] { 0, -1, 0 },
        [5]  = new int[] { 1, -1, 0 },
        [6]  = new int[] { 0, 0, 1 },
        [7]  = new int[] { 1, 0, 1 },
        [8]  = new int[] { 0, 1, 1 },
        [9]  = new int[] { 1, 1, 1 },
        [10] = new int[] { 0, -1, 1 },
        [11] = new int[] { 1, -1, 1 }
    };

    public Chromosome m_Chromosome { get; set; }

    private Ship m_Ship;
    private SensorManager m_SensorManager;

    [SerializeField]
    private bool UseSmartControl;

    [Header("Chromosome")]
    [SerializeField]
    private int m_ChromosomeSize;

    public void Start()
    {
        m_SensorManager = GetComponent<SensorManager>();
        m_Ship = GetComponent<Ship>();

        m_Chromosome = new Chromosome(m_ChromosomeSize);
        
        m_Ship.UseSmartControl = UseSmartControl;
    }

    public void Update()
    {
        if (UseSmartControl)
        {
            int sensor = m_SensorManager.ToDecimal();
            int control = m_Chromosome[sensor];

            int[] data = Commands[control];

            m_Ship.Vertical = data[0];
            m_Ship.Horizontal = data[1];
            m_Ship.Fire = data[2] == 1 ? true : false;
        }
    }
}
