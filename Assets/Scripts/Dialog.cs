using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public Sprite profile;
    public string speechTxt;

    public string actorName;

    private DialogControl dc;

    public LayerMask playerLayer;

    private void Start()
    {
        //Procura na cena um objeto q tenha classe DialogControl
        dc = FindObjectOfType<DialogControl>();
    }

    public void Interact()
    {
        //Colisor invisivel para detectar player
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer);

        if(hit != null){
            //Chama o metodo StartDialog() da classe DialogControl
            dc.Speech(profile, speechTxt, actorName);
        }
    }

    void FixedUpdate(){
        Interact();
    }
}
