using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyFather {

    public float shootSpeed;

	private void OnEnable() {
        scale = transform.localScale;
        scaleX = scale.x;
        anim = GetComponent<Animator>();
        Follow(WhatFollow);
    }

	void FixedUpdate()
    {
		transform.Translate(speed * Time.deltaTime, 0, 0);

        if(Input.GetKeyDown(KeyCode.T))
        {
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

    void Shoot()
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
