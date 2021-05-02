using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    public Button loginButton, registerButton;
    public InputField emailInput, passwordInput;
    public Text loginMessage;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(loginButtonOnClick);
        registerButton.onClick.AddListener(registerButtonOnClick);
        passwordInput.inputType = InputField.InputType.Password;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void loginButtonOnClick()
    {
        string email = emailInput.text, password = passwordInput.text;
        StartCoroutine(Login(email, password));
    }

    IEnumerator Login(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/login.php", form);
        yield return www;
        loginMessage.text = www.text;
    }

    void registerButtonOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}