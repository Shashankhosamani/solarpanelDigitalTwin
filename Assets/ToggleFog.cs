using UnityEngine;
using UnityEngine.UI;

public class ToggleFog : MonoBehaviour
{
    public Slider fogSlider;  // Reference to the UI slider
    public Text fogValueText; // Reference to the UI Text

    void Start()
    {
        // Initialize the slider value and add a listener for value changes
        fogSlider.onValueChanged.AddListener(AdjustFogDensity);
        fogSlider.value = RenderSettings.fogDensity;

        // Set slider's min and max values to ensure they are within desired range
        fogSlider.minValue = 0.0f;
        fogSlider.maxValue = 0.02f;

        // Update the text with the initial value
        UpdateFogValueText(fogSlider.value);
    }

    void AdjustFogDensity(float value)
    {
        // Clamp the value between 0 and 0.02
        value = Mathf.Clamp(value, 0.0f, 0.02f);

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = Color.gray;
        RenderSettings.fogDensity = value;

        // Update the text with the new value
        UpdateFogValueText(value);
    }

    void UpdateFogValueText(float value)
    {
        fogValueText.text = $"Fog Density: {value:F4}";
    }
}
