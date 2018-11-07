using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Follow_Atack : MonoBehaviour {	

    //movimentação
    public  Transform OqueSeguir;   //dar follow
	        float speed;            //contem a velocidade do inimigo
            float escalaX = 0.7f;          //contem escala x do inimigo
	        Vector3 escala;         //contem a escala do inimigo
    public  float time = 0;         //tempo em que o inimigo fica parado
    public  float maxSpeed = 0;     //
    public  float minSpeed = 0;     //
	       
    //luta
    public  string TagOqueAtacar;   //ao encostar no que tiver essa tag, o inimigo ataca
            bool matar = false;     //se for true no ultimo frame da animação, ele mata
   

    //var importantes       
    public GameObject menina;       //contem a definição de menina
           bool empurrado = false;  //se ele for empurrado não pode atacar
           bool contador = false;           //define de o tempo continuara descendo
    public bool apenasMenina;       //true = seguir somente menina, false = seguir e atacar saci

    //estetica
            Animator anim;          //contem as animações do inimigo
    public ParticleSystem particula_atingido;

    private void OnEnable() {
         escala = transform.localScale;
        escalaX = escala.x;
        anim = GetComponent<Animator>();
        Seguir(OqueSeguir);
    }

    void FixedUpdate(){
		transform.Translate(speed * Time.deltaTime, 0, 0);
        Contador_tempo();
	}    

    void corEnemy(float corInicial){
        float cor;
        cor = 1 - corInicial/3.33f;
        GetComponent<SpriteRenderer>().color = new Color(1, cor, cor);    
    }

    //*************************************************
    //movimentação
    void Seguir(Transform Seguido){
        if(!empurrado){
            if (Seguido.transform.position.x >= transform.position.x){
                speed = Random.Range(minSpeed,maxSpeed);
                escala = transform.localScale;
                escala.x = Mathf.Abs(escalaX);
                transform.localScale = escala;        
            } else {
                speed = -(Random.Range(minSpeed,maxSpeed));
                escala = transform.localScale;
                escala.x = -(Mathf.Abs(escalaX));
                transform.localScale = escala;       
            }
        }
    }

    //*************************************************
    //luta

    void Contador_tempo(){
        if(time > 10){
            time = 10;
        }
        if(time > 0){
            time = time - Time.deltaTime;
            speed = 0;
            anim.SetBool("ataque1", false);
            corEnemy(time);
        } 
        if(time <= 0 && contador) {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            empurrado = false;
            contador = false;
            Seguir(OqueSeguir);            
        }
    }

	void Atacar(){
        if(!empurrado){ 
            speed = 0;
            anim.SetBool("ataque1", true);
        }
	}

    void Matar(){
        anim.SetBool("ataque1", false);
        if(!empurrado){
             Seguir(OqueSeguir);
        }
        if(matar && !empurrado){
            //perdeu
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }              
    }

    //*************************************************
    //força e tempo

    IEnumerator Forca_Empurrar_Puxar (float forca, float WaitTime){
        anim.SetBool("ataque1", false);
        yield return new WaitForSeconds(WaitTime);
        //menina.GetComponent<Rigidbody2D>().velocity = new Vector2(forca, 0);
    }    
  

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("morte")){
            Destroy(this.gameObject);
        }

        if(collision.CompareTag("turnback")){
            speed = speed * -1;
            escala.x = escala.x * -1;
            transform.localScale = escala;
        }

        if(collision.CompareTag(TagOqueAtacar)){
            Atacar();
            matar = true;
        }
          
      //colisão com a pedra
        if (collision.CompareTag("pedra")){
            float forcaTiroAbs = menina.GetComponent<Menina>().forcaTiroAbsoluta;
            if (OqueSeguir.position.x >= transform.position.x && menina.transform.position.x >= transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -5) -1, 2);
                collision.GetComponent<ParticleSystem>().loop = false;
                collision.GetComponent<SpriteRenderer>().enabled = false;
                collision.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(collision.gameObject, 0.30f);                
                time = time + (forcaTiroAbs * 5);
                empurrado = true;
                contador = true;
                SpawnParticulaAtingido();
            }

            if(OqueSeguir.position.x <= transform.position.x && menina.transform.position.x <= transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +5) +1, 2);
                collision.GetComponent<ParticleSystem>().loop = false;
                collision.GetComponent<SpriteRenderer>().enabled = false;
                collision.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(collision.gameObject, 0.20f);  
                time = time + (forcaTiroAbs * 5);
                empurrado = true;
                contador = true;
                SpawnParticulaAtingido();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag(TagOqueAtacar)){
            matar = false;
        }
    }   

    void Empurrado(float forca){
        SpawnParticulaAtingido();
        empurrado = true;                
        StartCoroutine(Forca_Empurrar_Puxar(forca,0.6f));
        time = time + Random.Range(2.7f,3.5f);
        contador = true;
    }

    void SpawnParticulaAtingido(){
        Instantiate(particula_atingido, transform.position, Quaternion.identity);
    }

    private void OnTriggerStay2D(Collider2D collision){        
        //garantir que o inimigo continue atacando caso ele não saia do colisor da Menina
        if(collision.CompareTag("Menina") && apenasMenina && matar){
            Atacar();
        }

        //Empurrar/puxar caso haja somente a menina
        if(collision.CompareTag("Spell_Menina_Empurrar")){
            time = Random.Range(2.7f,3.5f);
        }

        if (collision.CompareTag("Spell_Menina_Empurrar") && apenasMenina && !empurrado){
            if (menina.transform.position.x >= transform.position.x){                
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f, -4), 1);             //inimigo p/ esq
                Empurrado(3.5f);               
            }

            if (collision.transform.position.x < transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3f, 4), 1);              //inimigo p/ dir          
                Empurrado(-3.5f);
            }
        }

        if (collision.CompareTag("Spell_Menina_Empurrar") && !apenasMenina && !empurrado){
            if (OqueSeguir.transform.position.x >= transform.position.x){                
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f, -4), 1);             //inimigo p/ esq
                Empurrado(3.5f);                
            }

            if (OqueSeguir.transform.position.x < transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3f, 4), 1);              //inimigo p/ dir          
                Empurrado(-3.5f);
            }
        }
    }
}
