using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light sun;
    public float maxIntensity = 1.0f;
    public float cycleDuration = 1500.0f;  // Duration in seconds of a full cycle

    private float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;
        float fraction = (timer % cycleDuration) / cycleDuration;

        // Calculate current angle
        float angle = 360f * fraction;

        // Rotate sun around the y-axis at the origin
        ///sun.transform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));

        // Adjust intensity based on time of day
        // Using Mathf.Abs ensures the intensity decreases after reaching the peak at midday.
        sun.intensity = Mathf.Lerp(0, maxIntensity, Mathf.Sin(fraction * Mathf.PI));
    }
}
