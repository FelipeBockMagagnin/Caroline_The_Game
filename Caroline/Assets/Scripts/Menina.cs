using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menina : MonoBehaviour {

    //LUTA
    public float        time;                       //tempo a ficar parado
    public bool         Empurrou = false;           //define o inicio do ato de empurrar
           bool         apertouEspaco = false;      //saber se errou
    public GameObject   uiEmpurrar;                 //demonstração do tempo


    //MOVIMENTAÇÃO
    public bool         PodeAndar = true;           //movimentação precisa dessa var
    public bool         emdialogo = false;          //define se esta em dialogo
           bool         face = true;                //define a movimentação dir/esq invertendo a escala quando chamado
           float        VelocidadePulo;             //padrão de altura possivel com pulo
           float        VelCorrendo;                //padrão de velocidade possivel com pulo
           Rigidbody2D  rb;                         //aplicar forças

    //PULO
           bool         Jumprequest;                //contem atributos de pulo
    public bool         nochao;                     //ver se esta no chão por meio de raycast
    public Transform    check;                      //checar o chao
    public LayerMask    OqueEChao;                  //raycast check ground
           float        raio = 0.2f;                //tamanho do raycast
           float        fallMultiplier = 2.5f;      //modificação da gravidade
           float        lowjumpmultiplayer = 2f;    //modificação da gravidade


    //TIRO
    public GameObject   uiAtirar;                   //representação da força do tiro
           float        forcaTiro;                  //aumenta quando pressionado control
    public float        forcaTiroAbsoluta;          //guarda força total do tiro
    public GameObject   Pedra;                      //tiro prefab
    public Transform    instanciadorPedra;          //onde o tiro sera iniciado
           int          quantidadeMunicao;          //quantidade de monição

    //ANIMAÇÃO E PRA BONITO
           Animator     anim;                       //animações   
    public AudioSource  aAtirarpedra;               //audio placeholder
    public AudioSource  aAcertarlevantar;           //audio placeholder
    public AudioSource  aErrarlevantar;             //audio placeholder


    //start nos componentes
    void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        VelocidadePulo = 6.2f;
        VelCorrendo = 8; 
    }
    
    //Contem input de movimentação, animação
    void FixedUpdate () {

        //garantir que não haja bugs na paralização do personagem
        if (time <= 5 && time > Time.deltaTime)
        {          
            time -= Time.deltaTime;            
            if(time <= 0.4f)
            {
                uiEmpurrar.SetActive(false);
                anim.SetBool("Levantar", true);               
            }
            else
            {
                anim.SetBool("Levantar", false);
            }
        }
        else
        {            
            time = 0;
        }

        //Paralização do personagem após impacto
        if (time != 0){
            PodeAndar = false;
            anim.SetBool("Idle", false);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", false);

            //empurrar
            if (Empurrou)
            {
                anim.SetBool("Empurrar", true);
                anim.SetBool("Idle", false);
                anim.SetBool("Pulo", false);
                anim.SetBool("Andando", false);
                Empurrou = false;
                uiEmpurrar.SetActive(true);               
            }
        }
        else
        {
            anim.SetBool("Empurrar", false);
            PodeAndar = true;
            apertouEspaco = false;
        }       

        //Controla animação quando parada
        if (emdialogo)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", false);
        }

        //controla a possibilidade de flip() e animações quando parada ou pulando e mov dir e esq
        if (PodeAndar && !emdialogo)
        {
            if (Input.GetKey(KeyCode.RightArrow) && !face)
            {
                Flip();
                instanciadorPedra.Rotate(0, -180, 0);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && face)
            {
                Flip();
                instanciadorPedra.Rotate(0, 180, 0);               
            }

            //Movimentação dir e esquerda
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(VelCorrendo * Time.deltaTime, 0, 0);
                if (nochao)
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Andando", true);
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                if (nochao)
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Andando", true);
                }
                transform.Translate(-VelCorrendo * Time.deltaTime, 0, 0);
            }
            else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                if (nochao)
                {
                    anim.SetBool("Idle", true);
                    anim.SetBool("Andando", false);
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                if (nochao)
                {
                    anim.SetBool("Idle", true);
                    anim.SetBool("Andando", false);
                }
            }
        }

        //Modificação de Gravidade no pulo de acordo com o quando o usuario pressionou o botao
        if (rb.velocity.y < 0)
        { 
        rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow))
        {
            rb.gravityScale = lowjumpmultiplayer;
        }
        else
        {
            rb.gravityScale = 1f;
        }      
    }

    //Pulo precisa ficar em update por precisar checar cada frame
    private void Update()
    {
        //carregar força da pedra se tiver munição
        if (Input.GetKey(KeyCode.LeftAlt) && PodeAndar && quantidadeMunicao>=1)
        {
            uiAtirar.SetActive(true);
            if (forcaTiro <= 1)
            {
                forcaTiro = forcaTiro + Time.deltaTime;
            }
        }

        //atirar pedra
        if (Input.GetKeyUp(KeyCode.LeftAlt) && PodeAndar && quantidadeMunicao >=1)
        {
            aAtirarpedra.Play();
            Vector3 instanciador12 = instanciadorPedra.transform.position;
            Instantiate(Pedra, instanciador12,instanciadorPedra.rotation);
            uiAtirar.SetActive(false);
            forcaTiroAbsoluta = forcaTiro;
            forcaTiro = 0;
            quantidadeMunicao--;
        }

        //empurrar
        if (time >= 0.8f && time <= 1f)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !apertouEspaco)
            {
                aAcertarlevantar.Play();
                time = 0.2f;
                rb.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space) && !PodeAndar)
            {
                aErrarlevantar.Play();
                apertouEspaco = true;
                uiEmpurrar.SetActive(false);
            }
        }

        //Se nochão for false inicia animação de pulo
        if (nochao == false && !Empurrou)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", true);
        }
        else
        {
            anim.SetBool("Pulo", false);
        }

        //Checar se esta no chao
        nochao = Physics2D.OverlapCircle(check.position, raio, OqueEChao);

        //Script de pulo, inplementando força
        if (Jumprequest)
        {
            rb.AddForce(Vector2.up * VelocidadePulo, ForceMode2D.Impulse);
            Jumprequest = false;
        }

        //Script de movimentação dir e esq
        if (PodeAndar && !emdialogo)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && nochao == true)
            {  //se estiver no chão transforma a var nochao em falsa e inicia o script de pulo
                nochao = false;
                Jumprequest = true;
            }
        }

        //Reset de Cena
        if (Input.GetKey(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }

    //inverter a escala do personagem fazendo ele girar esq/dir
    void Flip (){
		face = !face;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

    //morte
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("morte"))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        if (collision.CompareTag("municao"))
        {
            Destroy(collision.gameObject);
            quantidadeMunicao++;
        }
    }   
}