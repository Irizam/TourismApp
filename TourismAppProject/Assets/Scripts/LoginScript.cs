using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class LoginScript : MonoBehaviour
{
    #region Login Window
    [Header("Login Window")]
    public GameObject loginWindow;
    public Button loginButton, registerButton, remindButton, confirmButton;
    public InputField emailInput, passwordInput;
    public Text loginMessage, errorEmailMessage, errorPasswordMessage;
    private IEnumerator showToastCoroutine;
    #endregion

    #region User Window
    [Header("User Window")]
    public GameObject userWindow;
    public GameObject confirmButtonField;
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
        remindButton.onClick.AddListener(RemindButtonOnClick);
        confirmButton.onClick.AddListener(ConfirmButtonOnClick);
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

    string email="";
    string vercode="";
    void RemindButtonOnClick()
    {
        errorEmailMessage.text = "";
        if (emailInput.text != "")
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailInput.text);

            }
            catch (System.Exception)
            {
                errorEmailMessage.text = "Formato incorrecto.";
            }
            email = emailInput.text;
            
            SendMail(email);

            loginMessage.text="Revise el correo que ingresó";

            confirmButtonField.SetActive(true);
        }
        else
        {
            errorEmailMessage.text = "Ingrese el correo.";
        }
    }

    void ConfirmButtonOnClick()
    {
        if (emailInput.text == vercode)
        {
            if(passwordInput.text!="" && passwordInput.text.Length >= 8 && passwordInput.text.Length <= 20)
            {
                StartCoroutine(UpdatePswdAndSendEmail(email, passwordInput.text));
            }
            else
            {
                errorPasswordMessage.text="contraseña invalida";
            }

        }
        else
        {
            errorEmailMessage.text = "El código es incorrecto";
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

    
    void SendMail(string email)
    {
        
        System.Random random=new System.Random();
        vercode=random.Next(100000,999999) + "";


        Send(email, vercode);

    }
    IEnumerator UpdatePswdAndSendEmail(string email, string newPswd)
    {
        
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("pswd", newPswd);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/update_password.php", form);
        yield return www;
        if(www.text=="success")
        {

        SendSuccessEmail(email, newPswd);

        loginMessage.text="Se actualizó correctamente";
        vercode="";
        passwordInput.text="";
        emailInput.text="";
        email="";
        
        confirmButtonField.SetActive(false);
        }
        else
        {
        loginMessage.text="nope";
        }


    }

    public static void Send(string email, string vercod) {
 
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("TourismAppAR@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Recordar contraseña";
        mail.Body = "Introduzca el codigo " + vercod+" en el campo de Correo Electronico y su nueva contraseña en el campo de contraseña, luego presione Confirmar";
 
        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.Port = 587;
        smtp.Credentials = new System.Net.NetworkCredential("TourismAppAR@gmail.com", "Tourism69App69AR69") as ICredentialsByHost;
        smtp.EnableSsl = true;
 
        ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                    return true;
                };
        smtp.Send(mail);
    }
    public static void SendSuccessEmail(string email, string newPswd) {
 
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("TourismAppAR@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Recordar contraseña";
        mail.Body = "Su nueva contraseña es " + newPswd;
 
        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.Port = 587;
        smtp.Credentials = new System.Net.NetworkCredential("TourismAppAR@gmail.com", "Tourism69App69AR69") as ICredentialsByHost;
        smtp.EnableSsl = true;
 
        ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                    return true;
                };
        smtp.Send(mail);
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