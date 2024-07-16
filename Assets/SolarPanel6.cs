using UnityEngine;
using UnityEngine.UI;

public class SolarPanel6 : MonoBehaviour
{
    public Light sun;
    public ParticleSystem rainParticleSystem, cloudParticleSystem, fogParticleSystem;
    public Text powerOutputText;
    public Text energyGeneratedText; // Ensure you assign this in the inspector

    public float panelArea = 1.0f;
    public float panelEfficiency = 0.15f;
    public float basePerformanceRatio = 0.75f;

    public SolarPanelDustAccumulator dustAccumulator; // Reference to the dust accumulator

    // Use the 'new' keyword to explicitly hide the inherited member
    public new float CurrentPowerOutput { get; private set; }

    private float accumulatedEnergy = 0f; // in Watt-hours

    void Update()
    {
        UpdatePowerOutput();
        UpdateEnergyGenerated();
    }

    private void UpdatePowerOutput()
    {
        float sunElevationSine = CalculateSimulatedElevation();
        if (sunElevationSine > 0)
        {
            float dustEffect = 1.0f - dustAccumulator.GetCurrentDustAmount(); // Calculate the dust effect based on the current dust amount
            CurrentPowerOutput = CalculateSolarPower(sunElevationSine, dustEffect);
            powerOutputText.text = $"Panel: {CurrentPowerOutput.ToString("F2")} Watts";
        }
        else
        {
            CurrentPowerOutput = 0;
            powerOutputText.text = $"Panel: {CurrentPowerOutput.ToString("F2")} Watts";
        }
    }

    private void UpdateEnergyGenerated()
    {
        // Calculate energy generated in this frame (Watt-hours)
        float energyThisFrame = CurrentPowerOutput * Time.deltaTime;
        accumulatedEnergy += energyThisFrame;

        // Update energy generated text
        if (energyGeneratedText != null)
        {
            energyGeneratedText.text = $"Energy Generated: {accumulatedEnergy.ToString("F2")} Wh";
        }
    }

    private float CalculateSolarPower(float sunElevationSine, float dustEffect)
    {
        float irradiance = sun.intensity * 1000; // Assuming sun intensity is scaled to 1 for max 1000 W/m^2
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
        if (rainParticleSystem.gameObject.activeInHierarchy && cloudParticleSystem.gameObject.activeInHierarchy)
        {
            float rainImpact = Mathf.Clamp01(rainParticleSystem.emission.rateOverTime.constant / 100000);
            float cloudImpact = Mathf.Clamp01(cloudParticleSystem.emission.rateOverTime.constant / 100000);
            float combinedImpact = rainImpact+cloudImpact;
            return 0.2f + 0.2f * (1f - combinedImpact);  // Keeps output between 20% to 40%
        }
        return 0.0f; // No rain or cloud means no reduction in solar power output
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

    private void UpdateDustIntensityDisplay()
    {
        // Example: Display dust intensity if needed
        float dustIntensity = dustAccumulator.GetCurrentDustAmount();
        // Update dust intensity display logic here if needed
    }

    // Method to get total energy generated
    public float GetTotalEnergyGenerated()
    {
        return accumulatedEnergy;
    }
}
