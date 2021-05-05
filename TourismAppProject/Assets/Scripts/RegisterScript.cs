using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void RegisterButtonOnClick()
    {
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
                    emailError.text = "Formato incorrecto.";
                }
            }else{
                emailError.text = "El correo requerido.";
            }

            if (passwordInput.text != "")//password
            {
                if (passwordInput.text.Length < 8)
                {
                    passwordError.text="Mínimo 8 caracteres";
                }
            }else{
                passwordError.text="Contraseña requerida.";
            }

            if (repeatPasswordInput.text != "")//repeat password
            {
                if (repeatPasswordInput.text != passwordInput.text)
                {
                    repeatpasswordError.text="Repita la contraseña.";
                }
            }else{
                repeatpasswordError.text="Repita la contraseña.";
            }

            
            Regex rgx = new Regex(@"^[a-zA-Z]{1,60}$");
            if (nameInput.text != "")//name
            {
                if (!rgx.IsMatch(nameInput.text))
                {
                    nameError.text = "Nombre erroneo.";
                }
            }else{
                nameError.text = "Nombre requerido.";
            }
            
            if (firstSurnameInput.text != "")//surname
            {
                if (!rgx.IsMatch(firstSurnameInput.text))
                {
                    firstSurnameError.text = "Apellido erroneo.";
                }
            }else{
                firstSurnameError.text="Apellido requerido.";
            }

            if (secondSurnameInput.text != "")//second surname
            {
                if (!rgx.IsMatch(secondSurnameInput.text))
                {
                    secondSurnameError.text = "apellido erroneo.";
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
            }
            if (d.Year >= System.DateTime.Now.Year || d.Year < 1900)
            {
                dateOfBirthError.text="Año no válido.";
            }

        }
        catch (System.Exception ex)
        {
            registerMessage.text = ex.Message;
        }
        /*
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
        }*/
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
        Debug.Log("Register w/o region");
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
    }

    IEnumerator Register(string email, string password, string name, string firstSurname,
        string secondSurname, string day, string month, string year, string country, string region)
    {
        Debug.Log("Register w/ region");
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
        Debug.Log(regionsStringList.Count.ToString());
    }
}
