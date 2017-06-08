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
    [SerializeField]
    private float lightningDuration = 0.3f;  // 每次闪电持续时间

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
                lightningStart = Time.time; //开启闪电的时间点
                firstLight = false;
            }
            
            TurnOnLightning();          // 开启打开shader中的闪电贴图控制，天空背景为第一张闪电贴图
            SetLightIntensity(1.0f);    // 照亮场景
            if(Time.time >= lightningStart + lightningDuration)   // 闪电持续0.3s后，切换闪电图片
            {
                if(Time.time < lightningStart + lightningDuration*2)  // 第二张闪电贴图
                {
                    ChangeShaderLightning(2.0f);
                    SetLightIntensity(0.6f);
                }
                else if(Time.time >= lightningStart + lightningDuration*2 && Time.time < lightningStart + lightningDuration*3)  // 第三章闪电贴图
                {
                    ChangeShaderLightning(3.0f);
                    SetLightIntensity(1.0f);
                }
                else
                {
                    firstLight = true;
                    //bLightning = false;   // 关闭闪电模拟
                    //ChangeShaderLightning(0);   // 取消shader中的闪电模拟
                    //SetLightIntensity(0);       // 恢复场景亮度
                }

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
    void ChangeShaderLightning(float shader_lightning)
    {
        Material m = skyBox.GetComponent<MeshRenderer>().material;
        m.SetFloat("_Lightning", shader_lightning);
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