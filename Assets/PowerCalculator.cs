using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PowerCalculator : MonoBehaviour
{
    public Text displayText;  // Assign this from the Inspector
    private List<WeatherData> weatherDataList;
    private int currentIndex = 0;
    private float updateInterval = 49f / 24f;  // Interval in seconds for each hour update

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "data.csv");
        if (File.Exists(filePath))
        {
            weatherDataList = LoadCSV(filePath);
            StartCoroutine(UpdatePowerData());
        }
        else
        {
            Debug.LogError("CSV file not found!");
        }
    }

    List<WeatherData> LoadCSV(string filePath)
    {
        List<WeatherData> data = new List<WeatherData>();
        using (StreamReader reader = new StreamReader(filePath))
        {
            bool isHeader = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }
                var values = line.Split(',');

                WeatherData wd = new WeatherData
                {
                    Time = values[0],
                    Temperature2m = float.Parse(values[1]),
                    RelativeHumidity2m = float.Parse(values[2]),
                    DewPoint2m = float.Parse(values[3]),
                    Rain = float.Parse(values[4]),
                    CloudCover = float.Parse(values[5]),
                    Visibility = float.Parse(values[6]),
                    DNI = float.Parse(values[7]),
                    GHI = float.Parse(values[8])
                };

                data.Add(wd);
            }
        }
        return data;
    }

    System.Collections.IEnumerator UpdatePowerData()
    {
        while (currentIndex < weatherDataList.Count)
        {
            WeatherData wd = weatherDataList[currentIndex];
            float power = CalculatePower(wd);
            Debug.Log(power);
            displayText.text = $"Time: {wd.Time}, Power: {power} W\n";  // Display only the current time's power

            currentIndex = (currentIndex + 1) % weatherDataList.Count;  // Loop back to start after finishing

            yield return new WaitForSeconds(updateInterval);  // Wait for the specified interval before updating again
        }
    }

    float CalculatePower(WeatherData wd)
    {
        // Replace with your actual power calculation formula
        return wd.GHI * 0.2f;  // Example formula
    }

    public class WeatherData
    {
        public string Time { get; set; }
        public float Temperature2m { get; set; }
        public float RelativeHumidity2m { get; set; }
        public float DewPoint2m { get; set; }
        public float Rain { get; set; }
        public float CloudCover { get; set; }
        public float Visibility { get; set; }
        public float DNI { get; set; }
        public float GHI { get; set; }
    }
}
