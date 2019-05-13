using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyFather {	        	       

    bool kill = false;                  //se for true no ultimo frame da animação, ele mata         
      

    //*************start************************************
    //start components on spawn
    private void OnEnable() {
        girl = GameObject.Find("Girl");
        if(WhatFollow == null)
        {
            WhatFollow = girl.transform;
        }
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
        if(!attacking)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }		
        TimeCount();
        Follow(WhatFollow);
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
            Follow(WhatFollow);            
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
        if(!pushed)
        {
            Follow(WhatFollow);
        }
        if(kill && !pushed)
        {
            Girl.Reload();
        }              
    }

    /// <summary>
    /// finalize the animation and make the enemy stop for a litle time
    /// </summary>
    void Pushed()
    {
        anim.SetBool("ataque1", false);
        SpawnHitParticle();
        pushed = true;                
        time = time + Random.Range(2.7f,3.5f);
        counter = true;
    }


    /// <summary>
    /// destroy the enemy and spawn particles
    /// </summary>
    public virtual void DestroyThis()
    {
        SpawnHitParticle();
        Destroy(this.gameObject);
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
            DestroyThis();
        }

        if(collision.CompareTag(WhatFollow.tag)){
            if(WhatFollow != girl.transform)
            {
                Atack();
                kill = true;
            }
            else 
            {
                if(girl.GetComponent<Girl>().canBeAttacked)
                {
                    Atack();
                    kill = true;
                }  
            }            
        }
          
      //colisão com a pedra
        if (collision.CompareTag("pedra")){
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
    }

    void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag(WhatFollow.tag)){
            kill = false;
        }
    }   

    private void OnTriggerStay2D(Collider2D collision){        
        //garantir que o inimigo continue atacando caso ele não saia do colisor da Menina
        if(collision.CompareTag("Menina") && justGirl && kill)
        {
            if(girl.GetComponent<Girl>().canBeAttacked)
            {
                Atack();
            }            
        }        

        if (collision.CompareTag("Spell_Menina_Empurrar") && justGirl && !pushed)
        {
            audioManager.PlayEnemyHitSound();
            if (girl.transform.position.x >= transform.position.x){                
                Destroy(collision);
                Destroy(collision.gameObject);
                DestroyThis();
            }

            if (collision.transform.position.x < transform.position.x){   
                Destroy(collision);
                Destroy(collision.gameObject);
                DestroyThis();
            }
        }

        if (collision.CompareTag("Spell_Menina_Empurrar") && !justGirl && !pushed)
        {
            audioManager.PlayEnemyHitSound();
            if (WhatFollow.transform.position.x >= transform.position.x){                
                Destroy(collision);
                Destroy(collision.gameObject);
                DestroyThis();               
            }

            if (WhatFollow.transform.position.x < transform.position.x){    
                Destroy(collision);
                Destroy(collision.gameObject);
                DestroyThis();
            }
        }
    }
}
