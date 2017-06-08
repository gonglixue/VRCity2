using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PadScreenController : MonoBehaviour {

    private string _latLon;
    private string _city;
    private string _country;

    public Transform LocationTransform;
    public Transform CityTransform;
    public Transform CountryTransform;

    public GameObject TitleText;
    public GameObject ContentText;
    public GameObject InitialTip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayInfo(string location, string cityName, string countryName)
    {
        _latLon = location;
        _city = cityName;
        _country = countryName;

        // 显示ContentText，隐藏InitialTip
        TitleText.SetActive(true);
        ContentText.SetActive(true);
        InitialTip.SetActive(false);

        // 设置显示文字 （TODO 文字显示特效）
        //LocationTransform.GetComponent<TextMesh>().text = location;
        //CityTransform.GetComponent<TextMesh>().text = cityName;
        //CountryTransform.GetComponent<TextMesh>().text = countryName;

        // 使用Canvas显示文字
        LocationTransform.GetComponent<Text>().text = location;
        CityTransform.GetComponent<Text>().text = cityName;
        CountryTransform.GetComponent<Text>().text = countryName;
    }

    public void DisplayPin(string location)
    {
        string context = "You put a pin at\r\n" + 
            location
            + "\r\nPress the button under the text\r\n"
            + "And enter the city you choose.";
        TitleText.SetActive(false);
        ContentText.SetActive(false);
        InitialTip.SetActive(true);

        //InitialTip.transform.GetChild(0).GetComponent<TextMesh>().text = context;
        // 使用canvas显示文字
        InitialTip.GetComponent<Text>().text = context;
    }
}
