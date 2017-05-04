using UnityEngine;
using System.Collections;
using System;
using System.IO;
using LitJson;

public class NetworkService{

    //private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&mode=xml";
    private const string xmlApi = "http://127.0.0.1:8888/upload";
    private const string fileDownloadPath = "http://127.0.0.1:8000/3dwebclient/hello.html";
    private string file_saveUrl = @"D:\temp";
    private const string loadKMLUrl = "http://127.0.0.1:8888/loadKML";
    private const string loadTileUrl = "http://127.0.0.1:8888/loadTileKML";
    private const string host = "http://127.0.0.1:8888";
    private const string loadTextureUrl = "http://127.0.0.1:8888/texture";
    private const string downloadFilePath = @"D:/temp/";

    private const string classDataUrl = "http://127.0.0.1:8888/classData";

    

    private bool IsResponseValid(WWW www)
    {
        if(www.error != null)
        {
            Debug.Log("bad connection");
            Debug.Log(www.error);
            return false;
        }
        else if(string.IsNullOrEmpty(www.text))
        {
            Debug.Log("bad data");
            return false;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        WWW www = new WWW(url);
        yield return www;

        if(!IsResponseValid(www))
        {
            yield break;
        }

        callback(www.text);
    }

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        Debug.Log("get wheather xml");
        return CallAPI(xmlApi, callback);
    }

    public IEnumerator DownFile()
    {
        WWW www = new WWW(fileDownloadPath);
        yield return www;

        if (!IsResponseValid(www))
        {
            yield break;
        }
        if (www.isDone)
        {
            Debug.Log("下载完成");
            byte[] fileBytes = www.bytes;
            CreateFile(fileBytes);
        }
    }

    public IEnumerator DownloadFile(string args, string savePath)
    {
        string url = fileDownloadPath + args;
        WWW www = new WWW(url);
        yield return www;

        if (!IsResponseValid(www))
        {
            yield break;
        }
        if (www.isDone)
        {
            Debug.Log("download " + savePath);
            byte[] fileBytes = www.bytes;
            CreateFile(fileBytes, savePath);
        }
    }

    public IEnumerator RequestKML(string args, Action<string> callback)
    {
        string url = loadKMLUrl + args;
        WWW www = new WWW(url);
        yield return www;

        if (!IsResponseValid(www))
        {
            Debug.Log("request KML error");
            yield break;
        }
        if (www.isDone)
        {
            //string content = www.text;
            //byte[] fileBytes = System.Text.Encoding.Default.GetBytes(content);
            //CreateFile(fileBytes);
            //Debug.Log("request kml isdown");
            callback(www.text);
        }
    }

    public IEnumerator RequestTileKML(string cityName, int idx, int idy, Action<string,int,int> callback)
    {
        string url = loadTileUrl + "?idx=" + idx + "&idy=" + idy;

        // 适当延时，避免too many open connection
        System.Random ran = new System.Random();
        int ranNum = ran.Next(50, 150);
        float delay = (float)ranNum / 100.0f;

        yield return new WaitForSeconds(delay);

        WWW www = new WWW(url);
        yield return www;

        if (!IsResponseValid(www))
        {
            Debug.Log("request tile KML error");
            yield break;
        }
            
        if (www.isDone)
        {
            Debug.Log("request tile kml is down");
            callback(www.text,idx, idy);
        }
    }

    public IEnumerator DownloadBuilding(string href, Action<string,string> callback)
    {
        string savePath = downloadFilePath + "/Resources/Download" + href;
        string dirPath = savePath.Substring(0, savePath.LastIndexOf('/'));
        Debug.Log(dirPath);
        System.IO.Directory.CreateDirectory(dirPath);  //各级目录必须存在，创建目录

        string url = host + href;
        //Debug.Log("download building url:" + url);
        WWW www = new WWW(url);
        yield return www;

        if (!IsResponseValid(www))
        {
            Debug.Log("DownloadBuilding response is not valid");
            yield break;
        }
            
        if (www.isDone)
        {
            byte[] fileBytes = www.bytes;
            CreateFile(fileBytes, savePath);
            callback(href,www.text);
        }
    }

    public IEnumerator DownloadTexture(string href, Action<string> callback)
    {
        string savePath = downloadFilePath + "/Resources/Download" + href;

        string url = loadTextureUrl + href;
        Debug.Log("download texture url:" + url);
        WWW www = new WWW(url);
        yield return www;

        if (!IsResponseValid(www))
        {
            Debug.Log("DownloadTexture response is not valud");
            yield break;
        }
        if (www.isDone)
        {
            byte[] fileBytes = www.bytes;
            CreateFile(fileBytes, savePath);
            callback(href);
        }
    }

    private void CreateFile(byte[] fileBytes)
    {
        FileInfo file = new FileInfo(file_saveUrl);
        Stream stream = file.Create();
        stream.Write(fileBytes, 0, fileBytes.Length);
        stream.Close();
        stream.Dispose();
        
    }

    private void CreateFile(byte[] fileBytes, string savePath)
    {
        FileInfo file = new FileInfo(savePath);
        Stream stream = file.Create();
        stream.Write(fileBytes, 0, fileBytes.Length);
        stream.Close();
        stream.Dispose();

    }

    public IEnumerator RequestClassData(string args, GameObject monthDotEntity, Action<GameObject, float[]> callback)
    {
        string url = classDataUrl + args;
        WWW www = new WWW(url);
        yield return www;

        if(!IsResponseValid(www))
        {
            Debug.Log("request class data error");
            yield break;
        }
        if(www.isDone)
        {
            // 解析JSON数据
            JsonData classJson = new JsonData();
            classJson = JsonMapper.ToObject(www.text);
            float[] classDataList = new float[4];
            classDataList[0] = int.Parse(classJson["sunnydays"].ToString());
            classDataList[1] = int.Parse(classJson["rainydays"].ToString());
            classDataList[2] = int.Parse(classJson["cloudydays"].ToString());
            classDataList[3] = int.Parse(classJson["snowydays"].ToString());

            callback(monthDotEntity, classDataList); //设置柱状图数据
        }
    }
}
