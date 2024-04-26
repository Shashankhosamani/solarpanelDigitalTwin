using UnityEngine;

public class SolarPanelController : MonoBehaviour
{
    public Transform sun;  // Assign the sun light's transform in the inspector

    void Update()
    {
        if (sun != null)
        {
            // Calculate the direction from the panel to the sun
            Vector3 directionToSun = sun.position - transform.position;

            // Rotate the panel to face the sun directly
            transform.rotation = Quaternion.LookRotation(directionToSun);
        }
    }
}
