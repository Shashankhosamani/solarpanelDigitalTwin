using UnityEngine;
using UnityEngine.UI;

public class SolarPanel6 : SolarPanelBase
{
    public Light sun;
    public Slider dustSlider;
    public ParticleSystem rainParticleSystem, cloudParticleSystem, fogParticleSystem;
    public Text powerOutputText, dustIntensityText;

    public float panelArea = 1.0f;
    public float panelEfficiency = 0.15f;
    public float basePerformanceRatio = 0.75f;

    public BatteryManager batteryManager;  // Reference to the BatteryManager

    // Use the 'new' keyword to explicitly hide the inherited member
    public new float CurrentPowerOutput { get; private set; }

    void Update()
    {
        UpdatePowerOutput();
        if (batteryManager != null)
        {
            // Convert watts to kWh and add to battery storage
            batteryManager.AddEnergy(CurrentPowerOutput * Time.deltaTime / 3600);
        }
        UpdateDustIntensityDisplay();
    }

    protected override void UpdatePowerOutput()
    {
        float sunElevationSine = CalculateSimulatedElevation();
        if (sunElevationSine > 0)
        {
            CurrentPowerOutput = CalculateSolarPower(sunElevationSine);
            powerOutputText.text = $"Panel-6:= {CurrentPowerOutput.ToString("F2")} Watts";
        }
        else
        {
            CurrentPowerOutput = 0;
        }
        powerOutputText.text = $"Panel-6:= {CurrentPowerOutput.ToString("F2")} Watts";
    }

    private void UpdateDustIntensityDisplay()
    {
        dustIntensityText.text = "Dust Intensity: " + (dustSlider.value * 100).ToString("F0") + "%";
    }

    private float CalculateSolarPower(float sunElevationSine)
    {
        float irradiance = sun.intensity * 1000; // Assuming sun intensity is scaled to 1 for max 1000 W/m^2
        float dustEffect = 1.0f - (dustSlider.value * 0.5f);
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

        if (rainParticleSystem.gameObject.activeInHierarchy)
        {
            // Assuming emission rate can give us a scale from 0 to 1000
            // Adjusting formula to have the worst-case scenario (heaviest rain) at 20% production capacity
            // and the best case during rain at 40% production capacity.
            float rainImpact = Mathf.Clamp01(rainParticleSystem.emission.rateOverTime.constant / 100000);
            return 0.2f + 0.2f * (1f - rainImpact);  // Keeps output between 20% to 40%
        }
        return 0.0f; // No rain means no reduction in solar power output


    }



    private float GetCloudEffect()
    {
        if (cloudParticleSystem.gameObject.activeInHierarchy)
        {
            float cloudImpact = Mathf.Clamp01(cloudParticleSystem.emission.rateOverTime.constant / 100000);
            return 0.3f + 0.7f * (1f - cloudImpact);  // Keeps output between 30% to 100%
        }
        return 0.0f;
    }

    private float GetFogEffect()
    {
        if (fogParticleSystem.gameObject.activeInHierarchy)
        {
            float fogImpact = Mathf.Clamp01(fogParticleSystem.emission.rateOverTime.constant / 1000);
            return 0.5f + 0.5f * (1f - fogImpact);  // Keeps output between 50% to 100%
        }
        return 0.0f;
    }
}
