using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    //iniciar dialogo


 



    public Dialogue dialogue;



    //Trigger = startDialogue
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }


    private void Update()
    {

      


      
    }

    //Start no Dialogo se colidir com Saci
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Menina"))
        {
            TriggerDialogue();
        }
    }
}
