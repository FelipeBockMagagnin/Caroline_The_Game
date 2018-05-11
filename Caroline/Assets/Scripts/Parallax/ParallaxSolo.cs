using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSolo : MonoBehaviour
{

    public float velParallax;
    public Renderer quad;



    void Update()
    {
        Vector2 offset = new Vector2(velParallax * Time.deltaTime, 0);
        quad.material.mainTextureOffset += -offset;

     

    }

    
}