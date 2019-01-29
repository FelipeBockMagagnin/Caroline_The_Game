using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTester : MonoBehaviour
{
    SceneTester instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Lvl_1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Lvl_2");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Lvl_3");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("Lvl_4");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("Lvl_5");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SceneManager.LoadScene("Lvl_6");
        }


    }
}
