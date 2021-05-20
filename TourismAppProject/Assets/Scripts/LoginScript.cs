using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    public GameObject loginWindow, userWindow;
    public Button loginButton, registerButton, goBackToMainScreenButton, logoutButton;
    public InputField emailInput, passwordInput;
    public Text loginMessage, errorEmailMessage, errorPasswordMessage, tokenMessage;
    private IEnumerator showToastCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(LoginButtonOnClick);
        registerButton.onClick.AddListener(RegisterButtonOnClick);
        goBackToMainScreenButton.onClick.AddListener(GoBackToMainScreenButtonOnClick);
        passwordInput.inputType = InputField.InputType.Password;

        logoutButton.onClick.AddListener(LogoutButtonOnClick);

        if (PlayerPrefs.GetString("LoginToken") != "") {
            loginWindow.SetActive(false);
            userWindow.SetActive(true);
            tokenMessage.text = PlayerPrefs.GetString("LoginToken");
        }
        else
        {
            loginWindow.SetActive(true);
            userWindow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    void LoginButtonOnClick()
    {
        errorEmailMessage.text = "";
        errorPasswordMessage.text = "";
        String expresion;
        expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        bool flag = true;
        string email = emailInput.text, password = passwordInput.text;
        if (email=="")
        {
            errorEmailMessage.text = "El correo es requerido";
            flag = false;
        }
        if (!Regex.IsMatch(email,expresion))
        {
            errorEmailMessage.text = "Ingrese un correo valido";
            flag = false;
        }
        if (password=="")
        {
            errorPasswordMessage.text = "La contraseña es requerida";
            flag = false;
        }
        if (flag)
        {
            StartCoroutine(Login(email, password));
        }
        
    }

    void RegisterButtonOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    void GoBackToMainScreenButtonOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void LogoutButtonOnClick()
    {
        PlayerPrefs.SetString("LoginToken", "");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    IEnumerator Login(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/login.php", form);
        yield return www;
        ShowToast(www.text, 10);
        if(www.text == "Bienvenido")
        {
            System.Random random = new System.Random();
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string token = new string(Enumerable.Repeat(chars, 40).Select(s => s[random.Next(s.Length)]).ToArray());
            form.AddField("token", token);
            www = new WWW("https://tourismappar.000webhostapp.com/insert_login_token.php", form);
            yield return www;
            PlayerPrefs.SetString("LoginToken", www.text);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    void ShowToast(string text, int duration)
    {
        if (showToastCoroutine != null)
        {
            StopCoroutine(showToastCoroutine);
        }
        showToastCoroutine = ShowToastCoroutineMet(text, duration);
        StartCoroutine(showToastCoroutine);
    }

    private IEnumerator ShowToastCoroutineMet(string text, int duration)
    {
        loginMessage.text = text;
        loginMessage.enabled = true;
        loginMessage.color = Color.red;

        //Fade in
        yield return FadeInAndOut(loginMessage, true, 0.1f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return FadeInAndOut(loginMessage, false, 0.5f);

        loginMessage.enabled = false;
    }

    IEnumerator FadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
}