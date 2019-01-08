using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : EnemyFather {	        	       

    bool matar = false;                 //se for true no ultimo frame da animação, ele mata     
    
    public  bool apenasMenina;          //true = seguir somente menina, false = seguir e atacar saci    

    //*************************************************
    //iniciais
    //ao ser spawnado ele starta seus componentes
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
        if(!empurrado && time <= 0){ 
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

    void Empurrado(float forca){
        anim.SetBool("ataque1", false);
        SpawnParticulaAtingido();
        empurrado = true;                
        time = time + Random.Range(2.7f,3.5f);
        contador = true;
    }    

    //***********************************************
    //Colisão

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("morte")){
            Destroy(this.gameObject);
        }

        if(collision.CompareTag("turnback")){
            speed = speed * -1;
            escala.x = escala.x * -1;
            transform.localScale = escala;
        }

        //Empurrar/puxar caso haja somente a menina
        if(collision.CompareTag("Spell_Menina_Empurrar")){
            time = Random.Range(2.7f,3.5f);
        }

        if(collision.CompareTag(OqueSeguir.tag)){
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
        if(collision.CompareTag(OqueSeguir.tag)){
            matar = false;
        }

        if(collision.CompareTag("Spell_Menina_Empurrar")){
            empurrado = false;
        }
    }   

    private void OnTriggerStay2D(Collider2D collision){        
        //garantir que o inimigo continue atacando caso ele não saia do colisor da Menina
        if(collision.CompareTag("Menina") && apenasMenina && matar){
            Atacar();
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
