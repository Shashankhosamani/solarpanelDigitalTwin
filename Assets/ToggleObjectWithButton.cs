using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectWithButton : MonoBehaviour
{
    public GameObject objectToToggle;

    public void ToggleObject()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
    }

}