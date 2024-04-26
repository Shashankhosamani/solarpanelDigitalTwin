using UnityEngine;
using UnityEngine.UI;

public class SolarFarmManager : MonoBehaviour
{
    public SolarPanelBase[] solarPanels;
    public Text totalPowerOutputText;

    public BatteryManager batteryManager;
    public GameObject sun;
    public EnergyGraph energyGraph; // Reference to the EnergyGraph component

    private float totalEnergyGeneratedToday = 0f;
    private float lastUpdateTime = 0f;
    private int lastDayChecked = -1;

    void Start()
    {
        lastDayChecked = System.DateTime.Now.Day;
    }

    void Update()
    {
        if (!IsDaytime())
        {
            if (IsNewDay())
            {
                // Log and reset daily energy at the end of the day
                energyGraph.AddPoint(totalEnergyGeneratedToday);
                Debug.Log("New day detected at night. Resetting energy.");
                totalEnergyGeneratedToday = 0;
            }
            return;
        }

        // Calculate total power from all panels
        float totalPower = CalculateTotalPower();

        // Calculate energy generated in this update
        float energyGeneratedThisUpdate = CalculateEnergyGenerated();
        totalEnergyGeneratedToday += energyGeneratedThisUpdate;

        // Update UI and Battery Manager
        totalPowerOutputText.text = $"{totalPower:F2} Watts";
        batteryManager.AddEnergy(energyGeneratedThisUpdate);

        // Check if it's a new day to reset the daily energy
        if (IsNewDay())
        {
            energyGraph.AddPoint(totalEnergyGeneratedToday);
            Debug.Log("New day has started. Resetting daily energy.");
            totalEnergyGeneratedToday = 0;
        }
    }

    private float CalculateTotalPower()
    {
        float totalPower = 0f;
        foreach (SolarPanelBase panel in solarPanels)
        {
            if (panel != null)
            {
                totalPower += panel.CurrentPowerOutput;
            }
        }
        return totalPower;
    }

    private float CalculateEnergyGenerated()
    {
        float realSecondsPerGameSecond = 220f / 24f / 3600f; // Convert game day to real seconds
        float timeElapsedHours = (Time.time - lastUpdateTime) * realSecondsPerGameSecond / 3600f;
        lastUpdateTime = Time.time;
        float totalPowerInkWh = CalculateTotalPower() / 1000; // Convert from Watts to kiloWatts
        return totalPowerInkWh * timeElapsedHours;
    }

    private bool IsNewDay()
    {
        int currentDay = System.DateTime.Now.Day;
        if (currentDay != lastDayChecked && !IsDaytime())
        {
            lastDayChecked = currentDay;
            return true;
        }
        return false;
    }

    private bool IsDaytime()
    {
        return CalculateSimulatedElevation() > 0;
    }

    private float CalculateSimulatedElevation()
    {
        float rotationX = sun.transform.rotation.eulerAngles.x;
        if (rotationX > 180) rotationX -= 360;
        return Mathf.Sin(rotationX * Mathf.Deg2Rad);
    }
}
