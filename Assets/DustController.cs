using UnityEngine;

public class DustController : MonoBehaviour
{
    public float dustThreshold = 0.8f; // Threshold at which cleaning starts
    public float cleaningSpeed = 0.1f; // Speed at which dust is cleaned
    public GameObject wiper; // Reference to the wiper GameObject
    private Animator wiperAnimator; // Animator component for the wiper
    private bool isCleaning = false; // Flag to check if cleaning is in progress

    void Start()
    {
        // Get the Animator component from the wiper GameObject
        if (wiper != null)
        {
            wiperAnimator = wiper.GetComponent<Animator>();
            if (wiperAnimator == null)
            {
                Debug.LogError("Animator component not found on wiper GameObject.");
            }
        }
        else
        {
            Debug.LogError("Wiper GameObject not assigned.");
        }

        // Ensure the wiper has a Rigidbody component
        Rigidbody rb = wiper.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = wiper.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object is a panel
        DustMaterial dustMaterial = other.GetComponent<DustMaterial>();
        if (dustMaterial != null && !isCleaning && dustMaterial.GetDustAmount() >= dustThreshold)
        {
            Debug.Log("Activated");
            // Start the cleaning process
            isCleaning = true;
            if (wiperAnimator != null)
            {
                wiperAnimator.SetTrigger("Activate");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Check if the object is a panel
        DustMaterial dustMaterial = other.GetComponent<DustMaterial>();
        if (dustMaterial != null && isCleaning)
        {
            // Gradually clean the dust
            dustMaterial.ReduceDust(Time.deltaTime * cleaningSpeed);

            // Stop cleaning if dust amount is sufficiently low
            if (dustMaterial.GetDustAmount() <= 0.0f)
            {
                Debug.Log("Deactivated");
                isCleaning = false;
                if (wiperAnimator != null)
                {
                    wiperAnimator.SetTrigger("Deactivate");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Ensure cleaning stops if the wiper exits the panel
        DustMaterial dustMaterial = other.GetComponent<DustMaterial>();
        if (dustMaterial != null && isCleaning)
        {
            Debug.Log("Deactivated");
            isCleaning = false;
            if (wiperAnimator != null)
            {
                wiperAnimator.SetTrigger("Deactivate");
            }
        }
    }
}
