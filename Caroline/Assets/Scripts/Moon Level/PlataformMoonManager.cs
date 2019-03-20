using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMoonManager : MonoBehaviour
{
    float speed;
    public bool move;

    public float time;
    public float TimeWhenTransition;
    public float TimeWhenTransition2;
    public float TimeWhenStartSpawn;
    public float TimeWhenStopSpawn;

    public AudioClip Music1;
    public AudioClip TransitionSound;
    public AudioClip Music2;
    public AudioClip Transition2Sound;
    public AudioClip Music3;

    public Animator moonAnim;

    public FightMoonManager fightMoonManager;

    bool start = false;
    bool startSpawn = true;
    bool stopspawn = true;
    bool transition1 = true;
    bool transition2 = true;


    public Transform finalPoint;

    public Girl girl;





    private void Awake()
    {
        speed = 1f;
        move = false;
    }

    void countTime()
    {
        time -= Time.deltaTime;
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
        if(time <= TimeWhenStartSpawn && startSpawn)
        {
            startSpawn = false;
            //fightMoonManager.StartSpawnning();
            TransitionStartSpawn();
        }

        if(time <= TimeWhenStopSpawn && stopspawn)
        {
            stopspawn = false;
            //fightMoonManager.StopSpawnning();
            TransitionStopSpawn();
        }

        if(time <= TimeWhenTransition && transition1)
        {
            transition1 = false;
            Transition1();
        }

        if (time <= TimeWhenTransition2 && transition2)
        {
            transition2 = false;
            Transition2();
        }
    }

    void Transition1 ()
    {
        move = false;
        //animação de transição
        //troca de musica
        Debug.Log("Transição 1");
    }

    void Transition2()
    {
        Debug.Log("Transisiton 2");
        MoonTurnRed();
    }

    public void GirlCanMove()
    {
        girl.canMove = true;
    }

    void TransitionStopSpawn()
    {
        move = false;
        speed = 0;
        Debug.Log("Parou de Spawnar");
    }

    void TransitionStartSpawn()
    {
        move = true;
        speed = 0.5f;
        Debug.Log("Começou a spawnar");
    }

    void MoonTurnRed()
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
