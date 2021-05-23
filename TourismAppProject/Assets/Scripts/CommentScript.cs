using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CommentScript : MonoBehaviour
{
    public InputField commentInput;
    public Button commentButton;
    public Text commentMessage;
    string token,idClient="puto";
    // Start is called before the first frame update
    IEnumerator Start()
    {
        commentButton.onClick.AddListener(CommentButtonOnClick);

        token = PlayerPrefs.GetString("LoginToken");
        WWWForm form = new WWWForm();
        form.AddField("loginToken", token);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_idClient.php", form);
        yield return www;
        idClient = www.text;
        commentMessage.text =idClient;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CommentButtonOnClick()
    {
        commentMessage.text = "";
        Regex rgx = new Regex(@"^[a-zA-Z]{1,60}$");
        string comment = commentInput.text, idTouristSpot = "1";
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
    IEnumerator GetIdClient(string token)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginToken", token);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/get_idClient.php", form);
        yield return www;
        idClient = www.text;
    }
    IEnumerator RegisterComment(string comment, string idClient, string idTuristSpot)
    {
        WWWForm form = new WWWForm();
        form.AddField("comment", comment);
        form.AddField("idClient", idClient);
        form.AddField("idTuristSpot", idTuristSpot);
        WWW www = new WWW("https://tourismappar.000webhostapp.com/insert_comment.php", form);
        yield return www;
        commentMessage.text = www.text;
    }
}
