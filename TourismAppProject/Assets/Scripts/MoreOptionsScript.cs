using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreOptionsScript : MonoBehaviour
{
    public RectTransform subMenu;
    public RectTransform subMenuPlaceOptions;

    float posFinal;
    bool openMenu = true;
    public float time = 0.5f;
    // Start is called before the first frame update

    private string urlMap = "";
    public int zoom = 10;
    public RawImage imgmap;
    public string latitudePlace = "", longitudePlace = "";
    void Start()
    {
        posFinal = Screen.width / 2;
        subMenu.position = new Vector3(-posFinal, subMenu.position.y, 0);
    }
    IEnumerator Move(float time, Vector3 posInit, Vector3 posFin)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            subMenu.position = Vector3.Lerp(posInit, posFin, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        subMenu.position = posFin;
    }
    void moveMenu(float time, Vector3 posInit, Vector3 posFin)
    {
        StartCoroutine(Move(time, posInit, posFin));
    }

    public void BUTTON_Sub_Menu()
    {
        int sign = 1;
        if (!openMenu)
            sign = -1;

        moveMenu(time, subMenu.position, new Vector3(sign * posFinal, subMenu.position.y, 0));
        openMenu = !openMenu;

    }

    public void BUTTON_Login()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void BUTTON_AR()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(11);
    }

    //ubication from place 

    IEnumerator PlaceUbication(int val)
    {      

        string lat, lon;
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

        StartCoroutine(GetMapUbi(lat, lon));
        subMenuPlaceOptions.gameObject.SetActive(true);
        
    }

    IEnumerator GetMapUbi(string lat, string log)
    {
       
        Input.location.Start();
        urlMap = "https://maps.geoapify.com/v1/staticmap?style=osm-carto&width=600&height=400&center=lonlat:"+ log +"," + lat +"&zoom=16.8483&marker=lonlat:" + log + "," + lat + ";type:awesome;color:red;size:x-large;icon:landmark&apiKey=69ddfb1f86844681a87f152ead49a07e";
     
        WWW www = new WWW(urlMap);
        yield return www;
        imgmap.texture = www.texture;

        int sign = 1;
        if (!openMenu)
            sign = -1;

        moveMenu(time, subMenu.position, new Vector3(sign * posFinal, subMenu.position.y, 0));
        openMenu = !openMenu;
    }

}
