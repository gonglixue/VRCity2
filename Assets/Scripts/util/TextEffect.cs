using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour {
    public string fullText = "";

    public bool test = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(test)
        {
            TypeString("building name");
            test = false;
        }
	
	}

    public void TypeString(string text)
    {
        StartCoroutine(SetText(text));
    }

    private IEnumerator SetText(string text)
    {
        for(int i=1;i<=text.Length;i++)
        {
            string temp = text.Substring(0, i);
            this.GetComponent<Text>().text = temp;
            yield return new WaitForSeconds(0.2f);
        }
        
    }
}
