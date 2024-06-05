using UnityEngine;
using UnityEngine.UI;

public class wiperAnimation : MonoBehaviour
{
    public Slider transparencySlider; // Reference to the UI slider
    public Animator wiperAnimator; // Reference to the Animator component
    public float startThreshold = 0.5f; // Threshold to start the animation
    public float stopThreshold = 0.0f; // Threshold to stop the animation

    private PlaneTransparencyController planeTransparencyController;
    private bool wiperActive = false;

    void Start()
    {
        transparencySlider.onValueChanged.AddListener(CheckTransparency); // Add listener to the slider value change

        // Find the PlaneTransparencyController script in the scene
        planeTransparencyController = FindObjectOfType<PlaneTransparencyController>();
        if (planeTransparencyController)
        {
            Debug.Log("PlaneTransparencyController found");
        }
        if (planeTransparencyController == null)
        {
            Debug.LogError("PlaneTransparencyController not found in the scene.");
        }

        if (wiperAnimator == null)
        {
            Debug.LogError("Wiper Animator not assigned.");
        }
    }

    void Update()
    {
        if (planeTransparencyController != null)
        {
            float currentTransparency = planeTransparencyController.transparency;

            // If the wiper is active, decrease the transparency
            if (wiperActive)
            {
                float newTransparency = Mathf.Max(0f, currentTransparency - Time.deltaTime * 0.1f);
                planeTransparencyController.SetTransparency(newTransparency);
                planeTransparencyController.UpdateSliderValue(newTransparency); // Update the slider value

                // Check if transparency has fallen below a lower limit to deactivate the wiper
                if (newTransparency <= 0.0f) // Adjust this value as needed
                {
                    wiperAnimator.SetTrigger("Deactivate");
                    Debug.Log("DeactivateWiper trigger set");
                    wiperActive = false;
                }
            }
            
        }
    }

    void CheckTransparency(float value)
    {
        if (planeTransparencyController != null)
        {
            float currentTransparency = planeTransparencyController.transparency;
            Debug.Log($"Current Transparency: {currentTransparency}");

            if (currentTransparency >= startThreshold)
            {
                if (!wiperActive)
                {
                    wiperAnimator.SetTrigger("Activate");
                    Debug.Log("ActivateWiper trigger set");
                    wiperActive = true;
                }
            }
            else if (currentTransparency < stopThreshold)
            {
                if (wiperActive)
                {
                    wiperAnimator.SetTrigger("Deactivate");
                    Debug.Log("DeactivateWiper trigger set");
                    wiperActive = false;
                }
            }
        }
    }
}
