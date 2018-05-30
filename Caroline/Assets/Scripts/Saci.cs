using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Saci : MonoBehaviour {

    public bool             PodeAtacar = true;      //se pode atacar
           float            escala;                 //Guarda a escala do personagem
           float            time;                   //tempo em que fica parado
           bool             embatalha;              //define se se transformará e se pode atacar
    public ParticleSystem   particle;               //particulas placeholder
           Animator         anim;                   //animações
           Vector3          scale;                  //guarda atributos de escala
    public AudioSource      aMatar;                 //audio placeholder

    private void Start()
    {
        anim = GetComponent<Animator>();
        scale = transform.localScale;
        escala = scale.x;
    }
    
    private void Update()
    {    
        //tempo sem atacar
        if (time <= 5 && time >= Time.deltaTime)
        {
            time -= Time.deltaTime;
            embatalha = true;
        }
        else
        {
            time = 0;
            embatalha = false;
        }
        
        //efeito visual que acontece quando n se pode atacar
        if (embatalha)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            PodeAtacar = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        //definir atributos de combate
        if (collision.CompareTag("inimigo") && PodeAtacar)
        {
            anim.SetBool("ModoDemon", true);
            anim.SetTrigger("atacar");
            Destroy(collision.gameObject);
            particle.Play();
            aMatar.Play();
            time = 2;
            PodeAtacar = false;

            //inverter escala
            if (collision.transform.position.x <= transform.position.x)
            {
                scale.x = escala;
                transform.localScale = scale;
            }
            else
            {
                scale.x = -escala;
                transform.localScale = scale;
            }
        }
    }
}
