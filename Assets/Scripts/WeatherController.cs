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

    public GameObject skyBox;

	// Use this for initialization
	void Start () {
        ChooseWeather();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitSnowySystem()
    {
        SkyBoxConfig snowSkyConf = new SkyBoxConfig(new Color(3, 30, 49, 255), new Color(75, 86, 86, 255), new Color(167, 168, 168), 0.3f, 0.079f, 0.821f);
        SetSkyBoxAttrib(snowSkyConf);

        snowySystem.SetActive(true);  // 开启粒子系统
    }
    void InitRainySystem()
    {
        SkyBoxConfig rainSkyConf = new SkyBoxConfig(new Color(3f/255, 30f/255, 49f/255, 1), new Color(75f/255, 86f/255, 86f/255, 1), new Color(167f/255, 168f/255, 168f/255), 0.3f, 0.079f, 0.821f);
        SetSkyBoxAttrib(rainSkyConf);
        rainySystem.SetActive(true);
    }
    void InitSunnySystem()
    {
        SkyBoxConfig sunnySkyConf = new SkyBoxConfig(new Color(20f/255, 124f/255, 197f/255, 1), new Color(181f/255, 216f/255, 246f/255, 1), new Color(255f/255, 255f/255, 255f/255), 0.3f, 0.0f, 1.0f);
        SetSkyBoxAttrib(sunnySkyConf);
        sunnySystem.SetActive(true);
    }
    void InitCloudySystem()
    {
        SkyBoxConfig cloudSkyConf = new SkyBoxConfig(new Color(20f/255, 124f/255, 197f/255, 1), new Color(181f/255, 216f/255, 246f/255, 1), new Color(191f/255, 191f/255, 191f/255), 0.3f, 0.0f, 0.32f);
        SetSkyBoxAttrib(cloudSkyConf);
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

    // 根据天气设置天空盒参数
    void SetSkyBoxAttrib(SkyBoxConfig config)
    {
        Material m = skyBox.GetComponent<MeshRenderer>().material;
        m.SetColor("_SkyColor0", config.colorAbove);
        m.SetColor("_SkyColor1", config.colorBelow);
        m.SetColor("_CloudColor", config.cloudColor);
        m.SetFloat("_Speed", config.speed);
        m.SetFloat("_Emptiness", config.emptiness);
        m.SetFloat("_Sharpness", config.sharpness);

    }
}

public class SkyBoxConfig
{
    public Color colorAbove;
    public Color colorBelow;
    public Color cloudColor;
    public float speed;
    public float emptiness;
    public float sharpness;

    public SkyBoxConfig(Color _colorAbove, Color _colorBelow, Color _cloudColor, float _speed, float _emptiness, float _sharpness)
    {
        colorAbove = _colorAbove;
        colorBelow = _colorBelow;
        cloudColor = _cloudColor;
        speed = _speed;
        emptiness = _emptiness;
        sharpness = _sharpness;
    }
}