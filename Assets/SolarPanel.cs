using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    public int id;
    public float efficiency = 0.15f; // Default efficiency is 20%
    public float area = 0.33f; // Default area is 1 m²

    public float CalculatePower(float radiation)
    {
        return radiation * area * efficiency;
    }
}
