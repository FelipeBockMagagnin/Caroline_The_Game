using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Follow_Atack : MonoBehaviour {

	

    //movimentação
    public  Transform OqueSeguir;   //dar follow
	        float speed;            //contem a velocidade do inimigo
            float escalaX;          //contem escala x do inimigo
	        Vector3 escala;         //contem a escala do inimigo
            float time = 0;         //tempo em que o inimigo fica parado
    public  float maxSpeed = 0;     //
    public  float minSpeed = 0;     //
	       
    //luta
    public  string TagOqueAtacar;   //ao encostar no que tiver essa tag, o inimigo ataca
            bool matar = false;     //se for true no ultimo frame da animação, ele mata
   

    //var importantes       
    public GameObject menina;       //contem a definição de menina
           bool empurrado = false;  //se ele for empurrado não pode atacar
           bool contador;           //define de o tempo continuara descendo
    public bool apenasMenina;       //true = seguir somente menina, false = seguir e atacar saci

    //estetica
            Animator anim;          //contem as animações do inimigo


	void Start(){
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
    //movimentação
    void Seguir(Transform Seguido){
        if(!empurrado){
            if (Seguido.transform.position.x > transform.position.x){
                speed = Random.Range(minSpeed,maxSpeed);
                escala.x = escalaX;
                transform.localScale = escala;
            } else {
                speed = -(Random.Range(minSpeed,maxSpeed));
                escala.x = -escalaX;
                transform.localScale = escala;
            }
        }
    }

    //*************************************************
    //luta

    void Contador_tempo(){
        if(time > 0){
            time = time - Time.deltaTime;
            speed = 0;
            anim.SetBool("ataque1", false);
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        } 
        if(time <= 0 && contador) {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            empurrado = false;
            Seguir(OqueSeguir);
            contador = false;
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
        // menina.GetComponent<Rigidbody2D>().velocity = new Vector2(forca, 0);
    }    

	IEnumerator Tempo_Parado(float time){
        anim.SetBool("ataque1", false);
		speed = 0;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        Seguir(OqueSeguir);
        empurrado = false;
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
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -8) - 2, 2);
                Destroy(collision.gameObject);                
                StartCoroutine(Tempo_Parado((forcaTiroAbs * 5) + 3));
            }

            if(OqueSeguir.position.x <= transform.position.x && menina.transform.position.x <= transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +8) + 2, 2);
                Destroy(collision.gameObject);
                StartCoroutine(Tempo_Parado((forcaTiroAbs * 5) + 3));
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag(TagOqueAtacar)){
            matar = false;
        }
    }   

    private void OnTriggerStay2D(Collider2D collision){        
        //garantir que o inimigo continue atacando caso ele não saia do colisor da Menina
        if(collision.CompareTag("Menina") && apenasMenina && matar){
            Atacar();
        }

        //Empurrar/puxar caso haja somente a menina
        if (collision.CompareTag("Menina") && Input.GetKey(KeyCode.X) && collision.GetComponent<Menina>().PodeAndar == true && !Input.GetKey(KeyCode.Z) && apenasMenina){
            if (collision.transform.position.x >= transform.position.x){                
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f, -4), 1);             //inimigo p/ esq
                collision.GetComponent<Menina>().EmpurrarInimigo();
                empurrado = true;                
                StartCoroutine(Forca_Empurrar_Puxar(3.5f,0.6f));
                time = time + Random.Range(2.5f,3.5f);
                contador = true;                
            }

            if (collision.transform.position.x < transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3f, 4), 1);              //inimigo p/ dir          
                collision.GetComponent<Menina>().EmpurrarInimigo();
                empurrado = true;                
                StartCoroutine(Forca_Empurrar_Puxar(-3.5f,0.6f));
                contador = true;
                time = time + Random.Range(2.5f,3.5f);
            }
        }

        //Empurrar/puxar caso haja o saci e a menina na cena
        if (collision.CompareTag("Menina") && Input.GetKey(KeyCode.X) && (collision.GetComponent<Menina>().PodeAndar == true) && !Input.GetKey(KeyCode.Z) && !apenasMenina){
            //SIM
            //menina direita do saci e direita do inimigo
            if (collision.transform.position.x >= transform.position.x && collision.transform.position.x >= OqueSeguir.transform.position.x){               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5,7), 1);                  //inimigo p/ dir s/ voar
                collision.GetComponent<Menina>().puxarInimigo();
                empurrado = true;               
                StartCoroutine(Forca_Empurrar_Puxar(4.5f,.6f));
                time = time + Random.Range(3,5);
                contador = true;
            }

            //SMI
            //menina direita de saci e esquerda do inimigo 
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x > OqueSeguir.transform.position.x){               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5, 7), 5);             //inimigo p/ dir          
                collision.GetComponent<Menina>().EmpurrarInimigo();
                empurrado = true;
                StartCoroutine(Forca_Empurrar_Puxar(-7f,.6f));
                time = time + Random.Range(3,5);
                contador = true;
            }
            
            //IMS
            //menina esquerda do saci e direita inimigo 
            if (collision.transform.position.x > transform.position.x && collision.transform.position.x < OqueSeguir.transform.position.x){         
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 5);             //inimigo p/ esq                
                collision.GetComponent<Menina>().EmpurrarInimigo();
                empurrado = true;
                StartCoroutine(Forca_Empurrar_Puxar(7f,.6f));
                time = time + Random.Range(3,5);
                contador = true;                
            }

            //MIS
            //menina esquerda do saci e esquerda do inimigo           
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x < OqueSeguir.transform.position.x){           
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 1);              //inimigo p/ esq s/ voar    
                collision.GetComponent<Menina>().puxarInimigo();
                empurrado = true;
                StartCoroutine(Forca_Empurrar_Puxar(-4.5f,.6f));                
                time = time + Random.Range(3,5);
                contador = true;                
            }
        } 
    }
}
