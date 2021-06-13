using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCalificacion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(BtnCalificacion_click);
    }

    // Update is called once per frame
    public void BtnCalificacion_click()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

}
