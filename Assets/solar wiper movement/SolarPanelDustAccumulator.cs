using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SolarPanelDustAccumulator : MonoBehaviour
{
    public List<Material> panelMaterials; // List of materials for the panels
    public float dustAccumulationRate = 0.01f; // Dust increase per second
    public Slider dustSlider; // Reference to the UI slider

    private float currentDustAmount; // Current dust amount on this panel
    private bool isAccumulating = true; // Flag to track if dust is currently accumulating
    private bool isCleaning = false; // Flag to track if this panel is currently being cleaned

    void Start()
    {
        if (panelMaterials == null || panelMaterials.Count == 0)
        {
            Debug.LogError("Panel Materials not assigned to SolarPanelDustAccumulator script.");
            enabled = false; // Disable script if materials are not assigned
            return;
        }

        if (dustSlider != null)
        {
            dustSlider.onValueChanged.AddListener(OnSliderValueChanged);
            dustSlider.minValue = 0f;
            dustSlider.maxValue = 1f;
            dustSlider.value = currentDustAmount;
        }

        currentDustAmount = 0f; // Initialize dust amount to clean
    }

    void Update()
    {
        if (isAccumulating && !isCleaning)
        {
            // Accumulate dust over time
            currentDustAmount += dustAccumulationRate * Time.deltaTime;
            currentDustAmount = Mathf.Clamp01(currentDustAmount);

            // Update slider value if it exists
            if (dustSlider != null && !dustSlider.interactable)
            {
                dustSlider.value = currentDustAmount;
            }

            // Update material properties for all panels
            UpdateMaterialProperties();
        }
    }

    public float GetCurrentDustAmount()
    {
        return currentDustAmount;
    }

    public void ResetDustAmount()
    {
        currentDustAmount = 0f;
        UpdateMaterialProperties();
    }

    public void StopAccumulation()
    {
        isAccumulating = false;
    }

    public void ResumeAccumulation()
    {
        isAccumulating = true;
    }

    public bool IsAccumulating()
    {
        return isAccumulating;
    }

    public void StartCleaning()
    {
        isCleaning = true;
    }

    public void FinishCleaning()
    {
        isCleaning = false;
    }

    public bool IsCleaning()
    {
        return isCleaning;
    }

    void UpdateMaterialProperties()
    {
        foreach (var material in panelMaterials)
        {
            // Update shader properties or material properties based on the current dust amount
            material.SetFloat("_DustAmount", currentDustAmount);
        }
    }

    void OnSliderValueChanged(float value)
    {
        currentDustAmount = value;
        UpdateMaterialProperties();
    }
}
