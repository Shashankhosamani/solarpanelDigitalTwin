using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public float appRefresh;
    private float timer;
    public Text currentWeatherText, tempText, currentTimeText;
    public WeatherStates weatherController;

    // UI Elements
    public InputField latitudeInput;
    public InputField longitudeInput;
    public Button submitButton;

    private float lat;
    private float lon;

    public delegate void WeatherDataUpdated(float shortwaveRadiation, bool isDay);
    public event WeatherDataUpdated OnWeatherDataUpdated;

   void Start()
    {
        lat = 0f; // Default latitude
        lon = 0f; // Default longitude

        submitButton.onClick.AddListener(OnSubmit);

        timer = appRefresh;
    }

    public void OnSubmit()
    {
        if (float.TryParse(latitudeInput.text, out float parsedLat))
        {
            lat = parsedLat;
        }
        else
        {
            Debug.LogError("Invalid latitude input");
        }

        if (float.TryParse(longitudeInput.text, out float parsedLon))
        {
            lon = parsedLon;
        }
        else
        {
            Debug.LogError("Invalid longitude input");
        }

        StopCoroutine(nameof(GetWeather));
        StartCoroutine(GetWeather(lat, lon));
    }

    public IEnumerator GetWeather(float lat, float lon)
    {
        var weatherAPI = new UnityWebRequest($"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true&hourly=temperature_2m,cloud_cover,weathercode,shortwave_radiation&timezone=auto")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return weatherAPI.SendWebRequest();

        if (weatherAPI.isNetworkError || weatherAPI.isHttpError)
        {
            Debug.LogError("Failed to get data: " + weatherAPI.error);
            yield break;
        }

        JSONNode weatherInfo = JSON.Parse(weatherAPI.downloadHandler.text);
        Debug.Log(weatherInfo);

        // Extract the timezone from the API response
        string timeZoneId = weatherInfo["timezone"];
        if (string.IsNullOrEmpty(timeZoneId))
        {
            Debug.LogError("Time zone information is missing from the API response");
            yield break;
        }

        // Get the current UTC time string from the response
        string utcTimeString = weatherInfo["current_weather"]["time"];
        Debug.Log("UTC Time String: " + utcTimeString);

        // Parse the UTC time string
        DateTime utcTime;
        string utcFormat = "yyyy-MM-ddTHH:mm"; // Adjust the format if needed
        if (!DateTime.TryParseExact(utcTimeString, utcFormat, null, System.Globalization.DateTimeStyles.None, out utcTime))
        {
            Debug.LogError("Failed to parse UTC time");
            yield break;
        }

        // Convert UTC time to local time
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        Debug.Log("Local Time: " + localTime);

        bool isDay = localTime.Hour >= 6 && localTime.Hour < 18;

        currentWeatherText.text = "Current weather: " + GetWeatherDescription(weatherInfo["current_weather"]["weathercode"].AsInt);
        tempText.text = "Current temperature: " + Mathf.Floor(weatherInfo["current_weather"]["temperature"].AsFloat) + "°C";

        int weatherCode = weatherInfo["current_weather"]["weathercode"].AsInt;
        float shortwaveRadiation = weatherInfo["hourly"]["shortwave_radiation"][0].AsFloat;

        ApplyWeatherEffect(weatherCode, isDay);

        OnWeatherDataUpdated?.Invoke(shortwaveRadiation, isDay);

        Debug.Log(weatherInfo["current_weather"]["weathercode"]);
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
                if (isDay) weatherController.MistDay();
                else weatherController.MistNight();
                break;
            case 48:
                if (isDay) weatherController.MistDay();
                else weatherController.MistNight();
                break;
            case 51:
            case 53:
            case 55:
                if (isDay) weatherController.RainDay();
                else weatherController.RainNight();
                break;
            case 56:
            case 57:
                if (isDay) weatherController.RainDay();
                else weatherController.RainNight();
                break;
            case 61:
            case 63:
            case 65:
                if (isDay) weatherController.RainDay();
                else weatherController.RainNight();
                break;
            case 66:
            case 67:
                if (isDay) weatherController.RainDay();
                else weatherController.RainNight();
                break;
            case 80:
            case 81:
            case 82:
                if (isDay) weatherController.RainDay();
                else weatherController.RainNight();
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
        currentTimeText.text = "Current time: " + DateTime.Now.ToString("HH:mm:ss");
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            StartCoroutine(GetWeather(lat, lon));
            Debug.Log("App Refresh");
            timer = appRefresh;
        }
    }
}
