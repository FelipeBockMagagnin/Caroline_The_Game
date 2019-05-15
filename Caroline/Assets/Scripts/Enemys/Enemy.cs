using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : EnemyFather {	        	       

    bool kill = false;                  //se for true no ultimo frame da animação, ele mata

    //*************start************************************
    //start components on spawn
    private void OnEnable() {
        alertSignal = transform.Find("AlertSignal").gameObject;
        girl = GameObject.Find("Girl");
        girlScript = girl.GetComponent<Girl>();
        scale = transform.localScale;
        scaleX = scale.x;
        anim = GetComponent<Animator>();
        try
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        catch (System.Exception)
        {
        }
    }    

    void FixedUpdate(){	
        TimeCount();
        Follow();
	}

    //***************fight**********************************

    /// <summary>
    /// count the stop time of the enemy
    /// </summary>
    protected virtual void TimeCount()
    {
        if(time > 10){
            time = 10;
        }
        if(time > 0)
        {
            time = time - Time.deltaTime;
            speed = 0;
            anim.SetBool("ataque1", false);
            EnemyColor(time);
        } 
        if(time <= 0 && counter)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            pushed = false;
            counter = false;           
        }
    }

    /// <summary>
    /// start anim of Atack - girl/Saci
    /// </summary>
	void Atack()
    {
        if(!pushed && time <= 0)
        { 
            speed = 0;
            attacking = true;
            anim.SetBool("ataque1", true);
        }
	}

    /// <summary>
    /// kill girl/saci of they are in the range
    /// </summary>
    void Kill()
    {
        anim.SetBool("ataque1", false);
        attacking = false;
        if(kill && !pushed)
        {
            Girl.Reload();
        }              
    }

    /// <summary>
    /// destroy the enemy and spawn particles
    /// </summary>
    public virtual void DestroyThis(Collider2D collider)
    {
        SpawnHitParticle();
        Destroy(this.gameObject);
    }

    public void OnRockCollision()
    {   
        float forcaTiroAbs = girl.GetComponent<Girl>().absoluteShootingForce;
        if (WhatFollow.position.x >= transform.position.x && girl.transform.position.x >= transform.position.x){
            GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -5) -1, 2);          
            time = time + (forcaTiroAbs * 5);
            pushed = true;
            counter = true;
            SpawnHitParticle();
            PlayEnemyHitSound();
        }

        if(WhatFollow.position.x <= transform.position.x && girl.transform.position.x <= transform.position.x){
            GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +5) +1, 2);
            time = time + (forcaTiroAbs * 5);
            pushed = true;
            counter = true;
            SpawnHitParticle();
            PlayEnemyHitSound();
        }
    }

    //************Collision***********************************

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("morte"))
        {
            Destroy(this.gameObject);
        }

        if(collision.CompareTag("turnback"))
        {
            speed = speed * -1;
            scale.x = scale.x * -1;
            transform.localScale = scale;
        }

        //Empurrar/puxar caso haja somente a menina
        if(collision.CompareTag("Spell_Menina_Empurrar"))
        {
            girl.GetComponent<Change_camera_atributes>().NormalShake();
            Destroy(collision);
            Destroy(collision.gameObject);
            DestroyThis(collision);
        }

        try 
        {
            if(collision.CompareTag(decideWhatToFollow().tag)){
                Atack();
                kill = true;           
            }
        }
        catch(UnassignedReferenceException)
        {
            Debug.Log("Don't have char to follow");
        }
          
        //colisão com a pedra
        if (collision.CompareTag("pedra")){
            OnRockCollision();
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        try
        {
            if(collision.CompareTag(decideWhatToFollow().tag))
            {
                kill = false;
            }
        }
        catch(UnassignedReferenceException)
        {
            Debug.Log("Don't have char to follow");
        }
        
    }   

    private void OnTriggerStay2D(Collider2D collision){        
        //garantir que o inimigo continue atacando caso ele não saia do colisor da Menina
        try
        {
            if(collision.CompareTag(decideWhatToFollow().tag) && kill)
            {
                Atack();            
            } 
        }
        catch(UnassignedReferenceException)
        {
            Debug.Log("Don't have char to follow");
        }
        catch (NullReferenceException)
        {
            Debug.Log("Don't have char to follow");
        }
               

        if (collision.CompareTag("Spell_Menina_Empurrar") && !pushed)
        {
            audioManager.PlayEnemyHitSound();              
            Destroy(collision);
            Destroy(collision.gameObject);
            DestroyThis(collision);
        }
    }
}
