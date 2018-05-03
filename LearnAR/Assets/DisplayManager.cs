﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

using Newtonsoft.Json;

public class DisplayManager : MonoBehaviour
{

    [SerializeField]
    private RawImage img = null;
    [SerializeField]
    private TextMesh t;
    [SerializeField]
    private AnimateCurrentManager am = null;

    Dictionary<string, float> components = new Dictionary<string, float>();

    private float lastCalled = 0;
    private float debounce = 0.5f;

    private string host = "http://192.168.137.54:5000";

    private string post_uri = "/circuit/image";
    private string get_image_uri = "/circuit/image/hololens";
    private string get_points_uri = "/circuit/evaluate";

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SendCircuitImage(string rawImage)
    {

        StringBuilder str = new StringBuilder();
        Dictionary<string, string> jsonObj = new Dictionary<string, string>();
        //jsonObj.Add("image", rawImage.ToString());
        jsonObj.Add("image", rawImage);
        string jsonStr = JsonConvert.SerializeObject(jsonObj);

        str.Append(jsonStr);

        var req = new UnityWebRequest(host + post_uri, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStr);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if(!req.isNetworkError)
        {

            str.Append(req.responseCode);
            str.Append(req.downloadHandler.text);
            //t.text = req.downloadHandler.text;
            if (req.downloadHandler.text == "")
            {
                str.Append("it's empty");
            }
            t.text = str.ToString();
        }
        else
        {
            t.text = "there was an error!";
        }
    }

    IEnumerator evaluateCircuitConnections()
    {
        StringBuilder str = new StringBuilder();
        using (UnityWebRequest www = UnityWebRequest.Get(host + get_points_uri))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                str.Append(www.error);
            }
            else
            {
                // Show results as text
                string result = www.downloadHandler.text;
                str.Append(result);

                Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                float xmin = System.Convert.ToSingle(res["xmin"]);
                float xmax = System.Convert.ToSingle(res["xmax"]);
                float ymin = System.Convert.ToSingle(res["ymin"]);
                float ymax = System.Convert.ToSingle(res["ymax"]);

                AnimateFlow(xmin, xmax, ymin, ymax);
                
            }
        }
    }

    IEnumerator GetImage()
    {
        StringBuilder str = new StringBuilder();
        Texture2D temp = new Texture2D(672, 504);
        using (UnityWebRequest www = UnityWebRequest.Get(host + get_image_uri))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                str.Append(www.error);
            }
            else
            {
                // Show results as text
                string result = www.downloadHandler.text;
                str.Append(result);
      
                Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                byte[] image = Convert.FromBase64String(res["image"]);
                str.Append(res["image"]);
                t.text = str.ToString();
                temp.LoadImage(image);
                img.texture = temp;
                //temp.LoadRawTextureData(image);
                //img.texture = temp; 
            }
        }
        //t.text = str.ToString();
    }

    public void GetCircuitImage()
    {
        if ((Time.time - lastCalled) < debounce)
        {
            return;
        }
        lastCalled = Time.time;
        StartCoroutine(GetImage());
    }

    public void EvaluateCircuit()
    {
        if ((Time.time - lastCalled) < debounce)
        {
            return;
        }
        lastCalled = Time.time;
        StartCoroutine(evaluateCircuitConnections());
    }

    public void DisplayCircuit(Texture2D targetTexture)
    {
        img.texture = targetTexture;

        // not sure which to send yet ... raw bytes or base64 encoded string
        byte [] imgData = targetTexture.GetRawTextureData();
        string encodedImg = System.Convert.ToBase64String(targetTexture.EncodeToJPG());

        // send circuit image
        StartCoroutine(SendCircuitImage(encodedImg));
    }

    private void DisplayValues()
    {
        StringBuilder str = new StringBuilder();
        foreach (KeyValuePair<string, float> entry in components)
        {
            str.Append(entry.Key).Append(" = ").Append(entry.Value).Append('\n');
        }

        t.text = str.ToString();
    }

    public void SetVoltageFive()
    {
        if (components.ContainsKey("V"))
        {
            components["V"] = 5.0f;
        }
        else
        {
            components.Add("V", 5.0f);
        }
        // update values
        DisplayValues();
    }

    public void SetR1oneK()
    {
        if (components.ContainsKey("R1"))
        {
            components["R1"] = 1000.0f;
        }
        else
        {
            components.Add("R1", 1000.0f);
        }
        // update values
        DisplayValues();
    }

    public void AnimateFlow(float xmin, float xmax, float ymin, float ymax)
    {
        am.ShowCurrentFlow(xmin, xmax, ymin, ymax);
    }
}