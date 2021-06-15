using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CommentScript : MonoBehaviour
{
    //Definimos variables
    public InputField commentInput;
    public Button commentButton;
    public Text commentMessage;
    string token, idClient;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        commentButton.onClick.AddListener(CommentButtonOnClick);
        //Hagarramos el id del cliente
        token = PlayerPrefs.GetString("LoginToken");
        WWWForm form = new WWWForm();
        form.AddField("loginToken", token);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_idClient.php", form);
        yield return www;
        idClient = www.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CommentButtonOnClick()
    {
        //Metodo del boton que llama al metodo para insetar el comentario
        commentMessage.text = "";
        Regex rgx = new Regex(@"^[a-zA-Z]{1,60}$");
        
        string comment = commentInput.text, idTouristSpot = PlayerPrefs.GetString("SpotID"); ;
        if (!rgx.IsMatch(comment))
        {
            StartCoroutine(RegisterComment(comment, idClient, idTouristSpot));
            commentInput.text = "";
        }
        else
        {
            commentMessage.text = "Solo se permiten letras y números";
        }
        
    }

    IEnumerator RegisterComment(string comment, string idClient, string idTuristSpot)
    {
        // Metodo para insertar comentarios
        WWWForm form = new WWWForm();
        form.AddField("comment", comment);
        form.AddField("idClient", idClient);
        form.AddField("idTuristSpot", idTuristSpot);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/insert_comment.php", form);
        yield return www;
        if (www.text== "Por favor no use lenguaje ofensivo")
        {
            commentMessage.text = www.text;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }

        
    }
}
