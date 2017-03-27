using UnityEngine;
using System.Collections;

public class wwwTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetworkService service = new NetworkService();
        Startup(service);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Startup(NetworkService service)
    {
        StartCoroutine(service.GetWeatherXML(OnXMLDataLoaded));
    }

    public void OnXMLDataLoaded(string data)
    {
        Debug.Log(data);
        
    }
}
