//-----------------------------------------------------------------------
// <copyright file="HTTPRequest.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Unity
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;

    internal sealed class HTTPRequest : IAsyncRequest
    {
        private readonly UnityWebRequest request;
        private readonly Action<Response> callback;

        public HTTPRequest(MonoBehaviour behaviour, string url, Action<Response> callback)
        {
            this.request = UnityWebRequest.Get(url);
            //Debug.Log("url:" + url);
            this.callback = callback;

            behaviour.StartCoroutine(this.DoRequest());
        }

        public void Cancel()
        {
            Debug.Log("http request.cs: cancel()");
            this.request.Abort();
        }

        private IEnumerator DoRequest()
        {
            //Debug.Log("DoRequest");
            
            //yield return this.request.Send();
            /*
            var response = new Response();
            response.Error = this.request.error;
            Debug.Log("error:"+response.Error);
            response.Data = this.request.downloadHandler.data;
            this.callback(response);*/

            
            WWW www = new WWW(request.url);
            yield return www;

            if(www.error != null)
            {
                Debug.Log("ERROR: " + www.error);
                yield break;
            }
            if(www.isDone)
            {
                //Debug.Log("www is donw");
                var response = new Response();
                response.Error = www.error;
                response.Data = www.bytes;
                this.callback(response);
            }
            


        }
    }
}
