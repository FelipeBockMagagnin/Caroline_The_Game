using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSolo : MonoBehaviour
{

    public float    velParallax;    //guarda velocidade de parallax
    public Renderer quad;           //guarda o parallax

    void Update()
    {
        Vector2 offset = new Vector2(velParallax * Time.deltaTime, 0);
        quad.material.mainTextureOffset += -offset;
    }    
}