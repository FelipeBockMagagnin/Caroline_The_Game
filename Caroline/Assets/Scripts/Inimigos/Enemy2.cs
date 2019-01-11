using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyFather {

    public float shootSpeed;
    float distance;
    bool wasShoot = false;


    private void OnEnable() {
        scale = transform.localScale;
        scaleX = scale.x;
        anim = GetComponent<Animator>();
        Follow(WhatFollow);
    }

    /// <summary>
    /// 
    /// </summary>
    void Move()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

	void FixedUpdate()
    {
        Move();

        distance = Vector3.Distance(transform.position, girl.transform.position);

        if (distance <= 10.0 && wasShoot == false) 
        {
            wasShoot = true;
            anim.SetBool("Atirar", true);
            if (girl.transform.position.x >= transform.position.x) 
            {
                speed = 0;
                scale = transform.localScale;
                scale.x = Mathf.Abs(scaleX);
                transform.localScale = scale;        
            }
            else
            {
                speed = 0;
                scale = transform.localScale;
                scale.x = -(Mathf.Abs(scaleX));
                transform.localScale = scale;       
            }
        }
        
	}   


    /// <summary>
    /// 
    /// </summary>
    void Shoot()
    {
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


}
