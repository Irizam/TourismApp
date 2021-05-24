using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;





public class Menu2ButtonsStar : MonoBehaviour
{

    public InputField commentInput;

    public Text commentMessage;
    string token, idClient = "user";


    IEnumerator Start()
    {
        

        token = PlayerPrefs.GetString("LoginToken");
        WWWForm form = new WWWForm();
        form.AddField("loginToken", token);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_idClient.php", form);
        yield return www;
        idClient = www.text;
        
    }


    // Start is called before the first frame update
    public Sprite newButtonImg;
    public Sprite oldButtonImg;
    public Button buttonStar1;
    public Button buttonStar2;
    public Button buttonStar3;
    public Button buttonStar4;
    public Button buttonStar5;
    public int counter1 = 2;
    public string totalStarsRate = "";

    IEnumerator RegisterScore(string score, string idClient, string idTuristSpot)
    {
        WWWForm form = new WWWForm();
        form.AddField("score", score);
        form.AddField("idClient", idClient);
        form.AddField("idTuristSpot", idTuristSpot);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/score.php", form);
        yield return www;
        commentMessage.text = www.text;
    }

    public void ChangeImage1()
    {
        counter1 = 2;
        counter1++;
        if (counter1 % 2 ==1)
        {
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = "1";

            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient, idTouristSpot));



        }
        else
        { 
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = "0";
        }
        
    }

    public void ChangeImage2()
    {
        counter1 = 2;
        counter1++;
        if (counter1 % 2 == 1)
        {
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;

            totalStarsRate = "2";

            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient, idTouristSpot));
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = "0";
        }

    }

    public void ChangeImage3()
    {
        counter1 = 2;
        counter1++;
        if (counter1 % 2 == 1)
        {
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = newButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;

            totalStarsRate = "3";

            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient, idTouristSpot));
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = "0";
        }

    }

    public void ChangeImage4()
    {
        counter1 = 2;
        counter1++;
        if (counter1 % 2 == 1)
        {
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = newButtonImg;
            buttonStar4.image.sprite = newButtonImg;
            buttonStar5.image.sprite = oldButtonImg;

            totalStarsRate = "4";
            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient, idTouristSpot));
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = "0";
        }

    }

    public void ChangeImage5()
    {
        counter1 = 2;
        counter1++;
        if (counter1 % 2 == 1)
        {
            buttonStar1.image.sprite = newButtonImg;
            buttonStar2.image.sprite = newButtonImg;
            buttonStar3.image.sprite = newButtonImg;
            buttonStar4.image.sprite = newButtonImg;
            buttonStar5.image.sprite = newButtonImg;


            totalStarsRate = "5";
            string score = totalStarsRate, idTouristSpot = "1";
            StartCoroutine(RegisterScore(score, idClient, idTouristSpot));
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = "0";
        }

    }



    
}
