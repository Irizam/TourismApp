using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class RegisterScript : MonoBehaviour
{
    public InputField emailInput, passwordInput, repeatPasswordInput, nameInput, firstSurnameInput,
        secondSurnameInput, dayInput, monthInput, yearInput;
    public Dropdown countryDropdown, regionDropdown;
    public Button loginButton, registerButton;
    public Text registerMessage, emailError, passwordError, repeatpasswordError, nameError, firstSurnameError, secondSurnameError, dateOfBirthError;
    List<string> regionsStringList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(LoginButtonOnClick);
        registerButton.onClick.AddListener(RegisterButtonOnClick);
        countryDropdown.onValueChanged.AddListener(delegate {
            CountryDropdownOnValueChanged();
        });
        passwordInput.inputType = repeatPasswordInput.inputType = InputField.InputType.Password;
        StartCoroutine(LoadCountries());
        StartCoroutine(LoadRegions());
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void RegisterButtonOnClick()
    {
        bool errorFlag = false;
        emailError.text="";
        passwordError.text="";
        repeatpasswordError.text="";
        nameError.text="";
        firstSurnameError.text="";
        secondSurnameError.text="";
        dateOfBirthError.text="";
        registerMessage.text="";
        try//validations
        {
            if (emailInput.text != "")//email ****VERIFICAR QUE NO SE REPITA EN LA BASE DE DATOS****
            {
                try
                {
                    System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailInput.text);
                }
                catch (System.Exception)
                {
                    errorFlag = true;
                    emailError.text = "Formato incorrecto.";
                }
            }else{
                errorFlag = true;
                emailError.text = "El correo requerido.";
            }

            if (passwordInput.text != "")//password
            {
                if (passwordInput.text.Length < 8)
                {
                    errorFlag = true;
                    passwordError.text="Mínimo 8 caracteres";
                }
            }else{
                errorFlag = true;
                passwordError.text="Contraseña requerida.";
            }

            if (repeatPasswordInput.text != "")//repeat password
            {
                if (repeatPasswordInput.text != passwordInput.text)
                {
                    errorFlag = true;
                    repeatpasswordError.text="Repita la contraseña.";
                }
            }else{
                errorFlag = true;
                repeatpasswordError.text="Repita la contraseña.";
            }

            
            Regex rgx = new Regex(@"^[a-zA-Z]{1,60}$");
            if (nameInput.text != "")//name
            {
                if (!rgx.IsMatch(nameInput.text))
                {
                    errorFlag = true;
                    nameError.text = "Nombre erroneo.";
                }
            }else{
                errorFlag = true;
                nameError.text = "Nombre requerido.";
            }
            
            if (firstSurnameInput.text != "")//surname
            {
                if (!rgx.IsMatch(firstSurnameInput.text))
                {
                    errorFlag = true;
                    firstSurnameError.text = "Apellido erroneo.";
                }
            }else{
                errorFlag = true;
                firstSurnameError.text="Apellido requerido.";
            }

            if (secondSurnameInput.text != "")//second surname
            {
                if (!rgx.IsMatch(secondSurnameInput.text))
                {
                    errorFlag = true;
                    secondSurnameError.text = "Apellido erroneo.";
                }
            }

            System.DateTime d;//date of birth
            if (dayInput.text.Length == 1)
                dayInput.text = "0"+dayInput.text;
            if (monthInput.text.Length == 1)
                monthInput.text = "0"+monthInput.text;
            bool chValidity = System.DateTime.TryParseExact(
            dayInput.text+"/"+monthInput.text+"/"+yearInput.text,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out d);
            if (!chValidity)
            {
                dateOfBirthError.text="Fecha no válida.";
                errorFlag = true;
            }
            if (d.Year >= System.DateTime.Now.Year || d.Year < 1900)
            {
                dateOfBirthError.text="Año no válido.";
                errorFlag = true;
            }

            if (!errorFlag)
            {
                string email = emailInput.text, password = passwordInput.text, repeatPassword = repeatPasswordInput.text,
                   name = nameInput.text, firstSurname = firstSurnameInput.text, secondSurname = secondSurnameInput.text,
                   day = dayInput.text, month = monthInput.text, year = yearInput.text,
                   country = countryDropdown.options[countryDropdown.value].text, region = "";
                if (regionDropdown.options.Count > 0)
                {
                    region = regionDropdown.options[regionDropdown.value].text;
                    StartCoroutine(Register(email, password, name, firstSurname, secondSurname, day, month, year, country, region));
                }
                else
                {
                    StartCoroutine(Register(email, password, name, firstSurname, secondSurname, day, month, year, country));
                }
            }
        }
        catch (System.Exception ex)
        {
            registerMessage.text = ex.Message;
        }
    }

    void CountryDropdownOnValueChanged()
    {
        if(countryDropdown.options[countryDropdown.value].text == "Bolivia")
        {
            regionDropdown.AddOptions(regionsStringList);
            regionDropdown.interactable = true;
        }
        else
        {
            regionDropdown.ClearOptions();
            regionDropdown.interactable = false;
        }
    }

    IEnumerator Register(string email, string password, string name, string firstSurname,
        string secondSurname, string day, string month, string year, string country)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", name);
        form.AddField("firstSurname", firstSurname);
        form.AddField("secondSurname", secondSurname);
        form.AddField("day", day);
        form.AddField("month", month);
        form.AddField("year", year);
        form.AddField("country", country);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/register_user.php", form);
        yield return www;
        registerMessage.text = www.text;

        Send(email, name, firstSurname, password);

        if(www.text == "Usuario registrado")
        {
            registerButton.enabled = false;
            yield return new WaitForSeconds(5);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    IEnumerator Register(string email, string password, string name, string firstSurname,
        string secondSurname, string day, string month, string year, string country, string region)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("name", name);
        form.AddField("firstSurname", firstSurname);
        form.AddField("secondSurname", secondSurname);
        form.AddField("day", day);
        form.AddField("month", month);
        form.AddField("year", year);
        form.AddField("country", country);
        form.AddField("region", region);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/register_user.php", form);
        yield return www;
        registerMessage.text = www.text;

        
        Send(email, name, firstSurname, password);

        if (www.text == "Usuario registrado")
        {
            registerButton.enabled = false;
            yield return new WaitForSeconds(5);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public static void Send(string email, string name, string firstname, string pswd) {
 
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("TourismAppAR@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Bienvenido a TourismApp <3";
        mail.Body = "Cuenta creada para "+name+" "+firstname+" con la contraseña "+pswd;
 
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

    IEnumerator LoadCountries()
    {
        WWW www = new WWW("https://tourismappar.000webhostapp.com/countries.php");
        yield return www;
        List<string> countriesStringList = new List<string>();
        string[] countriesStringArray = www.text.Split('|');
        countryDropdown.ClearOptions();
        foreach (string country in countriesStringArray)
        {
            countriesStringList.Add(country);
        }
        countriesStringList.RemoveAt(countriesStringList.Count - 1);
        countryDropdown.AddOptions(countriesStringList);
    }

    IEnumerator LoadRegions()
    {
        WWW www = new WWW("https://tourismappar.000webhostapp.com/regions.php");
        yield return www;
        string[] regionsStringArray = www.text.Split('|');
        regionDropdown.ClearOptions();
        regionsStringList.Clear();
        foreach (string country in regionsStringArray)
        {
            regionsStringList.Add(country);
        }
        regionsStringList.RemoveAt(regionsStringList.Count - 1);
        regionDropdown.AddOptions(regionsStringList);
    }
}
