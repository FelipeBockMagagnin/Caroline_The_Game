using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Divulgação : MonoBehaviour
{

    public GameObject dialoguepanel;

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.KeypadEnter))
       {
           SceneManager.LoadScene ("MiniGame", LoadSceneMode.Additive);
           dialoguepanel.SetActive(false);
       } 
    }
}
