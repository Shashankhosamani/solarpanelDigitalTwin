using UnityEngine;
using UnityEngine.UI;

public class SolarPanelSimulation : MonoBehaviour
{
    public Light sun;
    public Slider dustSlider;
    public ParticleSystem rainParticleSystem, cloudParticleSystem, fogParticleSystem;
    public Text powerOutputText, dustIntensityText;

    public float panelArea = 1.0f;
    public float panelEfficiency = 0.15f;
    public float basePerformanceRatio = 0.75f;

    public BatteryManager batteryManager;  // Reference to the BatteryManager

    public float CurrentPowerOutput { get; private set; }

    void Update()
    {
        UpdatePowerOutput();
        if (batteryManager != null)
        {
            batteryManager.AddEnergy(CurrentPowerOutput * Time.deltaTime / 3600); // Convert watts to kWh
        }
        UpdateDustIntensityDisplay();
    }

    private void UpdatePowerOutput()
    {
        float sunElevationSine = CalculateSimulatedElevation();
        if (sunElevationSine > 0)
        {
            CurrentPowerOutput = CalculateSolarPower(sunElevationSine);
            powerOutputText.text = CurrentPowerOutput.ToString("F2") + " Watts";
        }
        else
        {
            CurrentPowerOutput = 0;
            powerOutputText.text = "0 Watts"; // Sun is below horizon
        }
    }

    private void UpdateDustIntensityDisplay()
    {
        dustIntensityText.text = "Dust Intensity: " + (dustSlider.value * 100).ToString("F0") + "%";
    }

    private float CalculateSolarPower(float sunElevationSine)
    {
        float irradiance = sun.intensity * 1000; // Assuming sun intensity is scaled to 1 for max 1000 W/m^2
        float dustEffect = 1.0f - (dustSlider.value * 0.25f);
        float rainEffect = 1.0f - GetRainEffect();
        float cloudEffect = 1.0f - GetCloudEffect();
        float fogEffect = 1.0f - GetFogEffect();
        float performanceRatio = basePerformanceRatio * dustEffect * rainEffect * cloudEffect * fogEffect;
        return panelArea * panelEfficiency * irradiance * performanceRatio * sunElevationSine;
    }

    private float CalculateSimulatedElevation()
    {
        float rotationX = sun.transform.rotation.eulerAngles.x;
        if (rotationX > 180) rotationX -= 360;
        return Mathf.Sin(rotationX * Mathf.Deg2Rad);
    }

    private float GetRainEffect()
    {
        return rainParticleSystem.gameObject.activeInHierarchy ? Mathf.Clamp01(rainParticleSystem.emission.rateOverTime.constant / 1000) : 0;
    }

    private float GetCloudEffect()
    {
        return cloudParticleSystem.gameObject.activeInHierarchy ? Mathf.Clamp01(cloudParticleSystem.emission.rateOverTime.constant / 1000) : 0;
    }

    private float GetFogEffect()
    {
        return fogParticleSystem.gameObject.activeInHierarchy ? Mathf.Clamp01(fogParticleSystem.emission.rateOverTime.constant / 1000) : 0;
    }
}
