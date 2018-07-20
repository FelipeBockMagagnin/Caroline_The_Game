using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Follow_Atack : MonoBehaviour {

	public Transform OqueSeguir;
	float speed;
	Vector3 escala;
	float escalaX;
    public string TagOqueAtacar;
    Animator anim;
    bool matar = false;
    public GameObject menina;

    public float startSpeed;

    bool empurrado = false;

    bool Acaboudeempurrar;

    public bool apenasMenina; //true = seguir somente menina, false = seguir e atacar saci

	void Start(){
    speed = startSpeed;
	escala = transform.localScale;
	escalaX = escala.x;
	}

	void Update(){
		Seguir(OqueSeguir);
        anim = GetComponent<Animator>();
	}

	void Seguir(Transform Seguido){
        if (Seguido.transform.position.x > transform.position.x)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            escala.x = escalaX;
            transform.localScale = escala;
        }
        else
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            escala.x = -escalaX;
            transform.localScale = escala;
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
            speed = Random.Range(3,5);
        }
        if(matar && !empurrado){
            //perdeu
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }              
    }
    
    IEnumerator Forca_Empurrar_Puxar (float forca, float WaitTime){
        yield return new WaitForSeconds(WaitTime);
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(forca, 0);
    }    

	IEnumerator Tempo_Parado(float time){
		speed = 0;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        speed = Random.Range(3,5);
        empurrado = false;
	}

  
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag(TagOqueAtacar)){
            Atacar();
            matar = true;
        }
          
      //colisão com a pedra
      if (collision.CompareTag("pedra"))
        {
            float forcaTiroAbs = menina.GetComponent<Menina>().forcaTiroAbsoluta;

            if (OqueSeguir.position.x >= transform.position.x && menina.transform.position.x >= transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -8) - 2, 2);
                Destroy(collision.gameObject);                
                StartCoroutine(Tempo_Parado((forcaTiroAbs * 5) + 3));
            }

            if(OqueSeguir.position.x <= transform.position.x && menina.transform.position.x <= transform.position.x)
            {
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

    

    //Empurrar e Puchar inimigo
    private void OnTriggerStay2D(Collider2D collision)
    {

         if (collision.CompareTag("Menina") && Input.GetKey(KeyCode.X) && (collision.GetComponent<Menina>().PodeAndar == true) && !Input.GetKey(KeyCode.Z) && apenasMenina){

            if (collision.transform.position.x >= transform.position.x){
                
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 5);             //inimigo p/ esq
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;

                empurrado = true;
                
                StartCoroutine(Forca_Empurrar_Puxar(7f,.6f));

                StartCoroutine(Tempo_Parado(Random.Range(3,5)));
            }

            if (collision.transform.position.x < transform.position.x){
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5, 7), 5);             //inimigo p/ dir          
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;

                empurrado = true;
                
                StartCoroutine(Forca_Empurrar_Puxar(-7f,.6f));

                StartCoroutine(Tempo_Parado(Random.Range(3,5)));
            }
         }


        //Se colidir com menina paraliza e recua
        if (collision.CompareTag("Menina") && Input.GetKey(KeyCode.X) && (collision.GetComponent<Menina>().PodeAndar == true) && !Input.GetKey(KeyCode.Z) && !apenasMenina)
        {
            //SIM
            //menina direita do saci e direita do inimigo
            if (collision.transform.position.x >= transform.position.x && collision.transform.position.x >= OqueSeguir.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5,7), 1);                  //inimigo p/ dir s/ voar
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().puxar = true;

                empurrado = true;
                
                StartCoroutine(Forca_Empurrar_Puxar(4.5f,.6f));
                  //  if(Acaboudeempurrar == true){

                StartCoroutine(Tempo_Parado(Random.Range(3,5)));
                   // Acaboudeempurrar = false;
                   // }
              
            }

            //SMI
            //menina direita de saci e esquerda do inimigo 
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x > OqueSeguir.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5, 7), 5);             //inimigo p/ dir          
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;

                empurrado = true;

                StartCoroutine(Forca_Empurrar_Puxar(-7f,.6f));
                  //  if(Acaboudeempurrar == true){

                StartCoroutine(Tempo_Parado(Random.Range(3,5)));
                   //     Acaboudeempurrar = false;
                   // }
                
            }
            
            //IMS
            //menina esquerda do saci e direita inimigo 
            if (collision.transform.position.x > transform.position.x && collision.transform.position.x < OqueSeguir.transform.position.x)
            {         
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 5);             //inimigo p/ esq
                
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;

                empurrado = true;

                StartCoroutine(Forca_Empurrar_Puxar(7f,.6f));
                  //  if(Acaboudeempurrar == true){

                StartCoroutine(Tempo_Parado(Random.Range(3,5)));
                  //  Acaboudeempurrar = false;
                  //  }
               
            }

            //MIS
            //menina esquerda do saci e esquerda do inimigo           
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x < OqueSeguir.transform.position.x)
            {           
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 1);              //inimigo p/ esq s/ voar    
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().puxar = true;

                empurrado = true;

                StartCoroutine(Forca_Empurrar_Puxar(-4.5f,.6f));
                  //  if(Acaboudeempurrar == true){

                StartCoroutine(Tempo_Parado(Random.Range(3,5)));
                  //  Acaboudeempurrar = false;
                  //  }
            }
        } 
    }

}
