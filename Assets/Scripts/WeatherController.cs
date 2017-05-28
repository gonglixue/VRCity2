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
    public GameObject DirectLight;

    public GameObject skyBox;
    public bool bLightning = false;
    private float lightningStart;
    private bool firstLight = true;

    #region snowMat
    public GameObject[] matHostGameObject;
    #endregion

    // Use this for initialization
    void Start () {
        ChooseWeather();
	}
	
	// Update is called once per frame
	void Update () {
        if (bLightning)  // 闪电模拟
        {
            if (firstLight)
            {
                lightningStart = Time.time;
                firstLight = false;
            }
            
            TurnOnLightning();
            SetLightIntensity(1.0f);
            if(Time.time > lightningStart + 0.3f)
            {
                firstLight = true;
                bLightning = false;
                TurnDownLightning();
                SetLightIntensity(0);
            }
                
        }
	
	}

    void InitSnowySystem()
    {
        SetLightIntensity(0);
        SkyBoxConfig snowSkyConf = new SkyBoxConfig(new Color(3f/255, 30f/255, 49f/255, 1), new Color(75f/255, 86f/255, 86f/255, 1), new Color(167f/255, 168f/255, 168f/255), 0.3f, 0.079f, 0.821f);
        SetSkyBoxAttrib(snowSkyConf);

        snowySystem.SetActive(true);  // 开启粒子系统

        // 设置材质
        foreach(GameObject tileObject in matHostGameObject)
        {
            Material m = tileObject.GetComponent<MeshRenderer>().sharedMaterial;
            m.SetFloat("_Snow", 0.003f);
            m.SetFloat("_SnowDepth", 0.032f);
        }
    }
    void InitRainySystem()
    {
        SetLightIntensity(0);
        SkyBoxConfig rainSkyConf = new SkyBoxConfig(new Color(3f/255, 30f/255, 49f/255, 1), new Color(75f/255, 86f/255, 86f/255, 1), new Color(167f/255, 168f/255, 168f/255), 0.3f, 0.079f, 0.821f);
        SetSkyBoxAttrib(rainSkyConf);
        rainySystem.SetActive(true);
        ClearSnowMat();

    }
    void InitSunnySystem()
    {
        SetLightIntensity(0.8f);
        CancelSnowMat();
        SkyBoxConfig sunnySkyConf = new SkyBoxConfig(new Color(20f/255, 124f/255, 197f/255, 1), new Color(181f/255, 216f/255, 246f/255, 1), new Color(255f/255, 255f/255, 255f/255), 0.3f, 0.0f, 1.0f);
        SetSkyBoxAttrib(sunnySkyConf);
        //sunnySystem.SetActive(true);
        ClearSnowMat();
    }
    void InitCloudySystem()
    {
        SetLightIntensity(0.54f);
        SkyBoxConfig cloudSkyConf = new SkyBoxConfig(new Color(20f/255, 124f/255, 197f/255, 1), new Color(181f/255, 216f/255, 246f/255, 1), new Color(191f/255, 191f/255, 191f/255), 0.3f, 0.0f, 0.32f);
        SetSkyBoxAttrib(cloudSkyConf);
        //cloudySystem.SetActive(true);
        ClearSnowMat();
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

    void CancelSnowMat()
    {
        foreach(GameObject tileObj in matHostGameObject)
        {
            Material snowMat = tileObj.GetComponent<MeshRenderer>().sharedMaterial;
            snowMat.SetFloat("_Snow", 0);
        }
    }

    void ActiveSnowMat()
    {
        foreach(GameObject tileObj in matHostGameObject)
        {
            Material snowMat = tileObj.GetComponent<MeshRenderer>().sharedMaterial;
            snowMat.SetFloat("_Snow", 0.003f);
        }
    }

    void ClearSnowMat()
    {
        foreach (GameObject tileObject in matHostGameObject)
        {
            Material m = tileObject.GetComponent<MeshRenderer>().sharedMaterial;
            m.SetFloat("_Snow", 0);
            m.SetFloat("_SnowDepth", 0);
        }
    }

    void SetLightIntensity(float light)
    {
        DirectLight.GetComponent<Light>().intensity = light;
    }

    void TurnOnLightning()
    {
        Material m = skyBox.GetComponent<MeshRenderer>().material;
        m.SetFloat("_Lightning", 1.0f);
    }
    void TurnDownLightning()
    {
        Material m = skyBox.GetComponent<MeshRenderer>().material;
        m.SetFloat("_Lightning", 0.0f);
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