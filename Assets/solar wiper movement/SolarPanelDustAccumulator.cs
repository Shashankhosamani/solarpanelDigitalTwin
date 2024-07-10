using UnityEngine;

public class SolarPanelDustAccumulator : MonoBehaviour
{
    public Material panelMaterial; // Material for this panel
    public float dustAccumulationRate = 0.1f; // Dust increase per second

    private float currentDustAmount; // Current dust amount on this panel
    private bool isAccumulating = true; // Flag to track if dust is currently accumulating
    private bool isCleaning = false; // Flag to track if this panel is currently being cleaned

    void Start()
    {
        if (panelMaterial == null)
        {
            Debug.LogError("Panel Material not assigned to SolarPanelDustAccumulator script.");
            enabled = false; // Disable script if material is not assigned
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

            // Update material properties for this panel
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
        // Update shader properties or material properties based on the current dust amount
        panelMaterial.SetFloat("_DustAmount", currentDustAmount);
    }
}
