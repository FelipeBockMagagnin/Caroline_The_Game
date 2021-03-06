﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMoonManager : MonoBehaviour
{
    //moving
    private float speed;
    public bool move;

    //Triggers
    public float time;
    public float TimeWhenStopPlataform;
    public float TimeWhenStartSpawn;
    public float TimeWhenStopSpawn;
    public float TimeWhenStopPlataform2;

    //Musics
    public AudioClip Music1;
    public AudioClip Transition1Sound;
    public AudioClip Music2;
    public AudioClip Transition2Sound;
    public AudioClip Music3;

    //misc
    public Animator moonAnim;
    public FightMoonManager fightMoonManager;
    public Transform finalPoint;
    public Girl girl;
    
    //triggers
    private bool start = false;
    private bool startSpawn = true;
    private bool stopspawn = true;
    private bool transition1 = true;
    private bool transition2 = true;   

    private void Awake()
    {
        resetStats();        
    }

    private void resetStats()
    {
        speed = 2;
        time = 0;
        move = false;
        start = false;
        startSpawn = true;
        stopspawn = true;
        transition1 = true;
        transition2 = true; 
    }

    private void setSpeed()
    {
        //distance to final point
        float distance = Mathf.Abs(transform.position.y - finalPoint.position.y);
        speed = distance/(TimeWhenStopSpawn-time);
    }

    void countTime()
    {
        time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (start)
        {
            if(!startSpawn)
            {
                setSpeed();
            }
            countTime();
            verifyTime();
            if (move && this.transform.position.y <= finalPoint.position.y)
            {
                this.transform.Translate(0, speed * Time.deltaTime, 0);
            }
        }
    }

    void verifyTime()
    {
        if(time >= TimeWhenStartSpawn && startSpawn)
        {
            startSpawn = false;
            fightMoonManager.StartSpawnning();
            TransitionStartSpawn();
        }

        if(time >= TimeWhenStopSpawn && stopspawn)
        {
            stopspawn = false;
            fightMoonManager.StopSpawnning();
            TransitionStopSpawn();
        }

        if(time >= TimeWhenStopPlataform && transition1)
        {
            transition1 = false;
            Transition1();
        }

        if (time >= TimeWhenStopPlataform2 && transition2)
        {
            transition2 = false;
            Transition2();
        }
    }

    private void Transition1 ()
    {
        move = false;
        //animação de transição
        //troca de musica
        Debug.Log("Transição 1");
    }

    private void Transition2()
    {
        Debug.Log("Transisiton 2");
        MoonTurnRed();
    }

    public void GirlCanMove()
    {
        GirlManager.instance.canMove = true;
    }

    private void TransitionStopSpawn()
    {
        move = false;
        Debug.Log("Parou de Spawnar");
    }

    private void TransitionStartSpawn()
    {
        move = true;
        Debug.Log("Começou a spawnar");
    }

    private void MoonTurnRed()
    {
        moonAnim.SetTrigger("turn red");
    }

    public void GirlTurnIntoEnemy()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Menina"))
        {
            move = true;
            start = true;
            Destroy(this.GetComponent<Collider2D>());
            GirlManager.instance.canMove = false;
            this.GetComponent<Animator>().SetTrigger("ShakePlataform");
            girl.anim.SetBool("Idle", true);
            girl.anim.SetBool("Andando", false);
        }

        if(collision.CompareTag("enemy2"))
        {
            collision.transform.parent = this.transform;
        }
    }












}
