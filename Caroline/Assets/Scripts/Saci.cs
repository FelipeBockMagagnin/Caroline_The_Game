using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Saci : MonoBehaviour {

    public Behaviour luz;
    public bool PodeAtacar = true;
    public float time;
    Animator anim;
    bool embatalha;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        Vector3 scale = transform.localScale;
        scale.x = 0.7f;
    }
    
    private void Update()
    {
    
        if (time <= 5 && time >= Time.deltaTime)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 0;
        }
        
        if (time != 0)
        {
            luz.enabled = true;            
        }
        else
        {
            
                anim.SetBool("ModoDemon", false);
            
            luz.enabled = false;
            PodeAtacar = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("inimigo") && PodeAtacar)
        {
            anim.SetBool("ModoDemon", true);
            PodeAtacar = false;
            
            Destroy(collision.gameObject,0.05f);               
            time = 2;

            if (collision.transform.position.x <= transform.position.x)
            {
                Vector3 scale = transform.localScale;
                scale.x = 0.7f;
                transform.localScale = scale;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                Vector3 scale = transform.localScale;
                scale.x = -0.7f;
                transform.localScale = scale;
            }
        }
       

    }








}
