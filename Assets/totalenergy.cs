using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class totalenergy : MonoBehaviour
{
    public List<SolarPanel6> solarPanels; // List of all solar panels
    public Text totalEnergyGeneratedText;

    void Update()
    {
        UpdateTotalEnergyGenerated();
    }

    private void UpdateTotalEnergyGenerated()
    {
        float totalEnergy = 0f;

        foreach (SolarPanel6 panel in solarPanels)
        {
            totalEnergy += panel.GetTotalEnergyGenerated();
        }

        // Update total energy generated text
        if (totalEnergyGeneratedText != null)
        {
            totalEnergyGeneratedText.text = $"Total Energy:{totalEnergy.ToString("F2")} Wh";
        }
    }
}
