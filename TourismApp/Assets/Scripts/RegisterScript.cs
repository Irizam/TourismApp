using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour
{
    public Button registerButton, loginButton;
    // Start is called before the first frame update
    void Start()
    {

        loginButton.onClick.AddListener(loginButtonOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loginButtonOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
