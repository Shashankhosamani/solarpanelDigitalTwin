
using UnityEngine;

public class WeatherStates : MonoBehaviour
{
    public GameObject clouds, sun, rain,RainClouds,NightClouds,nightRainClouds;
    public Material ClearDaySky, ClearNightSky,CloudNightSkyBox,overcastSkyBox;
    public void ClearDay()
    {
        RenderSettings.skybox = ClearDaySky;
        clouds.SetActive(false);
        RainClouds.SetActive(false);
        rain.SetActive(false);
        sun.SetActive(true);
        RenderSettings.fog = false;
       
    }
    public void ClearNight()
    {
        RenderSettings.skybox = ClearNightSky;
        clouds.SetActive(false);
        rain.SetActive(false);
        nightRainClouds.SetActive(false);
        sun.SetActive(false);
        RenderSettings.fog = false;

    }
    public void RainNight()
    {
        RenderSettings.skybox = CloudNightSkyBox;
        clouds.SetActive(false);
        rain.SetActive(true);
        nightRainClouds.SetActive(true);
        sun.SetActive(false);
        RenderSettings.fog = false;
    }
    public void RainDay()
    {
        RenderSettings.skybox = overcastSkyBox;
        clouds.SetActive(false);
        rain.SetActive(true);
        nightRainClouds.SetActive(false);
        sun.SetActive(true);
        RenderSettings.fog = false;
    }
    public void CloudsDay()
    {
        RenderSettings.skybox = overcastSkyBox;
        clouds.SetActive(true);
        rain.SetActive(false);
        nightRainClouds.SetActive(false);
        sun.SetActive(true);
        RenderSettings.fog = false;
    }
    public void CloudsNight()
    {
        RenderSettings.skybox = CloudNightSkyBox;
        clouds.SetActive(false);
        rain.SetActive(false);
        nightRainClouds.SetActive(false);
        sun.SetActive(false);
        RenderSettings.fog = false;
    }
   
    
    public void MistDay()
    {
        RenderSettings.skybox = ClearDaySky;
        clouds.SetActive(false);
        rain.SetActive(false);
        nightRainClouds.SetActive(false);
        sun.SetActive(true);
        RenderSettings.fog = true;
    }
    public void MistNight()
    {
        RenderSettings.skybox = ClearNightSky;
        clouds.SetActive(false);
        rain.SetActive(false);
        nightRainClouds.SetActive(false);
        sun.SetActive(false);
        RenderSettings.fog = true;
    }
   

    public void CloudCover()
    {
       //
    }
}
