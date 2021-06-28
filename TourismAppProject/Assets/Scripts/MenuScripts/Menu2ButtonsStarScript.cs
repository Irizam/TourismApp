using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;




//Funcionalidad 100%
public class Menu2ButtonsStar : MonoBehaviour
{
    

    public InputField commentInput;//Instanciamos le inputField

    public Text commentMessage;//Se utiliza para los errores(Pureba, se puede borrar luego)
    string token, idClient="user"; //Variables para agarrar los datos

    /// <summary>
    /// Logica para obtener el ID
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        token = PlayerPrefs.GetString("LoginToken");
        WWWForm form = new WWWForm();
        form.AddField("loginToken", token);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_idClient.php", form);
        yield return www;
        idClient = www.text;
        string idTouristSpot = PlayerPrefs.GetString("SpotID"); ;
    }
    
    /// <summary>
    /// Declaracion de elementos de la interfaz Grafica
    /// </summary>
    public Sprite newButtonImg;
    public Sprite oldButtonImg;
    public Button buttonStar1;
    public Button buttonStar2;
    public Button buttonStar3;
    public Button buttonStar4;
    public Button buttonStar5;
    public int counter1 = 2;
    public string totalStarsRate = "";
    /// <summary>
    /// Logica para la puntuacion y registrarlo a la BDD
    /// </summary>
    /// <param name="score"></param>
    /// <param name="idClient"></param>
    /// <returns></returns>
    IEnumerator RegisterScore(string score, string idClient)
    {
        string idTouristSpot = PlayerPrefs.GetString("SpotID"); ;

        WWWForm form = new WWWForm();
        form.AddField("score", score);
        form.AddField("idClient", idClient);
        form.AddField("idTuristSpot", idTouristSpot);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/score.php", form);
        yield return www;

        string texto = www.text;
        //commentMessage.text = texto;
    }
    /// <summary>
    /// Logica para pasar de una imagen a otra
    /// y para mandar los datos a las consultas
    /// </summary>
    #region ChangeImage
    public void ChangeImage1()
    {
        
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = "1";

            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient));



        
        
        
    } 
    public void ChangeImage2()
    {
        
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;

            totalStarsRate = "2";

            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient));
        
       
    }

    public void ChangeImage3()
    {
        
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = newButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;

            totalStarsRate = "3";

            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient));
        
        
    }

    public void ChangeImage4()
    {
        
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = newButtonImg;
            buttonStar4.image.sprite = newButtonImg;
            buttonStar5.image.sprite = oldButtonImg;

            totalStarsRate = "4";
            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient));
        
        
    }

    public void ChangeImage5()
    {
        
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = newButtonImg;
            buttonStar4.image.sprite = newButtonImg;
            buttonStar5.image.sprite = newButtonImg;

            totalStarsRate = "5";
            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient));
        
    }
    #endregion
}
