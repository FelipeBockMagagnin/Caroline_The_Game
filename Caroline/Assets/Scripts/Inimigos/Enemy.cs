using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : EnemyFather {	        	       

    bool kill = false;                  //se for true no ultimo frame da animação, ele mata         
    public  bool justGirl;              //true = seguir somente menina, false = seguir e atacar saci    

    //*************start************************************
    //start components on spawn
    private void OnEnable() {
        scale = transform.localScale;
        scaleX = scale.x;
        anim = GetComponent<Animator>();
        Follow(WhatFollow);
    }

    void FixedUpdate(){
		transform.Translate(speed * Time.deltaTime, 0, 0);
        TimeCount();
	}

    //***************fight**********************************

    /// <summary>
    /// count the stop time of the enemy
    /// </summary>
    void TimeCount()
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
            anim.SetBool("ataque1", true);
        }
	}

    /// <summary>
    /// kill girl/saci of they are in the range
    /// </summary>
    void Kill()
    {
        anim.SetBool("ataque1", false);
        if(!pushed)
        {
            Follow(WhatFollow);
        }
        if(kill && !pushed)
        {
            //lose
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
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
            time = Random.Range(2.7f,3.5f);
        }

        if(collision.CompareTag(WhatFollow.tag)){
            Atack();
            kill = true;
        }
          
      //colisão com a pedra
        if (collision.CompareTag("pedra")){
            float forcaTiroAbs = girl.GetComponent<Girl>().absoluteShootingForce;
            if (WhatFollow.position.x >= transform.position.x && girl.transform.position.x >= transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -5) -1, 2);
                collision.GetComponent<ParticleSystem>().loop = false;
                collision.GetComponent<SpriteRenderer>().enabled = false;
                collision.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(collision.gameObject, 0.30f);                
                time = time + (forcaTiroAbs * 5);
                pushed = true;
                counter = true;
                SpawnHitParticle();
            }

            if(WhatFollow.position.x <= transform.position.x && girl.transform.position.x <= transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +5) +1, 2);
                collision.GetComponent<ParticleSystem>().loop = false;
                collision.GetComponent<SpriteRenderer>().enabled = false;
                collision.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(collision.gameObject, 0.20f);  
                time = time + (forcaTiroAbs * 5);
                pushed = true;
                counter = true;
                SpawnHitParticle();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag(WhatFollow.tag)){
            kill = false;
        }

        if(collision.CompareTag("Spell_Menina_Empurrar")){
            pushed = false;
        }
    }   

    private void OnTriggerStay2D(Collider2D collision){        
        //garantir que o inimigo continue atacando caso ele não saia do colisor da Menina
        if(collision.CompareTag("Menina") && justGirl && kill)
        {
            Atack();
        }        

        if (collision.CompareTag("Spell_Menina_Empurrar") && justGirl && !pushed)
        {
            if (girl.transform.position.x >= transform.position.x){                
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f, -4), 1);             //inimigo p/ esq
                Pushed();               
            }

            if (collision.transform.position.x < transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3f, 4), 1);              //inimigo p/ dir          
                Pushed();
            }
        }

        if (collision.CompareTag("Spell_Menina_Empurrar") && !justGirl && !pushed)
        {
            if (WhatFollow.transform.position.x >= transform.position.x){                
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f, -4), 1);             //inimigo p/ esq
                Pushed();                
            }

            if (WhatFollow.transform.position.x < transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3f, 4), 1);              //inimigo p/ dir          
                Pushed();
            }
        }
    }
}
