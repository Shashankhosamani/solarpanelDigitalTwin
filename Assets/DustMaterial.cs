using UnityEngine;

public class DustMaterial : MonoBehaviour
{
    public Material dustMaterial; // Individual dust material for this panel
    private float dustAmount = 1.0f; // Current amount of dust
    private bool isCleaning = false; // Flag to indicate if the panel is being cleaned

    void Start()
    {
        // Ensure dustMaterial is assigned and has the _DustAmount property
        if (dustMaterial == null || !dustMaterial.HasProperty("_DustAmount"))
        {
            Debug.LogError("Material is not assigned or does not have a _DustAmount property.");
        }
        else
        {
            // Initialize the material's dust amount
            dustMaterial.SetFloat("_DustAmount", dustAmount);
        }
    }

    void Update()
    {
        if (!isCleaning)
        {
            // Simulate dust accumulation over time
            dustAmount += Time.deltaTime * 0.01f; // Adjust accumulation speed as needed
            dustAmount = Mathf.Clamp01(dustAmount);

            // Update the material's dust amount
            if (dustMaterial != null && dustMaterial.HasProperty("_DustAmount"))
            {
                dustMaterial.SetFloat("_DustAmount", dustAmount);
            }
        }
    }

    public float GetDustAmount()
    {
        return dustAmount;
    }

    public void ReduceDust(float amount)
    {
        dustAmount -= amount;
        dustAmount = Mathf.Clamp01(dustAmount);

        // Update the material's dust amount
        if (dustMaterial != null && dustMaterial.HasProperty("_DustAmount"))
        {
            dustMaterial.SetFloat("_DustAmount", dustAmount);
        }
    }

    public void SetCleaning(bool cleaning)
    {
        isCleaning = cleaning;
    }
}
