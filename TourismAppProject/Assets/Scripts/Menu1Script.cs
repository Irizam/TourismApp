using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu1Script : MonoBehaviour
{
    public RectTransform subMenu;
    float posFinal;
    bool openMenu = true;
    public float time = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        posFinal = Screen.width / 2;
        subMenu.position = new Vector3(-posFinal, subMenu.position.y, 0);
    }
    IEnumerator Move(float time, Vector3 posInit, Vector3 posFin)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            subMenu.position = Vector3.Lerp(posInit, posFin, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        subMenu.position = posFin;
    }
    void moveMenu (float time, Vector3 posInit, Vector3 posFin)
    {
        StartCoroutine(Move(time, posInit, posFin));
    }

    public void BUTTON_Sub_Menu()
    {
        int sign = 1;
        if (!openMenu)
            sign = -1;

        moveMenu(time, subMenu.position, new Vector3(sign * posFinal, subMenu.position.y, 0));
        openMenu = !openMenu;
      
    }
}
