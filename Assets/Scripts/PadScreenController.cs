﻿using UnityEngine;
using System.Collections;

public class PadScreenController : MonoBehaviour {

    private string _latLon;
    private string _city;
    private string _country;

    public Transform LocationTransform;
    public Transform CityTransform;
    public Transform CountryTransform;

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

        // 设置显示文字 （TODO 文字显示特效）
        LocationTransform.GetComponent<TextMesh>().text = location;
        CityTransform.GetComponent<TextMesh>().text = cityName;
        CountryTransform.GetComponent<TextMesh>().text = countryName;
    }
}
