using UnityEngine;

public class SolarPanelDustAccumulator : MonoBehaviour
{
    private float dustAccumulationRate = 0.0001f; // Dust increase per second
    public Material material; // Single material for the panel

    private float currentDustAmount; // Current dust amount on this panel
    private bool isAccumulating = true; // Flag to track if dust is currently accumulating
    private bool isCleaning = false; // Flag to track if this panel is currently being cleaned

    void Start()
    {
        if (material == null)
        {
            Debug.LogError("Material not assigned to SolarPanelDustAccumulator script.");
            enabled = false; // Disable script if material is not assigned
            return;
        }

        currentDustAmount = 0f; // Initialize dust amount to clean
        UpdateMaterialProperties();
    }

    void Update()
    {
        if (isAccumulating && !isCleaning)
        {
            // Accumulate dust over time
            currentDustAmount += dustAccumulationRate * Time.deltaTime;
            currentDustAmount = Mathf.Clamp01(currentDustAmount);

            // Update material properties
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
        if (material != null)
        {
            // Update shader properties or material properties based on the current dust amount
            material.SetFloat("_DustAmount", currentDustAmount);
        }
    }
}
