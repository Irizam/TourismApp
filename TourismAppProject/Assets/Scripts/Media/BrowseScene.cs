using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrowseScene : MonoBehaviour
{
    public string image;
    public string video;
    public string principal;
    public void LoadSceneImage()
    {
        SceneManager.LoadScene(image);

    }
    public void LoadSceneVideo()
    {
        SceneManager.LoadScene(video);
    }
    public void LoadScenePrincipal()
    {
        SceneManager.LoadScene(principal);
    }


}
