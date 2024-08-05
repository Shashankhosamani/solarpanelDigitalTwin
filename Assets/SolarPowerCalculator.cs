using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class SolarPowerCalculator : MonoBehaviour
{
    public Text solarPowerText;
    public InputField latitudeInput;
    public InputField longitudeInput;
    public Button submitButton;
    public SolarPanel[] solarPanels;

    private float latitude;
    private float longitude;

    void Start()
    {
        // Initialize latitude and longitude
        latitude = 0f;
        longitude = 0f;

        // Add listener to submit button
        submitButton.onClick.AddListener(OnSubmit);

        // Optionally, you can also add listeners to input fields if you want to handle changes directly
        // latitudeInput.onEndEdit.AddListener(OnLatitudeChanged);
        // longitudeInput.onEndEdit.AddListener(OnLongitudeChanged);
    }

    public void OnSubmit()
    {
        if (float.TryParse(latitudeInput.text, out float lat))
        {
            latitude = lat;
        }
        else
        {
            Debug.LogError("Invalid latitude input");
            return;
        }

        if (float.TryParse(longitudeInput.text, out float lon))
        {
            longitude = lon;
        }
        else
        {
            Debug.LogError("Invalid longitude input");
            return;
        }

        // Start updating solar power data with the new coordinates
        StartCoroutine(UpdateSolarPowerData());
    }

    IEnumerator UpdateSolarPowerData()
    {
        while (true)
        {
            yield return StartCoroutine(GetSolarPowerData(latitude, longitude));
            yield return new WaitForSeconds(3600); // Wait for 1 hour (3600 seconds)
        }
    }

    public IEnumerator GetSolarPowerData(float lat, float lon)
    {
        Debug.Log($"Latitude: {lat}, Longitude: {lon}");

        // URL to fetch weather data from the API
        var weatherAPI = new UnityWebRequest($"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true&hourly=temperature_2m,relative_humidity_2m,rain,weathercode,cloud_cover,visibility,shortwave_radiation&timezone=auto")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        // Send the request and wait for a response
        yield return weatherAPI.SendWebRequest();

        // Check for network or HTTP errors
        if (weatherAPI.isNetworkError || weatherAPI.isHttpError)
        {
            Debug.LogError("Failed to get data: " + weatherAPI.error);
            yield break;
        }

        // Parse the JSON response
        JSONNode weatherInfo = JSON.Parse(weatherAPI.downloadHandler.text);
        Debug.Log(weatherInfo);

        // Get the current local time string from the response
        string localTimeString = weatherInfo["current_weather"]["time"];
        Debug.Log("Local Time String: " + localTimeString); // Debug log

        // Attempt to parse the local time string
        DateTime localTime;
        string format = "yyyy-MM-ddTHH:mm:ss"; // Adjust the format if needed
        if (!DateTime.TryParseExact(localTimeString, format, null, System.Globalization.DateTimeStyles.None, out localTime))
        {
            Debug.LogError("Failed to parse local time");
            yield break;
        }

        // Determine the current hour and date
        int currentHour = localTime.Hour;
        string currentDate = localTime.ToString("yyyy-MM-dd");
        Debug.Log($"Current Hour: {currentHour}, Current Date: {currentDate}");

        // Access the hourly data
        JSONNode hourlyData = weatherInfo["hourly"];
        JSONArray times = hourlyData["time"].AsArray;
        JSONArray shortwaveRadiation = hourlyData["shortwave_radiation"].AsArray;

        // Iterate through hourly data to find the matching date and hour
        float shortwaveRadiationValue = 0f;
        bool dataFound = false;
        for (int i = 0; i < times.Count; i++)
        {
            DateTime hourlyTime;
            if (DateTime.TryParse(times[i], out hourlyTime))
            {
                if (hourlyTime.ToString("yyyy-MM-dd") == currentDate && hourlyTime.Hour == currentHour)
                {
                    shortwaveRadiationValue = shortwaveRadiation[i].AsFloat;
                    dataFound = true;
                    break;
                }
            }
        }

        if (!dataFound)
        {
            Debug.LogError("No data available for the current hour and date.");
            yield break;
        }

        // Calculate the total power generated
        float totalPowerGenerated = CalculateTotalPower(shortwaveRadiationValue);
        solarPowerText.text = "Total Solar Power Generation: " + totalPowerGenerated + " W";

        // Log the results
        Debug.Log("Global Horizontal Irradiance (GHI) at hour " + currentHour + ": " + shortwaveRadiationValue + " W/m²");
        Debug.Log("Total Solar Power Generation: " + totalPowerGenerated + " W");
    }


    float CalculateTotalPower(float shortwaveRadiation)
    {
        float totalPower = 0f;
        for (int i = 0; i < solarPanels.Length; i++)
        {
            float panelPower = solarPanels[i].CalculatePower(shortwaveRadiation);
            Debug.Log($"Power generated by panel {i + 1}: {panelPower} W");
            totalPower += panelPower;
        }
        return totalPower;
    }

    private void OnLatitudeChanged(string value)
    {
        if (float.TryParse(value, out float lat))
        {
            latitude = lat;
        }
        else
        {
            Debug.LogError("Invalid latitude input");
        }
    }

    private void OnLongitudeChanged(string value)
    {
        if (float.TryParse(value, out float lon))
        {
            longitude = lon;
        }
        else
        {
            Debug.LogError("Invalid longitude input");
        }
    }
}
