using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{    

    private bool canBeKilled = false;
    private Color initialColor;

    private void OnEnable() {
        alertSignal = transform.Find("AlertSignal").gameObject;
        initialColor = this.GetComponent<SpriteRenderer>().color;
        girl = GameObject.Find("Girl");
        girlScript = girl.GetComponent<Girl>();
        scale = transform.localScale;
        scaleX = scale.x;
        anim = GetComponent<Animator>();
        canBeKilled = false;

        try
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        catch (System.Exception)
        {
        }
    }

    public override void DestroyThis(Collider2D collision)
    {
        if(collision.CompareTag("Saci"))
        {
            float forcaTiroAbs = Random.Range(0.5f,1);
            if (decideWhatToFollow().position.x >= transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -5) -1, 2);          
                Pushed(forcaTiroAbs*3);
            }
            else if(decideWhatToFollow().position.x < transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +5) +1, 2);
                Pushed(forcaTiroAbs*3);                
            }
        }

        if(collision.CompareTag("Spell_Menina_Empurrar"))
        {
            if(canBeKilled)
            {
                SpawnHitParticle();
                Destroy(this.gameObject);
            }
            else 
            {
                float forcaTiroAbs = Random.Range(0.5f,1);
                if (decideWhatToFollow().position.x >= transform.position.x)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -5) -1, 2);          
                    anim.SetBool("ataque1", false);
                    SpawnHitParticle();
                    PlayEnemyHitSound();
                    pushed = true;                
                    counter = true; 
                }
                else if(decideWhatToFollow().position.x < transform.position.x)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +5) +1, 2);  
                    anim.SetBool("ataque1", false);
                    SpawnHitParticle();
                    PlayEnemyHitSound();
                    pushed = true;                
                    counter = true;              
                }
            }
        }        
    }

    /// <summary>
    /// finalize the animation and make the enemy stop for a litle time
    /// </summary>
    void Pushed(float force)
    {
        anim.SetBool("ataque1", false);
        SpawnHitParticle();
        PlayEnemyHitSound();
        pushed = true;                
        time = time + force * 2;
        counter = true;
    }

    protected override void TimeCount()
    {
        if(time > 10){
            time = 10;
        }
        if(time > 0)
        {
            time = time - Time.deltaTime;
            speed = 0;
            canBeKilled = true;
            anim.SetBool("ataque1", false);
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
        } 
        if(time <= 0 && counter)
        {
            pushed = false;
            counter = false;   
            canBeKilled = false;  
            this.GetComponent<SpriteRenderer>().color = initialColor;     
        }
    }
}
