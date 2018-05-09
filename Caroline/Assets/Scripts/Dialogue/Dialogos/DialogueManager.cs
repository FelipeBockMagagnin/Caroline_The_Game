using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {


    //Onde ser aescrito o dialogo
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    //Anim de dialogo
    public Animator animator;

    public bool emDialogo;


    public int conjutosDeFrases;



    public Queue<string> sentences;


	void Start () {
        sentences = new Queue<string>();
	}

    public void StartDialogue(Dialogue dialogue)
    {
        emDialogo = true; 
        animator.SetBool("IsOpen", true); //Anim de dialogo
        nameText.text = dialogue.name; //Display o nome no lugar dele
        sentences.Clear(); 

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(); 

        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & emDialogo)
        {
            DisplayNextSentence();
        }
        



    }



    public void DisplayNextSentence()               //aplica a var sentence o valor da proxima frase na lista
    {
        if(sentences.Count == 0)
        {
            EndDialogue();                          //Se o numero de frases em espera for igual a zero stop
            return;
        }
    
       string sentence = sentences.Dequeue();       
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

        void EndDialogue()
    {
        emDialogo = false;
        animator.SetBool("IsOpen", false);
        conjutosDeFrases++;


    }
    



}
