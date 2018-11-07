using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Saci : MonoBehaviour {

    public bool             PodeAtacar = true;      //se pode atacar
           float            escala;                 //Guarda a escala do personagem
    public ParticleSystem   particle;               //particulas placeholder
           Animator         anim;                   //animações
           Vector3          scale;                  //guarda atributos de escala
    public AudioSource      aMatar;                 //audio placeholder
    public float            tempoEntreAtaques;      //armazena o tempo em que o saci ficaram sem atacar 
           bool             atacar = true;          //se true ele pode iniciar a nimação de ataque
    public ParticleSystem   ParticulaTransf;        //

    private void Start(){
        anim = GetComponent<Animator>();
        scale = transform.localScale;
        escala = scale.x;
    }

    public void spawnParticleTranf(){
        Instantiate(ParticulaTransf,transform.position,Quaternion.identity);
    }

    public void Matar(){
        atacar = true;        
    }

    IEnumerator tempo(){
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        PodeAtacar = false;
        tempoEntreAtaques = 2;
        yield return new WaitForSeconds(tempoEntreAtaques);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        PodeAtacar = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision){
        //definir atributos de combate
        if (collision.CompareTag("inimigo") && PodeAtacar){
            StartCoroutine(tempo());
            anim.SetBool("ModoDemon", true);
            anim.SetTrigger("atacar");
           
            if(atacar){
                Destroy(collision.gameObject);
                particle.Play();
                atacar = false;            
            }

            //inverter escala
            if (collision.transform.position.x <= transform.position.x){
                scale.x = escala;
                transform.localScale = scale;
            }
            else{
                scale.x = -escala;
                transform.localScale = scale;
            }
        }
    }
}
