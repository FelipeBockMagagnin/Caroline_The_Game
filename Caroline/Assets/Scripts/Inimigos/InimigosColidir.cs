using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InimigosColidir : MonoBehaviour
{
           float        speed;
           float        time;
    public bool         meninaColidiu = false;
    public bool         esperar = true;
           float        waittime;
           float        escala;
    public Transform    SaciT;
    public GameObject   menina;
           Vector3      scale;

    private void Start()
    {
        waittime = 0.3f;
        scale = transform.localScale;
        escala = scale.x;      
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
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(3.5f, 0);    //menina p/ dir fraco
    }

    IEnumerator SMI()
    {
        yield return new WaitForSeconds(waittime);
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);  //menina p/ esq
    }

    IEnumerator MIS()
    {
        yield return new WaitForSeconds(waittime);
        menina.GetComponent<Rigidbody2D>().velocity = new Vector2(-3.5f, 0);   //menina p/esq fraco
    }
    
    //movimentação inimigo
    void FixedUpdate()
    {        
        //Paralização do personagem após impacto
        if (time != 0)
        {
            speed = 0;
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            speed = 4.0f;
        }

        //garantir que não haja bugs na paralização do personagem
        if (time > Time.deltaTime)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 0;
        }
        
        //Movimentação do inimigo e escala ao virar
        if (SaciT.transform.position.x > transform.position.x)
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
                time = (forcaTiroAbs * 5) + 3;
            }

            if(SaciT.position.x <= transform.position.x && menina.transform.position.x <= transform.position.x)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2((forcaTiroAbs * +8) + 2, 2);
                Destroy(other.gameObject);
                time = (forcaTiroAbs * 5) + 3;
            }           
        }
    }



    //Empurrar e Puchar inimigo
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Se colidir com menina paraliza e recua
        if (collision.CompareTag("Menina") && Input.GetKey(KeyCode.Space) && (collision.GetComponent<Menina>().PodeAndar == true) && !Input.GetKey(KeyCode.LeftAlt))
        {

            //SIM
            //menina direita do saci e direita do inimigo
            if (collision.transform.position.x > transform.position.x && collision.transform.position.x > SaciT.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5,7), 1);                  //inimigo p/ dir s/ voar
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;
                time = Random.Range(3, 5);
                StartCoroutine(SIM());
            }

            //SMI
            //menina direita de saci e esquerda do inimigo 
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x > SaciT.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5, 7), 5);             //inimigo p/ dir
                
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;
                time = Random.Range(3, 5);
                StartCoroutine(SMI());



            }
            
            //IMS
            //menina esquerda do saci e direita inimigo 
            if (collision.transform.position.x > transform.position.x && collision.transform.position.x < SaciT.transform.position.x)
            {         
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 5);             //inimigo p/ esq
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;
                time = Random.Range(3, 5);

                StartCoroutine(IMS());
                


            }

            //MIS
            //menina esquerda do saci e esquerda do inimigo           
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x < SaciT.transform.position.x)
            {           
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 1);              //inimigo p/ esq s/ voar
                collision.GetComponent<Menina>().time = 1.5f;
                collision.GetComponent<Menina>().Empurrou = true;
                time = Random.Range(3, 5);
                StartCoroutine(MIS());



            }
        }          
    }

    //Lose Game
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Saci"))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }
}
    
   


