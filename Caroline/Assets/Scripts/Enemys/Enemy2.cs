using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy2 : EnemyFather {

    public float shootSpeed;
    private float distance;
    private bool wasShoot = false;
    [HideInInspector]
    public bool faceRight;
    private float nextShootSpeed;

    private void OnEnable() 
    {
        alertSignal = transform.Find("AlertSignal").gameObject;
        girl = GameObject.Find("Girl");
        girlScript = girl.GetComponent<Girl>();
        scale = transform.localScale;
        scaleX = scale.x;
        anim = GetComponent<Animator>();               
    }

	void FixedUpdate()
    {
        if(!wasShoot)
        {
            Follow();
        }
        else 
        {
            Move(speed);
        }
        
        try
        {
            distance = Vector3.Distance(transform.position, decideWhatToFollow().position);
        }
        catch (UnassignedReferenceException)
        {
            Debug.Log("Don't have char to follow");
        }
        catch (NullReferenceException)
        {
            Debug.Log("Don't have char to follow");
        }
        

        if (distance <= 10.0 && wasShoot == false) 
        {
            attacking = true;
            wasShoot = true;
            anim.SetBool("Atirar", true);
            if (decideWhatToFollow().position.x >= transform.position.x) 
            {
                faceRight = true;
                speed = 0;
                nextShootSpeed = shootSpeed;
                scale = transform.localScale;
                scale.x = Mathf.Abs(scaleX);
                transform.localScale = scale;        
            }
            else
            {
                faceRight = false;
                speed = 0;
                nextShootSpeed = -shootSpeed;
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
            if(nextShootSpeed > 0)
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
