using UnityEngine;

public class SolarPanelBase : MonoBehaviour
{
    protected float currentPowerOutput;

    public float CurrentPowerOutput
    {
        get { return currentPowerOutput; }
    }

    // This method will be overridden by derived classes to update their specific power output calculations.
    protected virtual void UpdatePowerOutput()
    {
    }

    void Update()
    {
        UpdatePowerOutput();
    }
}
