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
    private IEnumerator showToastCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(LoginButtonOnClick);
        registerButton.onClick.AddListener(RegisterButtonOnClick);
        passwordInput.inputType = InputField.InputType.Password;
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
        string email = emailInput.text, password = passwordInput.text;
        StartCoroutine(Login(email, password));
    }

    void RegisterButtonOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    IEnumerator Login(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/login.php", form);
        yield return www;
        ShowToast(www.text, 10);
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