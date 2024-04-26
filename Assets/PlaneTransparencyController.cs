using UnityEngine;
using UnityEngine.UI;

public class PlaneTransparencyController : MonoBehaviour
{
    public Slider transparencySlider; // Reference to the UI slider
    public Renderer planeRenderer; // Reference to the Renderer of the plane
    public Text transparencyText; // Reference to the text object

    public float transparency; // Add this line

    void Start()
    {
        // Initialize the plane's transparency to zero
        SetTransparency(0f); // Set initial transparency to zero
        transparencySlider.value = 0f; // Set slider's initial value to zero
        transparencySlider.onValueChanged.AddListener(SetTransparency); // Listen for value changes

        // Assign the text object's reference
        transparencyText = GameObject.Find("TextObjectName").GetComponent<Text>();
    }

    public void SetTransparency(float alpha)
    {
        Debug.Log($"SetTransparency called with alpha: {alpha}");
        // Adjust the transparency of the plane's material
        Color newColor = planeRenderer.material.color;
        newColor.a = alpha;
        
        planeRenderer.material.color = newColor;

        // Update the text object with the transparency percentage
        int transparencyPercentage = Mathf.RoundToInt(alpha * 100);
        transparencyText.text = transparencyPercentage + "%";

        transparency = alpha;// Assign the transparency value
        Debug.Log($"alpha: {alpha}");
        

    }
}