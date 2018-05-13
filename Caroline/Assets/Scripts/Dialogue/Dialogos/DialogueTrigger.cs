using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    //iniciar dialogo
    public Dialogue dialogue;
    public GameObject dialoguemanager;   

    //Trigger = startDialogue
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    //Start no Dialogo se colidir com Saci
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Menina"))
        {
            TriggerDialogue();
                Destroy(gameObject);
               
            
        }
    }
    



}
