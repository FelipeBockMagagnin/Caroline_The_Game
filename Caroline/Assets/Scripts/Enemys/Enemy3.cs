using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{    

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
        Follow(WhatFollow);
    }


    public override void DestroyThis()
    {
        SpawnHitParticle();
        float forcaTiroAbs = Random.Range(0.5f,1);
        if (WhatFollow.position.x >= transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -5) -1, 2);          
                time = time + (forcaTiroAbs * 2);
                pushed = true;
                counter = true;
                PlayEnemyHitSound();
        }

        if(WhatFollow.position.x < transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +5) +1, 2);
                time = time + (forcaTiroAbs * 2);
                pushed = true;
                counter = true;
                PlayEnemyHitSound();
        }
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
            anim.SetBool("ataque1", false);
        } 
        if(time <= 0 && counter)
        {
            pushed = false;
            counter = false;
            Follow(WhatFollow);            
        }
    }
}
