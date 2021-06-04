using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavegateScript : MonoBehaviour
{

    //public buttonScrip butonScrip;

    private string urlMap = "";

    public RawImage imgmap;

    public int zoom = 10;
    // Start is called before the first frame update

    public string latt, logg,latUser,logUser;
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
        latUser = latitud;

        string longitud = Input.location.lastData.longitude.ToString();
        logUser = longitud;
        Debug.Log("********* " + latUser + " --------User " + logUser);
        urlMap = "https://maps.geoapify.com/v1/staticmap?style=osm-carto&width=600&height=400&center=lonlat:-66.156397,-17.396189&zoom=" + zoom + "&marker=lonlat:-66.15696907766927,-17.39406057217893;type:awesome;color:red;size:x-large;icon:landmark|" +
            "lonlat:-66.13496497147685,-17.384519040992913;type:awesome;color:red;size:x-large;icon:landmark|lonlat:-66.15579063411896,-17.38824799127599;type:awesome;color:red;size:x-large;icon:landmark|lonlat:" + longitud + "," + latitud + ";type:awesome;color:%2319b8fc;size:large&apiKey=69ddfb1f86844681a87f152ead49a07e";
        WWW www = new WWW(urlMap);
        yield return www;

        imgmap.texture = www.texture;

    }

    public void GotoPlace()
    {
        string lt, lg,ltU,lgU;
        lt = latt;
        lg = logg;
        ltU = latUser;
        lgU = logUser;

        Debug.Log("********* " + lt + " --------Llegar "+lg);
        Debug.Log("********* " + ltU + " --------User " + lgU);
        Application.OpenURL("https://www.google.com.bo/maps/dir/"+ltU+","+lgU+"/"+ lt + ","+lg+"/@-17.3826693,-66.178493,13.64z/data=!4m5!4m4!1m1!4e1!1m0!3e0?hl=es");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
