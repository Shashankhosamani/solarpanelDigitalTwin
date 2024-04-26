using UnityEngine;
using UnityEngine.UI;

public class SolarPanel7 : SolarPanelBase // Inherits from SolarPanelBase
{
    public Light sun;
    public ParticleSystem rainParticleSystem, cloudParticleSystem, fogParticleSystem;
    public Text powerOutputText;
    public float panelArea = 1.0f;
    public float panelEfficiency = 0.15f;
    public float basePerformanceRatio = 0.75f;

     // Reference to the BatteryManager

    void Update()
    {
        UpdatePowerOutput(); // Call the overridden method
         // Update the battery manager
    }

    protected override void UpdatePowerOutput()
    {
        float sunElevationSine = CalculateSimulatedElevation();
        if (sunElevationSine > 0)
        {
            currentPowerOutput = CalculateSolarPower(sunElevationSine);
            powerOutputText.text = ($"Panel-7: {currentPowerOutput.ToString("F2")} Watts");
        }
        else
        {
            currentPowerOutput = 0;
            powerOutputText.text = ($"Panel-7: {currentPowerOutput.ToString("F2")} Watts"); // Sun is below the horizon
        }
    }

    private float CalculateSolarPower(float sunElevationSine)
    {
        float irradiance = sun.intensity * 1000; // Assuming sun intensity is scaled to 1 for max 1000 W/m^2
        float rainEffect = 1.0f - GetRainEffect();
        float cloudEffect = 1.0f - GetCloudEffect();
        float fogEffect = 1.0f - GetFogEffect();
        float performanceRatio = basePerformanceRatio * rainEffect * cloudEffect * fogEffect;
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
