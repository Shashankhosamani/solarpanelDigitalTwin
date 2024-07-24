using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class IPGrabber : MonoBehaviour
{
    public string ipAddress;
    public float iplat, iplong;
    public AppManager appManager;

    void Start()
    {
        StartCoroutine(GrabIP());
    }

    IEnumerator GrabIP()
    {
        var response = new UnityWebRequest("https://api.ipify.org?format=text")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return response.SendWebRequest();

        if (response.isNetworkError || response.isHttpError)
        {
            print("Failed to get IP");
            yield break;
        }

        ipAddress = response.downloadHandler.text;
        StartCoroutine(IPLocation());
    }

    IEnumerator IPLocation()
    {
        var response = new UnityWebRequest($"https://geo.ipify.org/api/v1?apiKey=at_FGzPAT0cEOBoBAdYtQh6rLF13POfE&ipAddress={ipAddress}")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return response.SendWebRequest();

        if (response.isNetworkError || response.isHttpError)
        {
            print("Failed to get location");
            yield break;
        }

        JSONNode IPlocation = JSON.Parse(response.downloadHandler.text);
        iplat = IPlocation["location"]["lat"].AsFloat;
        iplong = IPlocation["location"]["lng"].AsFloat;

        if (appManager != null)
        {
            appManager.StartCoroutine(appManager.GetWeather(iplat, iplong));
        }
    }

    public IEnumerator GetLocationAndWeather()
    {
        yield return GrabIP();
    }
}
