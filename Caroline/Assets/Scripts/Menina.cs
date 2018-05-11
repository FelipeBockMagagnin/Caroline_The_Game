using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menina : MonoBehaviour {

	bool face = true; //Se virada para direita

    public GameObject FogChao;
    public GameObject Fogfundo;

	public Transform meninaT;
	Animator anim;
	Rigidbody2D rb;

    public Behaviour luz;


	public float VelocidadePulo;
	public float VelCorrendo;
    public bool PodeAndar = true;

    public float time;

    float fallMultiplier = 2.5f;
	float lowjumpmultiplayer = 2f;

	private bool nochao;
	public Transform check;
	public LayerMask OqueEChao;
	float raio = 0.2f;

    public bool MeninaColidiu = false;

    public bool emdialogo;
    

	
	private bool Jumprequest; //Var Pulo contema força aplicada

    //Coisas não usadas
	//Tiro public Transform localTiro; public GameObject SpawnTiro; Pegar pedra do chão public int quantidadePedras; public TextMesh contagemPedras;
    //PEDRAS public GameObject[] PedrasTiro = new GameObject[4]; int NroTiposPedras;
   

	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        VelocidadePulo = 6.2f;
        VelCorrendo = 6.7f;
       
    }

    void Update () {

        if (MeninaColidiu == true)
        {
            time = 1.0f;
            MeninaColidiu = false;
        }

        //Paralização do personagem após impacto
        if (time != 0){
            PodeAndar = false;
            luz.enabled = true;
        } else {
            PodeAndar = true;
            luz.enabled = false;
        }

        if (emdialogo)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", false);
        }

        //garantir que não haja bugs na paralização do personagem
        if (time <= 5 && time > Time.deltaTime){
            time -= Time.deltaTime;
        } else { 
            time = 0;
        }

       




        nochao =  Physics2D.OverlapCircle(check.position, raio, OqueEChao); //Checa se esta no chão


        if (PodeAndar && !emdialogo)
        {
            if (Input.GetKey(KeyCode.RightArrow) && !face)
            {
                Flip();
                //SpawnTiro.transform.Rotate (0, 0, 180);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && face)
            {
                Flip();
                //SpawnTiro.transform.Rotate (0, 0, 180);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && nochao == true)
            {  //se estiver no chão transforma a var nochao em falsa e inicia o script de pulo
                nochao = false;
                Jumprequest = true;
            }

            if (nochao == false)
            {    //Se nochão for false inicia animação de pulo
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", false);
                anim.SetBool("Pulo", true);
            }
            else
            {
                anim.SetBool("Pulo", false);
            }

        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        { //Modificação de Gravidade no pulo
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

        if (PodeAndar && !emdialogo)
        {
            //Movimentação horizontal
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(VelCorrendo * Time.deltaTime, 0, 0);
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
                transform.Translate(-VelCorrendo * Time.deltaTime, 0, 0);
            }
            else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
        }

        //Script de pulo, inplementando força
        if (Jumprequest)
        {
            rb.AddForce(Vector2.up * VelocidadePulo, ForceMode2D.Impulse);
            Jumprequest = false;
        }
    }

	//inverter a escala do personagem fazendo ele girar esq/dir
	void Flip (){
		face = !face;
		Vector3 scale = meninaT.localScale;
		scale.x *= -1;
		meninaT.localScale = scale;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("morte"))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        if (collision.CompareTag("Fog"))
        {
            FogChao.SetActive(true);
            Fogfundo.SetActive(true);
        }
    }   
}