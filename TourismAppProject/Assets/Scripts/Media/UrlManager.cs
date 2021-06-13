using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlManager : MonoBehaviour
{
    public string video1;
    public string video2;
    public string video3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void VideoUno()
    {
        Application.OpenURL(video1);
    }

    public void VideoDos()
    {
        Application.OpenURL(video2);
    }

    public void VideoTres()
    {
        Application.OpenURL(video3);
    }
   
}
