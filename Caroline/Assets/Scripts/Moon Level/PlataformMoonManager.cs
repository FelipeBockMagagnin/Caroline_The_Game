using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMoonManager : MonoBehaviour
{
    float speed;
    public bool move;

    public float time;
    public float TimeWhenStopPlataform;
    public float TimeWhenStartSpawn;
    public float TimeWhenStopSpawn;
    public float TimeWhenStopPlataform2;

    public AudioClip Music1;
    public AudioClip Transition1Sound;
    public AudioClip Music2;
    public AudioClip Transition2Sound;
    public AudioClip Music3;

    public Animator moonAnim;
    public FightMoonManager fightMoonManager;

    private bool start = false;
    private bool startSpawn = true;
    private bool stopspawn = true;
    private bool transition1 = true;
    private bool transition2 = true;

    public Transform finalPoint;
    public Girl girl;

    private void Awake()
    {
        speed = 1f;
        move = false;
        time = 0;
    }

    void countTime()
    {
        time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (start)
        {
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
            //fightMoonManager.StartSpawnning();
            TransitionStartSpawn();
        }

        if(time >= TimeWhenStopSpawn && stopspawn)
        {
            stopspawn = false;
            //fightMoonManager.StopSpawnning();
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
        girl.canMove = true;
    }

    private void TransitionStopSpawn()
    {
        move = false;
        speed = 0;
        Debug.Log("Parou de Spawnar");
    }

    private void TransitionStartSpawn()
    {
        move = true;
        speed = 0.5f;
        Debug.Log("Começou a spawnar");
    }

    private void MoonTurnRed()
    {
        moonAnim.SetTrigger("turn red");
    }

    public void GirlTurnIntoEnemy()
    {

    }



    public void EnableSpawnEnemy2()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        move = true;
        start = true;
        if (collision.CompareTag("Menina"))
        {
            Destroy(this.GetComponent<Collider2D>());
            collision.GetComponent<Girl>().canMove = false;
            this.GetComponent<Animator>().SetTrigger("ShakePlataform");
            girl.anim.SetBool("Idle", true);
            girl.anim.SetBool("Andando", false);
        }
    }












}
