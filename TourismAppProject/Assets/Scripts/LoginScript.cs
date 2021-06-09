using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    #region Login Window
    [Header("Login Window")]
    public GameObject loginWindow;
    public Button loginButton, registerButton;
    public InputField emailInput, passwordInput;
    public Text loginMessage, errorEmailMessage, errorPasswordMessage;
    private IEnumerator showToastCoroutine;
    #endregion

    #region User Window
    [Header("User Window")]
    public GameObject userWindow;
    public Button updateUserButton, logoutButton, deleteButton, goBackButton, deleteAccountButton;
    public InputField nameInput, firstSurnameInput, secondSurnameInput;
    public Canvas blackScreen;
    public Text dateOfBirthMessage, errorNameMessage, errorFirstSurnameMessage, errorSecondSurnameMessage;
    bool updatingUser;
    #endregion

    #region Bottom Bar
    [Header("Bottom Bar")]
    public Button goBackToMainScreenButton;
    #endregion

    private bool isDeleteUserWindowOpen = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        loginButton.onClick.AddListener(LoginButtonOnClick);
        registerButton.onClick.AddListener(RegisterButtonOnClick);
        passwordInput.inputType = InputField.InputType.Password;

        updateUserButton.onClick.AddListener(UpdateUserOnClick);
        logoutButton.onClick.AddListener(LogoutButtonOnClick);
        deleteButton.onClick.AddListener(DeleteButtonOnClick);
        deleteAccountButton.onClick.AddListener(DeleteAccountButtonOnClick);
        goBackButton.onClick.AddListener(GoBackToMainScreenButtonOnClick);

        float canvasX = userWindow.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float canvasY = userWindow.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        blackScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasX, canvasY);
        blackScreen.enabled = false;

        goBackToMainScreenButton.onClick.AddListener(GoBackToMainScreenButtonOnClick);

        if (PlayerPrefs.GetString("LoginToken") != "") {
            loginWindow.SetActive(false);
            userWindow.SetActive(true);

            WWWForm form = new WWWForm();
            form.AddField("loginToken", PlayerPrefs.GetString("LoginToken"));
            WWW www = new WWW("https://tourismappar.000webhostapp.com/get_user_data.php", form);
            yield return www;
            string[] userDataStringArray = www.text.Split('|');
            nameInput.text = userDataStringArray[0];
            firstSurnameInput.text = userDataStringArray[1];
            secondSurnameInput.text = userDataStringArray[2];
            string[] dateOfBirthStringArray = userDataStringArray[3].Split('-');
            dateOfBirthMessage.text = dateOfBirthStringArray[2] + " de " + GetMonthInString(dateOfBirthStringArray[1]) + ", " + dateOfBirthStringArray[0];

            updatingUser = false;
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
        String expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        bool flag = true;
        string email = emailInput.text, password = passwordInput.text;
        if (email=="")
        {
            errorEmailMessage.text = "El correo es requerido";
            flag = false;
        }
        if (!Regex.IsMatch(email, expresion))
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
        if(!isDeleteUserWindowOpen)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        else
        {
            isDeleteUserWindowOpen = false;
            blackScreen.enabled = false;
        }
    }

    void UpdateUserOnClick()
    {
        Regex rgx = new Regex(@"^[ a-zA-Z]{1,60}$");
        if (updatingUser) {
            bool errorFlag = false;

            if (nameInput.text != "")
            {
                if (!rgx.IsMatch(nameInput.text))
                {
                    errorFlag = true;
                    errorNameMessage.text = "Nombre erroneo.";
                }
            }
            else
            {
                errorFlag = true;
                errorNameMessage.text = "Nombre requerido.";
            }

            if (firstSurnameInput.text != "")
            {
                if (!rgx.IsMatch(firstSurnameInput.text))
                {
                    errorFlag = true;
                    errorFirstSurnameMessage.text = "Apellido erroneo.";
                }
            }
            else
            {
                errorFlag = true;
                errorFirstSurnameMessage.text = "Apellido requerido.";
            }

            if (secondSurnameInput.text != "")
            {
                if (!rgx.IsMatch(secondSurnameInput.text))
                {
                    errorFlag = true;
                    errorSecondSurnameMessage.text = "Apellido erroneo.";
                }
            }

            if (!errorFlag)
            {
                StartCoroutine(UpdateUser(nameInput.text, firstSurnameInput.text, secondSurnameInput.text));
                updateUserButton.GetComponentInChildren<Text>().text = "Actualizar Cuenta";
                updatingUser = false;
            }
        } else {
            updateUserButton.GetComponentInChildren<Text>().text = "Confirmar Actualización";
            updatingUser = true;
            nameInput.readOnly = firstSurnameInput.readOnly = secondSurnameInput.readOnly = false;
        }
    }

    void LogoutButtonOnClick()
    {
        PlayerPrefs.SetString("LoginToken", "");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void DeleteButtonOnClick()
    {
        isDeleteUserWindowOpen = true;
        blackScreen.enabled = true;
    }

    void DeleteAccountButtonOnClick()
    {
        StartCoroutine(DeleteUser());
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

    IEnumerator UpdateUser(string firstName, string firstSurname, string secondSurname)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginToken", PlayerPrefs.GetString("LoginToken"));
        form.AddField("firstName", firstName);
        form.AddField("firstSurname", firstSurname);
        form.AddField("secondSurname", secondSurname);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/update_user.php", form);
        yield return www;
    }

    IEnumerator DeleteUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("loginToken", PlayerPrefs.GetString("LoginToken"));
        WWW www = new WWW("https://tourismappar.000webhostapp.com/delete_user.php", form);
        yield return www;
        if(www.text == "1")
        {
            PlayerPrefs.SetString("LoginToken", "");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        Debug.Log(www.text);
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

    string GetMonthInString(string monthInInt)
    {
        string monthInString = "";
        switch (int.Parse(monthInInt))
        {
            case 1: monthInString = "enero"; break;
            case 2: monthInString = "febrero"; break;
            case 3: monthInString = "marzo"; break;
            case 4: monthInString = "abril"; break;
            case 5: monthInString = "mayo"; break;
            case 6: monthInString = "junio"; break;
            case 7: monthInString = "julio"; break;
            case 8: monthInString = "agosto"; break;
            case 9: monthInString = "septiembre"; break;
            case 10: monthInString = "octubre"; break;
            case 11: monthInString = "noviembre"; break;
            case 12: monthInString = "diciembre"; break;
        }
        return monthInString;
    }
}