using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;

public class NormalDialogue : MonoBehaviour
{
    public GameObject dialogueCanvas;
    public TMP_Text dialogueTxt;

    public string archiveXMLName;

    public int sentenceID;
    public int  dialogueID;    
    //public List<string> dialogue0;
    //public List<string> dialogue1;
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
        if(dialogueOn == false)
        {
            sentenceID = 0;            
            ReadyDialogue();
            DisplayDialogue();
            dialogueCanvas.SetActive(true);
            dialogueOn = true;
        }
        else 
        {
            sentenceID += 1;
            DisplayDialogue();
        }
    }


    IEnumerator waitLether()
    {
        yield return new WaitForSeconds(0.2f);
    }

    int currentlyAnimatedIndex =0;
    string currentlyAnimatedText = "";//assign dialogue text here
    string tempAnimatedText = "";

    private void Update()
    {
        girl.canMove = true;
        if (currentlyAnimatedIndex < currentlyAnimatedText.Length)
        {
            currentlyAnimatedIndex++;
            dialogueTxt.text = currentlyAnimatedText;
            tempAnimatedText = dialogueTxt.text.Insert(currentlyAnimatedIndex, "<color=#00000000>");
            dialogueTxt.text = tempAnimatedText;
        }
    }

    public void DisplayDialogue()
    {
        if(sentenceID < dialogueLines.Count)
        {
            currentlyAnimatedText = dialogueLines[sentenceID];
 
        }
        else    //ends dialogue
        {
            switch(dialogueID)
            {
                case 0:
                    dialogueID = 1;
                break;
                case 1:
                    
                break;
            }  
            dialogueCanvas.SetActive(false);
            dialogueOn = false;
            girl.canMove = true;
        }
    }

    public void ReadyDialogue()
    {
        dialogueLines.Clear();        
        foreach(KeyValuePair<string, List<string>> l in dialogueLists)
        {
            switch(dialogueID)
            {
                case 0:  
                    if(l.Key == "fala0")                  
                        dialogueLines = l.Value;
                break;

                case 1:
                    if(l.Key == "fala1")   
                        dialogueLines = l.Value;
                break;
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
                Debug.Log("AAa" + d.InnerText);
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
