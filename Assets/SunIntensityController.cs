using UnityEngine;

public class SunIntensityController : MonoBehaviour
{
    [SerializeField]
    private AppManager appManager; // Reference to the AppManager

    [SerializeField]
    private Light sunLight; // Reference to the directional light representing the sun

    private void OnEnable()
    {
        appManager.OnWeatherDataUpdated += AdjustSunIntensity;
    }

    private void OnDisable()
    {
        appManager.OnWeatherDataUpdated -= AdjustSunIntensity;
    }

    private void AdjustSunIntensity(float shortwaveRadiation, bool isDay)
    {
        if (isDay)
        {
            // Assuming the maximum shortwave radiation value to be around 1000 W/m²
            float maxRadiation = 1000f;
            float intensity = Mathf.Clamp01(shortwaveRadiation / maxRadiation); // Normalize the intensity to be between 0 and 1
            sunLight.intensity = intensity * 1.5f; // Adjust the multiplier as needed for visual effect

            Debug.Log($"Sun intensity adjusted to: {sunLight.intensity}");
        }
        else
        {
            sunLight.intensity = 0.1f; // Set a low intensity value for night time
            Debug.Log("Sun intensity set for night time");
        }
    }
}
