using UnityEngine;
using System.Collections;

public class WeatherController : MonoBehaviour {
    public enum WeatherMode {
        Sunny = 0,
        Rainy = 1,
        Snowy = 2,
        Cloudy = 3
    }

    public WeatherMode currentWeather;
    public GameObject rainySystem;
    public GameObject snowySystem;
    public GameObject sunnySystem;
    public GameObject cloudySystem;

	// Use this for initialization
	void Start () {
        ChooseWeather();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitSnowySystem()
    {
        snowySystem.SetActive(true);  // 开启粒子系统
    }
    void InitRainySystem()
    {
        rainySystem.SetActive(true);
    }
    void InitSunnySystem()
    {
        sunnySystem.SetActive(true);
    }
    void InitCloudySystem()
    {
        cloudySystem.SetActive(true);
    }

    void ChooseWeather()
    {
        switch(currentWeather)
        {
            case WeatherMode.Cloudy: InitCloudySystem(); break;
            case WeatherMode.Rainy: InitRainySystem(); break;
            case WeatherMode.Snowy: InitSnowySystem(); break;
            case WeatherMode.Sunny: InitSunnySystem(); break;
        }
    }
}
