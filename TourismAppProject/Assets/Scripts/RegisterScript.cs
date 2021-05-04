using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour
{
    public InputField emailInput, passwordInput, repeatPasswordInput, nameInput, firstSurnameInput,
        secondSurnameInput, dayInput, monthInput, yearInput;
    public Dropdown countryDropdown, regionDropdown;
    public Button loginButton, registerButton;
    public Text registerMessage;
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
