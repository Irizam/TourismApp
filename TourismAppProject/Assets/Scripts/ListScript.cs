using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ListScript : MonoBehaviour
{
    
    List<string> placesStringList = new List<string>();
    int total;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Places());
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                Application.Quit();
            }
        }
    }
    
    IEnumerator Places()
    {
        WWW www = new WWW("https://tourismappar.000webhostapp.com/spotID.php");
        yield return www;
        GameObject buttonTemplate = transform.GetChild(0).gameObject;
        GameObject g;
        string idString;
        int quantity = int.Parse(www.text);
        for (int i = 1; i <= quantity; i++)
        {
            g = Instantiate(buttonTemplate, transform);
            WWWForm form = new WWWForm();
            form.AddField("id", i);
            WWW www1 = new WWW("https://tourismappar.000webhostapp.com/spots.php", form);
            yield return www1;
            idString = www1.text;
            Debug.Log(www1.text);
            g.transform.GetChild(0).GetComponent<Text>().text =idString;
        }

        Destroy(buttonTemplate);
    }
   

   

   
}
