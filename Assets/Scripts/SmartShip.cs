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

            int[] data = Helper.DecimalToBinary(sensor, m_ControlSize);

            m_Ship.Fire = data[0] == 1 ? true : false;
            m_Ship.Horizontal = data[1] - data[2];
            m_Ship.Vertical = data[3] - data[4];
        }
    }
}
