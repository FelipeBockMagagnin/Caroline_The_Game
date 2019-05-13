using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyFather {

    public float shootSpeed;
    float distance;
    bool wasShoot = false;
    public bool faceRight;


    private void OnEnable() 
    {
        girl = GameObject.Find("Girl");
        if(WhatFollow == null)
        {
            WhatFollow = girl.transform;
        }
        scale = transform.localScale;
        scaleX = scale.x;
        anim = GetComponent<Animator>();               
    }

    protected override void Follow(Transform followObject)
    {
        if(!wasShoot)
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
        }
    }

    void Move()
    {
        if(!attacking)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }        
    }

	void FixedUpdate()
    {
        Follow(WhatFollow);
        Move();
        distance = Vector3.Distance(transform.position, girl.transform.position);

        if (distance <= 10.0 && wasShoot == false) 
        {
            attacking = true;
            wasShoot = true;
            anim.SetBool("Atirar", true);
            if (girl.transform.position.x >= transform.position.x) 
            {
                faceRight = true;
                speed = 0;
                scale = transform.localScale;
                scale.x = Mathf.Abs(scaleX);
                transform.localScale = scale;        
            }
            else
            {
                faceRight = false;
                speed = 0;
                scale = transform.localScale;
                scale.x = -(Mathf.Abs(scaleX));
                transform.localScale = scale;       
            }
        }        
	}   

    void Shoot()
    {
        attacking = false;
        if (wasShoot)
        {            
            if (girl.transform.position.x >= transform.position.x)
            {
                speed = shootSpeed;
                scale = transform.localScale;
                scale.x = Mathf.Abs(scaleX);
                transform.localScale = scale;
            }
            else
            {
                speed = -shootSpeed;
                scale = transform.localScale;
                scale.x = -(Mathf.Abs(scaleX));
                transform.localScale = scale;
            }
        }
    }

    public void disableRigidBody()
    {
        this.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("morte"))
        {
            girl.GetComponent<Girl>().DetachCarolineChildren();
            SpawnHitParticle();
            Destroy(this.gameObject);
        }
    }
}
