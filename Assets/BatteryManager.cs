using UnityEngine;
using UnityEngine.UI;

public class BatteryManager : MonoBehaviour
{
    public float batteryCapacity = 20; // Battery capacity in kW
    private float batteryCurrentCharge; // Current charge of the battery in kWh
    private float totalDayEnergy; // Accumulated energy during the day

    public float roadLightsLoad = 40f;
    public float homeLoad = 35f;
    public float dayTimeHomeLoad = 12f;

    public Text batteryStatusText;
    public Text energyDrainText;
    public Text dayEnergyText;

    public GameObject sun;

    private bool canDrain = true; // Control flag for draining

    void Start()
    {
        batteryCurrentCharge = Random.Range(batteryCapacity * 0.6f, batteryCapacity * 0.8f);
        canDrain = true; // Start with the ability to drain energy
        UpdateUI();
    }
    public void totalenergy(float kWh)
    {
        
    }

    public void AddEnergy(float kWh)
    {
        batteryCurrentCharge = Mathf.Min(batteryCapacity, batteryCurrentCharge + kWh);
        totalDayEnergy += kWh;
        if (batteryCurrentCharge >= batteryCapacity * 0.6f && !canDrain)
        {
            canDrain = true; // Allow draining again once recharged to 60%
        }

        UpdateUI();
    }

    void Update()
    {
        if (canDrain)
        {
            float energyConsumed = CalculateEnergyConsumed();
            batteryCurrentCharge = Mathf.Max(0, batteryCurrentCharge - energyConsumed);

            if (batteryCurrentCharge == 0)
            {
                canDrain = false; // Stop draining if the battery is empty
            }
        }

        // Simulate adding energy based on sunlight conditions
        if (sun.transform.eulerAngles.x > 0 && sun.transform.eulerAngles.x < 180)
        {
            AddEnergy(0.0001f); // Modify this value based on your solar power generation logic
        }

        UpdateUI();

    }

    private float CalculateEnergyConsumed()
    {
        float load;
        float sunElevationSine = CalculateSimulatedElevation();
        if (sunElevationSine > 0)
        {
            load = dayTimeHomeLoad;
        }
        else
        {
            totalDayEnergy = 0; // Reset at night
            load = roadLightsLoad + homeLoad;
        }
        float realSecondsPerGameSecond = 220f / 24f / 3600f;
        return load * Time.deltaTime * realSecondsPerGameSecond;
    }

    private float CalculateSimulatedElevation()
    {
        float rotationX = sun.transform.rotation.eulerAngles.x;
        if (rotationX > 180) rotationX -= 360;
        return Mathf.Sin(rotationX * Mathf.Deg2Rad);
    }

    private float GetBatteryPercentage()
    {
        return (batteryCurrentCharge / batteryCapacity) * 100;
    }

    private void UpdateUI()
    {
        batteryStatusText.text = $"Battery Charge: {batteryCurrentCharge:F2} kWh ({GetBatteryPercentage():F2}%)";

        if (energyDrainText != null && canDrain)
        {
            float lastEnergyDrain = CalculateEnergyConsumed();
            energyDrainText.text = $"Energy Drained: {lastEnergyDrain * 1000:F2} Watts";
        }

        dayEnergyText.text = $"Total Day Energy: {totalDayEnergy:F2} kWh";
    }
}
