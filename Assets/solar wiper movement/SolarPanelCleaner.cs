using UnityEngine;
using System.Collections;

public class SolarPanelCleaner : MonoBehaviour
{
    public Material[] panelMaterials; // Array of materials for each panel
    public SolarPanelDustAccumulator[] dustAccumulators; // Array of dust accumulators for each panel
    public float wipeWidth = 0.1f;
    public Animator wiperAnimator;
    public string wiperAnimationTrigger = "StartWipe";
    public string wiperStopTrigger = "StopWipe";
    public float dustThreshold = 0.7f; // Threshold to start cleaning

    private float cleaningDuration = 30f; // Total cleaning duration for each panel
    private int currentPanelIndex = 0; // Index of the current panel being cleaned
    private bool isCleaning = false; // Flag to track if a panel is currently being cleaned

    void Start()
    {
        // Check if panel materials and dust accumulators are assigned
        if (panelMaterials == null || panelMaterials.Length == 0 || dustAccumulators == null || dustAccumulators.Length == 0)
        {
            Debug.LogError("Panel Materials or Dust Accumulators not assigned to SolarPanelCleaner script.");
            enabled = false; // Disable script if materials or accumulators are not assigned
        }
    }

    void Update()
    {
        // Start cleaning process if not already cleaning and no panels are currently being cleaned
        if (!isCleaning)
        {
            float currentDustAmount = dustAccumulators[currentPanelIndex].GetCurrentDustAmount();
            if (currentDustAmount >= dustThreshold)
            {
                StartCleaning(currentPanelIndex); // Pass the panel index to start cleaning
            }
        }
    }

    void StartCleaning(int panelIndex)
    {
        isCleaning = true;

        // Stop dust accumulation for all panels
        foreach (SolarPanelDustAccumulator accumulator in dustAccumulators)
        {
            accumulator.StopAccumulation();
        }

        // Reset wipe progress
        UpdateMaterialProperties(panelIndex, 0f);

        if (wiperAnimator != null)
        {
            wiperAnimator.SetTrigger(wiperAnimationTrigger); // Trigger wiper animation
        }
        Debug.Log("Started cleaning panel " + panelIndex);

        // Start coroutine for cleaning process
        StartCoroutine(CleanPanel(panelIndex));
    }

    IEnumerator CleanPanel(int panelIndex)
    {
        float currentProgress = 0f;
        float targetCleaningProgress = 0.37f; // Target cleaning progress to stop at

        while (currentProgress < targetCleaningProgress)
        {
            // Update shader properties for this panel
            UpdateMaterialProperties(panelIndex, currentProgress);

            // Increase progress gradually
            currentProgress += Time.deltaTime / cleaningDuration;

            yield return null;
        }

        FinishCleaning(panelIndex);
    }

    void FinishCleaning(int panelIndex)
    {
        isCleaning = false;
        dustAccumulators[panelIndex].ResetDustAmount(); // Reset dust amount after cleaning

        if (wiperAnimator != null)
        {
            wiperAnimator.SetTrigger(wiperStopTrigger); // Stop animation for wiper
        }
        Debug.Log("Finished cleaning panel " + panelIndex);

        // Move to the next panel immediately
        currentPanelIndex = (currentPanelIndex + 1) % panelMaterials.Length;

        // Check if the next panel needs cleaning
        if (dustAccumulators[currentPanelIndex].GetCurrentDustAmount() >= dustThreshold)
        {
            StartCleaning(currentPanelIndex); // Start cleaning the next panel immediately
        }
        else
        {
            // Resume dust accumulation for all panels if no more panels need cleaning
            foreach (SolarPanelDustAccumulator accumulator in dustAccumulators)
            {
                accumulator.ResumeAccumulation();
            }
        }

        // Reset wipe progress to 0 after cleaning
        UpdateMaterialProperties(panelIndex, 0f);
    }

    void UpdateMaterialProperties(int panelIndex, float wipeProgress)
    {
        // Update shader properties or material properties based on the wipe progress
        panelMaterials[panelIndex].SetFloat("_WipeProgress", wipeProgress);
        panelMaterials[panelIndex].SetFloat("_WipeWidth", wipeWidth);
        Debug.Log($"Panel {panelIndex} wipe progress: {wipeProgress}");
    }
}
