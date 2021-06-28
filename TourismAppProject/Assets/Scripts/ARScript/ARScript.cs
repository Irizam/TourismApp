using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAcction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnMenuHome ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void goToMedia()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }

    public void goToValidation()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
}
