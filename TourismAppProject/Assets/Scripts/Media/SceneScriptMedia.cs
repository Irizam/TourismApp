using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneScriptMedia : MonoBehaviour
{
    public Button btnImage, btnVideo,btnReturn;
    // Start is called before the first frame update
    void Start()
    {

        btnImage.onClick.AddListener(SceneImage);
        btnVideo.onClick.AddListener(SceneVideo);
        btnReturn.onClick.AddListener(SceneMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SceneImage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(8);
    }
    public void SceneVideo()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);

    }
    public void SceneMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }
    
}
