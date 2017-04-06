using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class NetworkService{

    //private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&mode=xml";
    private const string xmlApi = "http://127.0.0.1:8888/upload";
    private const string fileDownloadPath = "http://127.0.0.1:8000/3dwebclient/hello.html";
    private string file_saveUrl = @"D:\test";
    private const string loadKMLUrl = "http://127.0.0.1:8888/loadKML";
    private const string loadTileUrl = "http://127.0.0.1:8888/loadTile";
    private const string host = "http://127.0.0.1:8888";

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
            yield break;
        }
        if (www.isDone)
        {
            //string content = www.text;
            //byte[] fileBytes = System.Text.Encoding.Default.GetBytes(content);
            //CreateFile(fileBytes);
            Debug.Log("www.isdonw");
            callback(www.text);
        }
    }

    public IEnumerator RequestTileKML(string cityName, int idx, int idy, Action<string,int,int> callback)
    {
        string url = loadTileUrl + "?idx=" + idx + "&idy=" + idy;
        WWW www = new WWW(url);
        yield return www;

        if (!IsResponseValid(www))
            yield break;
        if (www.isDone)
        {
            callback(www.text,idx, idy);
        }
    }

    public IEnumerator DownloadBuilding(string href, Action callback)
    {
        string savePath = Application.dataPath + "/Resources/Download" + href;
        string dirPath = savePath.Substring(0, savePath.LastIndexOf('/'));
        Debug.Log(dirPath);
        System.IO.Directory.CreateDirectory(dirPath);  //各级目录必须存在，创建目录

        string url = host + href;
        Debug.Log("download building url:" + url);
        WWW www = new WWW(url);
        yield return www;

        if (!IsResponseValid(www))
            yield break;
        if (www.isDone)
        {
            byte[] fileBytes = www.bytes;
            CreateFile(fileBytes, savePath);
            callback();
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
}
