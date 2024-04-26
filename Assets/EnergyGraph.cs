using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGraph : MonoBehaviour
{
    public RectTransform graphContainer;
    public GameObject pointPrefab;
    private List<GameObject> points = new List<GameObject>();
    private List<float> energyValues = new List<float>();

    // Call this method to add a new data point
    public void AddPoint(float energy)
    {
        energyValues.Add(energy);
        UpdateGraph();
    }

    private void UpdateGraph()
    {
        foreach (GameObject point in points)
        {
            Destroy(point);
        }
        points.Clear();

        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 1000f; // Maximum energy expected, adjust accordingly
        float xSize = 50; // Spacing between points

        for (int i = 0; i < energyValues.Count; i++)
        {
            float xPosition = i * xSize;
            float yPosition = (energyValues[i] / yMax) * graphHeight;
            GameObject newPoint = Instantiate(pointPrefab, graphContainer);
            newPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, yPosition);
            points.Add(newPoint);
        }
    }
}
