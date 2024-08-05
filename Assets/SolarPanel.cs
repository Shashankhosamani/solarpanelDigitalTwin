using UnityEngine;
using UnityEngine.UI;

public class SolarPanel : MonoBehaviour
{
    public float efficiency = 0.15f; // Default efficiency is 15%
    public float area = 0.33f;
    public Text powerText; // Add this line

    public float CalculatePower(float radiation)
    {
        float power = radiation * area * efficiency;
        UpdatePowerText(power); // Add this line
        return power;
    }

    private void UpdatePowerText(float power)
    {
        if (powerText != null)
        {
            powerText.text = $" {power} W";
        }
    }
}