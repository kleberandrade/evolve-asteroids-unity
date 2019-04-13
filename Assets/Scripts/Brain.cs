using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public Chromosome Chromosome { get; set; }

    private SensorManager m_SensorManager;
    private ShipController m_ShipController;

    private Dictionary<int, int[]> Commands = new Dictionary<int, int[]>()
    {
        [0] = new int[] { 0, 0, 0 },
        [1] = new int[] { 1, 0, 0 },
        [2] = new int[] { 0, 1, 0 },
        [3] = new int[] { 1, 1, 0 },
        [4] = new int[] { 0, -1, 0 },
        [5] = new int[] { 1, -1, 0 },
        [6] = new int[] { 0, 0, 1 },
        [7] = new int[] { 1, 0, 1 },
        [8] = new int[] { 0, 1, 1 },
        [9] = new int[] { 1, 1, 1 },
        [10] = new int[] { 0, -1, 1 },
        [11] = new int[] { 1, -1, 1 },
    };

    private void Awake()
    {
        m_SensorManager = GetComponent<SensorManager>();
        m_ShipController = GetComponent<ShipController>();
    }

    private void Update()
    {
        if (Chromosome != null)
        {
            int sensor = m_SensorManager.ToDecimal();
            int control = Chromosome[sensor];
            int[] data = Commands[control];

            m_ShipController.Vertical = data[0];
            m_ShipController.Horizontal = data[1];
            m_ShipController.Fire = data[2] == 1 ? true : false;
        }
    }
}
