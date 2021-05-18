using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavegateScript : MonoBehaviour
{
    private string urlMap = "";

    public RawImage imgmap;


    public int zoom = 10;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GetMap");
    }

    public void ZoomMasButton()
    {
        zoom++;
        StartCoroutine("GetMap");
    }

    public void ZoomMenosButton()
    {
        if (zoom >= 0)
        {
            zoom--;
        }
        StartCoroutine("GetMap");
    }

    private IEnumerator GetMap()
    {
        Input.location.Start();

        string latitud = Input.location.lastData.latitude.ToString();

        string longitud = Input.location.lastData.longitude.ToString();

        urlMap = "https://maps.geoapify.com/v1/staticmap?style=osm-carto&width=600&height=400&center=lonlat:-66.156397,-17.396189&zoom=" + zoom + "&marker=lonlat:-66.15696907766927,-17.39406057217893;type:awesome;color:%23bb3f73;size:x-large;icon:landmark|lonlat:" + longitud + "," + latitud + ";type:awesome;color:%2319b8fc;size:large&apiKey=69ddfb1f86844681a87f152ead49a07e";
        WWW www = new WWW(urlMap);
        yield return www;

        imgmap.texture = www.texture;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
