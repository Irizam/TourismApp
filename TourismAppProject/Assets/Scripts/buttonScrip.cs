using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buttonScrip : MonoBehaviour
{
    //inacia de sripc para metodos    
    public NavegateScript navegationScrip;
    public MoreOptionsScript moreOption;
    //Start is called before the first frame update

    public RectTransform subMenuPlaceOptions;
    private string urlMap = "";
    public int zoom = 10;
    public RawImage imgmap;
    public string latitudePlace = "", longitudePlace = "";
    string idString;

    public string lat, lon;
    void Start()
    {
        //latt = "h";
        //lonn = "m";


    }

    // Update is called once per frame

    public void enviarID()
    {
        string id = this.gameObject.name;
        Debug.Log("--- " + id + " ---");
        StartCoroutine(PlaceUbication(id));
        navegationScrip.latt = lat;
        navegationScrip.logg = lon;
        PlayerPrefs.SetString("SpotID", id);
    }

    IEnumerator PlaceUbication(string val)
    {
       // string lat, lon;

        WWWForm formLat = new WWWForm();
        WWWForm formLon = new WWWForm();
        formLat.AddField("id", val);
        formLon.AddField("id", val);
        WWW www1 = new WWW("https://tourismappar.000webhostapp.com/spotsLatitude.php", formLat);
        WWW www2 = new WWW("https://tourismappar.000webhostapp.com/spotsLongitude.php", formLon);
        yield return www1;
        lat = www1.text;
        Debug.Log(www1.text);
        yield return www2;
        lon = www2.text;
        Debug.Log(www2.text);

        navegationScrip.latt = lat;
        navegationScrip.logg = lon;

        moreOption.BUTTON_Sub_Menu();        
        StartCoroutine(GetMapUbi(lat, lon));
        subMenuPlaceOptions.gameObject.SetActive(true);

    }

    IEnumerator GetMapUbi(string lat, string log)
    {
        Debug.Log("********* " + lat + " -------- " + log);
        Input.location.Start();
        urlMap = "https://maps.geoapify.com/v1/staticmap?style=osm-carto&width=600&height=400&center=lonlat:" + log + "," + lat + "&zoom=16.8483&marker=lonlat:" + log + "," + lat + ";type:awesome;color:red;size:x-large;icon:landmark&apiKey=69ddfb1f86844681a87f152ead49a07e";
        //urlMap = "https://maps.geoapify.com/v1/staticmap?style=osm-carto&width=600&height=400&center=lonlat:-66.156397,-17.396189&zoom=10.7479&marker=lonlat:-66.15696907766927,-17.39406057217893;type:awesome;color:%23bb3f73;size:x-large;icon:paw&apiKey=69ddfb1f86844681a87f152ead49a07e";
        WWW www = new WWW(urlMap);
        yield return www;
        imgmap.texture = www.texture;       
     
    }

    



}
