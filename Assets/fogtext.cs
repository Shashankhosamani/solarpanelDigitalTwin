using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fogtext : MonoBehaviour
{
    public Text textComponent; // Reference to the Text component
    private bool isOn = false; // A flag to track the toggle state

    // This method is called when the button is clicked
    public void ToggleTextOnOff()
    {
        isOn = !isOn; // Toggle the state
        textComponent.text = isOn ? "On" : "Off"; // Update the text based on the current state
    }
}
