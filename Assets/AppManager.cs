using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class AppManager : MonoBehaviour
{
    public float appRefresh;
    private float timer;
    public Text currentWeatherText, tempText, currentTimeText;
    public WeatherStates weatherController;

    // Public fields for latitude, longitude input, and submit button
    public InputField latitudeInputField;
    public InputField longitudeInputField;
    public Button submitButton;

    private float lat, lon;

    private void Start()
    {
        timer = appRefresh;

        // Add listener to the submit button
        submitButton.onClick.AddListener(OnSubmit);

        // Initialize latitude and longitude
        UpdateLatitudeLongitude();
    }

    public void OnSubmit()
    {
        UpdateLatitudeLongitude();
        StopCoroutine("GetWeather");
        StartCoroutine(GetWeather(lat, lon));
    }

    private void UpdateLatitudeLongitude()
    {
        if (float.TryParse(latitudeInputField.text, out float newLat))
        {
            lat = newLat;
        }
        else
        {
            Debug.LogError("Invalid latitude input");
        }

        if (float.TryParse(longitudeInputField.text, out float newLon))
        {
            lon = newLon;
        }
        else
        {
            Debug.LogError("Invalid longitude input");
        }
    }

    public IEnumerator GetWeather(float lat, float lon)
    {
        var weatherAPI = new UnityWebRequest($"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true&hourly=temperature_2m,cloud_cover,weathercode&timezone=auto")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return weatherAPI.SendWebRequest();

        if (weatherAPI.isNetworkError || weatherAPI.isHttpError)
        {
            print("Failed to get data");
            yield break;
        }

        JSONNode weatherInfo = JSON.Parse(weatherAPI.downloadHandler.text);

        // Get the UTC time from the API response
        string utcTimeString = weatherInfo["current_weather"]["time"];
        DateTime utcTime = DateTime.Parse(utcTimeString);

        // Convert UTC time to local time based on the UTC offset provided by the API
        int utcOffsetSeconds = weatherInfo["utc_offset_seconds"].AsInt;
        TimeSpan utcOffset = TimeSpan.FromSeconds(utcOffsetSeconds);
        DateTime localTime = utcTime;

        bool isDay = localTime.Hour >= 6 && localTime.Hour < 18;


        int weatherCode = weatherInfo["current_weather"]["weathercode"].AsInt;
        ApplyWeatherEffect(weatherCode, isDay);

        print(weatherInfo["current_weather"]["weathercode"]);
    }

    private string GetWeatherDescription(int weatherCode)
    {
        switch (weatherCode)
        {
            case 0: return "Clear sky";
            case 1: return "Mainly clear";
            case 2: return "Partly cloudy";
            case 3: return "Overcast";
            case 45: return "Fog";
            case 48: return "Depositing rime fog";
            case 51: return "Drizzle: Light";
            case 53: return "Drizzle: Moderate";
            case 55: return "Drizzle: Dense intensity";
            case 56: return "Freezing Drizzle: Light";
            case 57: return "Freezing Drizzle: Dense intensity";
            case 61: return "Rain: Slight";
            case 63: return "Rain: Moderate";
            case 65: return "Rain: Heavy intensity";
            case 66: return "Freezing Rain: Light";
            case 67: return "Freezing Rain: Heavy intensity";
            case 71: return "Snow fall: Slight";
            case 73: return "Snow fall: Moderate";
            case 75: return "Snow fall: Heavy intensity";
            case 77: return "Snow grains";
            case 80: return "Rain showers: Slight";
            case 81: return "Rain showers: Moderate";
            case 82: return "Rain showers: Violent";
            case 85: return "Snow showers slight";
            case 86: return "Snow showers heavy";
            case 95: return "Thunderstorm: Slight or moderate";
            case 96: return "Thunderstorm with slight hail";
            case 99: return "Thunderstorm with heavy hail";
            default: return "Unknown weather";
        }
    }

    private void ApplyWeatherEffect(int weatherCode, bool isDay)
    {
        switch (weatherCode)
        {
            case 0:
                if (isDay) weatherController.ClearDay();
                else weatherController.ClearNight();
                break;
            case 1:
            case 2:
            case 3:
                if (isDay) weatherController.CloudsDay();
                else weatherController.CloudsNight();
                break;
            case 45:
            case 48:
                if (isDay) weatherController.MistDay();
                else weatherController.MistNight();
                break;
            case 51:
            case 53:
            case 55:
            case 56:
            case 57:
            case 61:
            case 63:
            case 65:
            case 66:
            case 67:
            case 80:
            case 81:
            case 82:
                if (isDay) weatherController.RainDay();
                else weatherController.RainNight();
                break;
            case 71:
            case 73:
            case 75:
            case 77:
                if (isDay) weatherController.SnowDay();
                else weatherController.SnowNight();
                break;
            case 95:
            case 96:
            case 99:
                if (isDay) weatherController.RainDay();
                else weatherController.RainNight();
                break;
                // Add other weather conditions as needed
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            StopCoroutine("GetWeather");
            StartCoroutine(GetWeather(lat, lon));
            print("App Refresh");

            timer = appRefresh;
        }
    }
}
