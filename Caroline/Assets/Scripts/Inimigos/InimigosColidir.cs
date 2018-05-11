using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InimigosColidir : MonoBehaviour
{

    public Transform SaciT;
    public float speed = 2.5f;
    float time;

    public bool meninaColidiu = false;


    private void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x = 0.5f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }


        //Paralização do personagem após impacto
        if (time != 0)
        {
            speed = 0;
        }
        else
        {
            speed = 4.0f;
        }

        //garantir que não haja bugs na paralização do personagem
        if (time <= 5 && time > Time.deltaTime)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 0;
        }



        //Movimentação do inimigo
        if (SaciT.transform.position.x > transform.position.x)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            Vector3 scale = transform.localScale;
            scale.x = 0.7f;
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            scale.x = -0.7f;
            transform.localScale = scale;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Se colidir com menina paraliza e recua
        if (collision.CompareTag("Menina") && Input.GetKey(KeyCode.Space))
        {
            //collision = menina , script = inimigo , SaciT = Saci 
            

            //menina direita do saci e direita do inimigo 
            if (collision.transform.position.x > transform.position.x && collision.transform.position.x > SaciT.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5,7), 1);                  //inimigo p/ dir s/ voar
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(3.5f, 0);    //menina p/ dir fraco
                collision.GetComponent<Menina>().MeninaColidiu = true;
                time = Random.Range(4, 6);
            }

            //menina direita de saci e esquerda do inimigo 
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x > SaciT.transform.position.x)
            {               
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5, 7), 5);             //inimigo p/ dir
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);  //menina p/ esq
                collision.GetComponent<Menina>().MeninaColidiu = true;
                time = Random.Range(4, 6);
            }
            
            //menina esquerda do saci e direita inimigo 
            if (collision.transform.position.x > transform.position.x && collision.transform.position.x < SaciT.transform.position.x)
            {         
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 5);             //inimigo p/ esq
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(7, 0);    //menina p/ dir
                collision.GetComponent<Menina>().MeninaColidiu = true;
                time = Random.Range(4, 6);
            }

            //menina esquerda do saci e esquerda do inimigo 
            if (collision.transform.position.x < transform.position.x && collision.transform.position.x < SaciT.transform.position.x)
            {           
                GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, -7), 1);              //inimigo p/ esq s/ voar
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(-3.5f, 0);   //menina p/esq fraco
                collision.GetComponent<Menina>().MeninaColidiu = true;
                time = Random.Range(4, 6);
            }









        }        
            


            

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Saci"))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }
}
    
   


