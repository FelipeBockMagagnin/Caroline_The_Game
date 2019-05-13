using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;
using System.Text.RegularExpressions;
using System.Linq;

public class NormalDialogue : MonoBehaviour
{
    public GameObject dialogueCanvas;
    public TMP_Text dialogueTxt;

    public string archiveXMLName;
    public int sentenceID;
    public int  dialogueID;    
    public List<string> dialogueLines; 
    private bool dialogueOn;
    private Girl girl;
    
    public Dictionary<string, List<string>> dialogueLists = new Dictionary<string, List<string>>();    

    private void Start()
    {
        LoadDialogueData();
    }

    private void Awake()
    {
        girl = GameObject.Find("Girl").GetComponent<Girl>();
    }

    public void Interact()
    {
        if(dialogueOn == false && !playingText)
        {
            sentenceID = 0;            
            ReadyDialogue();
            DisplayDialogue();
            dialogueCanvas.SetActive(true);
            dialogueOn = true;
        }
        else if (dialogueOn == true && !playingText) 
        {
            sentenceID += 1;
            DisplayDialogue();
        }
    }

    public bool playingText;

    IEnumerator PlayText()
    {
        int totalVisibleChars = dialogueTxt.text.Length;
        int counter = 0;
        playingText = true;
        foreach(char c in dialogueTxt.text)
        {
            int visibleCounter = counter + 1;            
            dialogueTxt.maxVisibleCharacters = visibleCounter;
            counter += 1; 
            yield return new WaitForSeconds (0.02f);            	            
        }
        playingText = false;
        
    }

    public void DisplayDialogue()
    {
        if(sentenceID < dialogueLines.Count)
        {
            dialogueTxt.text = dialogueLines[sentenceID];
            StartCoroutine(PlayText());
        }
        else    //ends dialogue
        {
            if(dialogueID >= dialogueLists.Count-1)
            {
                dialogueID = dialogueLists.Count-1;
                ReadyDialogue();
                Debug.Log("Final dialogue id : " + dialogueID);
            }
            else 
            {
                dialogueID += 1;
                Debug.Log("Dialogue id : " + dialogueID);
            }                       
            dialogueCanvas.SetActive(false);
            dialogueOn = false;
            girl.canMove = true;
        }
    }

    public void ReadyDialogue()
    {
        dialogueLines.Clear();     
        for(int x = 0; x <= dialogueLists.Count; x++)
        {
            if(x == dialogueID)
            {
                dialogueLines = dialogueLists.ElementAt(x).Value;
                Debug.Log("Escolhido dialogo numero: " + x);
            }
        }       
    }        
    
    //Read XML archive
    private void LoadDialogueData()
    {
        TextAsset xmlData = (TextAsset)Resources.Load("pt-br/" + archiveXMLName);
        XmlDocument xmlDocument = new XmlDocument();
        
        xmlDocument.LoadXml(xmlData.text);
        
        foreach(XmlNode dialogue in xmlDocument["dialogues"].ChildNodes)
        {
            string dialogueName = dialogue.Attributes["name"].Value; 
            List<string> currentList = new List<string>();
            currentList.Clear(); 
            foreach(XmlNode d in dialogue["sentences"].ChildNodes)
            {
                currentList.Add(formatedText(d.InnerText));
            }      
            dialogueLists.Add(dialogueName, currentList);                              
        } 
    }

    public string formatedText(string sentence)
    {
        //cor = nomecor     <color = #corcorrespondente
        //fimcor            </color>
        string  temp = sentence;

        //colors
        temp = temp.Replace("(color=yellow)", "<color=#ffff00ff>");
        temp = temp.Replace("(color=red)", "<color=#ff0000ff>");
        temp = temp.Replace("(color=blue)", "<color=#0000ffff>");
        temp = temp.Replace("(color=green)", "<color=#008000ff>");
        temp = temp.Replace("(color=purple)", "<color=#800080ff>");
        temp = temp.Replace("(color=orange)", "<color=#ffa500ff>");

        //letter type
        temp = temp.Replace("(bold)","<b>");
        temp = temp.Replace("(endbold)", "</b>");

        temp = temp.Replace("(italic)","<i>");
        temp = temp.Replace("(enditalic)", "</i>");

        //end string
        temp = temp.Replace("(endcolor)","</color>");

        return temp;
    }
}
