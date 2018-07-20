using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InimigosColidir : MonoBehaviour
{
           float        speed = 4;              //velocidade do inimigo
           float        time;                   //tempo em que ele fica parado
    public bool         meninaColidiu = false;  //
    public bool         esperar = true;
           float        waittime = 0.55f;
           float        escala;
    public Transform    SaciT;
    public GameObject   menina;
           Vector3      scale;
           Animator     anim;

    public bool Acaboudeempurrar = true;

    public bool matar;

    public bool modoAtaqueMenina;

    public GameObject seguido;


    private void Start()
    {
        anim = GetComponent<Animator>();
        scale = transform.localScale;
        escala = scale.x;                   //guarda escala do inimigo
    }

    //coroutines que variam de acordo com a posição da menina
    IEnumerator IMS()
    {
        yield return new WaitForSeconds(waittime);
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(7, 0);    //menina p/ dir
    }

    IEnumerator SIM()
    {
        yield return new WaitForSeconds(waittime);
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(4.5f, 0);   //menina p/ dir fraco
    }

    IEnumerator SMI()
    {
        yield return new WaitForSeconds(waittime);
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);     //menina p/ esq
    }

    IEnumerator MIS()
    {
        yield return new WaitForSeconds(waittime);
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(-4.5f, 0);  //menina p/esq fraco
    }
    
    //ativado sempre que um inimigo for empurrado/puxado
    IEnumerator tempo(){
        speed = 0;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        time = Random.Range(3, 5);
        print(time);
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        speed = 4;
        Acaboudeempurrar = true;
    }

    //ativado sempre que levar um pedrada
    IEnumerator tempoPedra(){
        float forcaTiroAbs = menina.GetComponent<Menina>().forcaTiroAbsoluta;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        speed = 0;
        time = (forcaTiroAbs * 5) + 3;
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        speed = 4;
    }

    void Seguir(GameObject seguido){
        //Movimentação do inimigo e escala ao virar
        if (seguido.transform.position.x > transform.position.x)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            scale.x = escala;
            transform.localScale = scale;
        }
        else
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            scale.x = -escala;
            transform.localScale = scale;
        }
    }

    //movimentação inimigo
    void FixedUpdate()
    {         
        Seguir(seguido);

        //Movimentação do inimigo e escala ao virar
      //  if (SaciT.transform.position.x > transform.position.x)
        //{
          //  transform.Translate(speed * Time.deltaTime, 0, 0);
           // scale.x = escala;
           // transform.localScale = scale;
        //}
        //else
        //{
          //  transform.Translate(-speed * Time.deltaTime, 0, 0);
         //   scale.x = -escala;
          //  transform.localScale = scale;
       // }
    }    


    private void OnTriggerEnter2D(Collider2D other)
    {
        //colisão com a pedra
      if (other.CompareTag("pedra"))
        {
            float forcaTiroAbs = menina.GetComponent<Menina>().forcaTiroAbsoluta;

            if (SaciT.position.x >= transform.position.x && menina.transform.position.x >= transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * -8) - 2, 2);
                Destroy(other.gameObject);                
                StartCoroutine(tempoPedra());
            }

            if(SaciT.position.x <= transform.position.x && menina.transform.position.x <= transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +8) + 2, 2);
                Destroy(other.gameObject);
                StartCoroutine(tempoPedra());
            }           
        }
    }

    //Empurrar e Puchar inimigo
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Se colidir com menina paraliza e recua
        if (collision.CompareTag("Menina") && Input.GetKey(KeyCode.X) && (collision.GetComponent<Menina>().PodeAndar == true) && !Input.GetKey(KeyCode.Z))
        {

            //SIM
            //menina direita do saci e direita do inimigo
            if (collision.transform.position.x >= transform.position.x && collision.transform.position.x >= SaciT.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5,7), 1);                  //inimigo p/ dir s/ voar
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().puxar = true;
                StartCoroutine(SIM());
                    if(Acaboudeempurrar == true){
                    StartCoroutine(tempo());
                    Acaboudeempurrar = false;
                    }
              
            }

            //SMI
            //menina direita de saci e esquerda do inimigo 
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x > SaciT.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5, 7), 5);             //inimigo p/ dir          
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;
                StartCoroutine(SMI());
                    if(Acaboudeempurrar == true){
                        StartCoroutine(tempo());
                        Acaboudeempurrar = false;
                    }
                
            }
            
            //IMS
            //menina esquerda do saci e direita inimigo 
            if (collision.transform.position.x > transform.position.x && collision.transform.position.x < SaciT.transform.position.x)
            {         
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 5);             //inimigo p/ esq
                
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;
                StartCoroutine(IMS());
                    if(Acaboudeempurrar == true){
                    StartCoroutine(tempo());
                    Acaboudeempurrar = false;
                    }
               
            }

            //MIS
            //menina esquerda do saci e esquerda do inimigo           
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x < SaciT.transform.position.x)
            {           
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 1);              //inimigo p/ esq s/ voar    
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().puxar = true;
                StartCoroutine(MIS());
                    if(Acaboudeempurrar == true){
                    StartCoroutine(tempo());
                    Acaboudeempurrar = false;
                    }
            }
        } 
    }

    //não deixa matar se sair do contato
    void OnCollisionExit2D (Collision2D collision){
        if(collision.gameObject.CompareTag("Saci")){          
            matar = false;
        }
    }

    //ativa o ataque
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Saci")){          
        anim.SetBool("ataque1", true);
        speed = 0;
        matar = true;
       
        }
    }

    //checa se pode matar e finaliza a partida
    public void ataque1(){
         anim.SetBool("ataque1", false);
        if(matar == true){
           Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }
}


    
   


