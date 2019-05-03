using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float vel;


    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0,vel*Time.deltaTime,0);
        } 
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0,-vel*Time.deltaTime,0);
        }
    }
}
