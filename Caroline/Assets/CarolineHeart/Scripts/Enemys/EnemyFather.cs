﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyFather : MonoBehaviour {

 	[Range(0, 10)]
    public    float         maxSpeed = 0;     
    [Range(0, 10)]
    public    float         minSpeed = 0; 

	public    Transform     WhatFollow;             //dar follow
	protected float         speed;                  //contem a velocidade do inimigo
    protected float         scaleX;                 //contem escala x do inimigo
	protected Vector3       scale;                  //contem a escala do inimigo
    protected float         time = 0;               //tempo em que o inimigo fica parado
	protected bool          pushed = false;         //se ele for empurrado não pode atacar
    protected bool          counter = false;        //define de o tempo continuara descendo
    protected GameObject    girl;                   //contem a definição de menina
	public   ParticleSystem hitParticle;            //
	protected Animator      anim;                   //contem as animações do inimigo
    protected AudioManager audioManager;
    protected bool attacking = false;
    protected Girl girlScript;
    protected GameObject alertSignal;
    
    //***************MOVIMENTATION**********************************

    protected Transform decideWhatToFollow()
    {
        //Se apenas Girl
        if(WhatFollow == girl.transform && GirlManager.instance.canBeAttacked == true)
        {
            return girl.transform;
        }
        else if(WhatFollow == girl.transform && GirlManager.instance.canBeAttacked == false)
        {
            return null;
        }
        else if (WhatFollow != girl.transform && GirlManager.instance.canBeAttacked == false)
        {
            return WhatFollow;
        }
        else if (WhatFollow != girl.transform && GirlManager.instance.canBeAttacked == true)
        {
            return girl.transform;
        }
        return null;
    }

    /// <summary>
    /// Follow an object, just horizontal
    /// </summary>
    /// <param name="followObject"></param>
    protected void Follow()
    {
        Transform followObject = decideWhatToFollow();
        if(followObject != null)
        {
            alertSignal.SetActive(false);
            Debug.Log("disable alert");
        } 
        else 
        {
            alertSignal.SetActive(true);
            Debug.Log("enable alert");
        }

        if(!pushed && !attacking && followObject != null)
        {
            if (followObject.transform.position.x >= transform.position.x)
            {
                speed = UnityEngine.Random.Range(minSpeed,maxSpeed);
                scale = transform.localScale;
                scale.x = Mathf.Abs(scaleX);
                transform.localScale = scale;        
            }
            else
            {
                speed = -(UnityEngine.Random.Range(minSpeed,maxSpeed));
                scale = transform.localScale;
                scale.x = -(Mathf.Abs(scaleX));
                transform.localScale = scale;       
            }
            Move(speed);
        }
    }


    protected void Move(float _speed)
    {
        transform.Translate(_speed * Time.deltaTime, 0, 0);
    }

    /// <summary>
    /// set the of enemy according to the time
    /// </summary>
    /// <param name="InitialColor"></param>
    public virtual void EnemyColor(float InitialColor)
    {
        float color;
        color = 1 - InitialColor / 3.33f;
        GetComponent<SpriteRenderer>().color = new Color(1, color, color);    
    }

    /// <summary>
    /// spawn hitParticle on hit
    /// </summary>
	protected void SpawnHitParticle()
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// sound with exeption tratament
    /// </summary>
    protected void PlayEnemyHitSound()
    {
        try
        {
            audioManager.PlayEnemyHitSound();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }
}
