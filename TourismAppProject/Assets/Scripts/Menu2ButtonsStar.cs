using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Menu2ButtonsStar : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite newButtonImg;
    public Sprite oldButtonImg;
    public Button buttonStar1;
    public Button buttonStar2;
    public Button buttonStar3;
    public Button buttonStar4;
    public Button buttonStar5;
    public int counter1 = 2;
    public int totalStarsRate = 0;
    
    void Start()
    {
        
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


            totalStarsRate = 1;
        }
        else
        { 
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = 0;
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

            totalStarsRate = 2;
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = 0;
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

            totalStarsRate = 3;
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = 0;
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

            totalStarsRate = 4;
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = 0;
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


            totalStarsRate = 5;
        }
        else
        {
            buttonStar1.image.sprite = oldButtonImg;
            buttonStar2.image.sprite = oldButtonImg;
            buttonStar3.image.sprite = oldButtonImg;
            buttonStar4.image.sprite = oldButtonImg;
            buttonStar5.image.sprite = oldButtonImg;


            totalStarsRate = 0;
        }

    }
}
