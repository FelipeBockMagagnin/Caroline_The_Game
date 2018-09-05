using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menina : MonoBehaviour {

    
    [Header("LUTA")]
           bool         apertouEspaco = false;      //saber se errou
    public bool         Set_Levantar_true = true;   //muda a var podelevantar por meio de animação no jogo
           float        time;                       //tempo a ficar parado  
    public GameObject   uiEmpurrar;                 //demonstração do tempo  
    
    [Header("MOVIMENTAÇÃO")]
    public bool         PodeAndar = true;           //movimentação precisa dessa var           
    public bool         pararParallar = false;      //evitar paralax de andar enquanto menina trancada em parede
           bool         face = true;                //define a movimentação dir/esq invertendo a escala quando chamado
           bool         emdialogo = false;          //define se esta em dialogo
           bool         ClimbDir;                   //define a direção que ira se telopertar apos climb
           bool         ClimbEsq;                   //define a direção que ira se teleprotar apos climb
           bool         Gravidade = true;           //climb(), acaba com a movimentação do personagem                      
           float        VelocidadePulo;             //padrão de altura possivel com pulo
           float        VelCorrendo;                //padrão de velocidade possivel com pulo
           public float inputVertical;              //contem o input, 1 se dir/-1se esq/0 de parado
           Rigidbody2D  rb;                         //aplicar forças
           Vector3      ClimbPos;                   //localização de onde o personagem ficará apos o climb()
    
    [Header("PULO")]           
    public Transform    check;                      //checar o chao
    public LayerMask    OqueEChao;                  //raycast check ground
    public GameObject   particula_pulo;             //contem a particula liberada ao pular
           bool         nochao;                     //ver se esta no chão por meio de raycast   
           float        raio = 0.30f;               //tamanho do raycast
           float        fallMultiplier = 5f;        //modificação da gravidade
           float        lowjumpmultiplayer = 4.3f;  //modificação da gravidade
           bool         Spawn_Particula_Pulo=false; //define se ira spawnar particula durante o pulo
       
    [Header("TIRO")]    
    public GameObject   uiAtirar;                   //representação da força do tiro
    public GameObject   Pedra;                      //tiro prefab
    public Transform    instanciadorPedra;          //onde o tiro sera iniciado
           int          quantidadeMunicao;          //quantidade de monição
           float        forcaTiro;                  //aumenta quando pressionado control
    public float        forcaTiroAbsoluta;          //guarda força total do tiro
 
    [Header("ANIMAÇÕES")]
    public Animator     Cam;                        //animação de camera
           Animator     anim;                       //animações   
    

    void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        VelocidadePulo = 9f;
        VelCorrendo = 7.5f; 
    }
   
    void FixedUpdate (){

        if(PodeAndar && !emdialogo){
            Move();
            Controle_Flip(); 
        }

        Spawn_Particula_Chao();        
        Controle_Gravidade_Pulo();
        Calc_time();        

        //Controla animação quando parada
        if (emdialogo)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", false);
        }
    }  

    private void Update(){
       
        if(PodeAndar && !emdialogo){
            Pulo();
            AnimAndar();
        }   

        Check_Input_Levantar();
        Input_AtirarPedra();

        //Checar se esta no chao
        nochao = Physics2D.OverlapCircle(check.position, raio, OqueEChao); 

        //movimentação
        inputVertical = Input.GetAxisRaw("Horizontal");        
        
        //Se nochão for false inicia animação de pulo
        if (nochao == false && PodeAndar){
            anim.SetBool("Idle", false);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", true);
        } else {
            anim.SetBool("Pulo", false);
        }        

        //Reset de Cena
        if (Input.GetKeyDown(KeyCode.R)){
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        if(Gravidade == false){
            rb.velocity = Vector3.zero;
        }
    }

    //*******************************************************************\\
    //METODOS DE BATALHA

    void Check_Input_Levantar(){
        if (time >= 0.8f && time <= 1f){           
            if (Input.GetKeyDown(KeyCode.X) && !apertouEspaco){
                uiEmpurrar.SetActive(false);
                rb.velocity = new Vector2(0, 0);
                time = 0;
                anim.SetBool("Levantar", true); 
            }

        } else if (time <= 1.4f && time >= 1f || time >= 0.2f && time <= 0.8f){            
            if(Input.GetKeyDown(KeyCode.X)){
                apertouEspaco = true;
                uiEmpurrar.SetActive(false);
            }
        }
    }  

    void Calc_time(){
        //garantir que não haja bugs na paralização do personagem
        if (time <= 5 && time > Time.deltaTime){          
            
            time -= Time.deltaTime;            
            
            if(time <= Time.deltaTime && Set_Levantar_true){
                uiEmpurrar.SetActive(false);
                anim.SetBool("Levantar", true); 
                Set_Levantar_true = false;                        
            }
        }
    }   

    void Input_AtirarPedra(){
        //carregar força da pedra se tiver munição
        if (Input.GetKey(KeyCode.Z) && PodeAndar && quantidadeMunicao>=1)
        {
            uiAtirar.SetActive(true);
            if (forcaTiro <= 1)
            {
                forcaTiro = forcaTiro + Time.deltaTime;
            }
        }

        //atirar pedra
        if (Input.GetKeyUp(KeyCode.Z) && PodeAndar && quantidadeMunicao >=1){
            atirarPedra();
        }
    }

    void atirarPedra(){
        Vector3 instanciador12 = instanciadorPedra.transform.position;
        Instantiate(Pedra, instanciador12,instanciadorPedra.rotation);
        uiAtirar.SetActive(false);
        forcaTiroAbsoluta = forcaTiro;
        forcaTiro = 0;
        quantidadeMunicao--;
    }

    public void EmpurrarInimigo(){
        
        time = 1.5f;
        Set_Levantar_true = true;
        uiEmpurrar.SetActive(true);
        anim.SetBool("Empurrar", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Pulo", false);
        anim.SetBool("Andando", false);
        PodeAndar = false;
    }

    public void puxarInimigo(){
        
        time = 1.5f;
        uiEmpurrar.SetActive(true);
        anim.SetBool("Puxando", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Pulo", false);
        anim.SetBool("Andando", false);
        PodeAndar = false;   
        Set_Levantar_true = true;
    }

    void levantar(){
        anim.SetBool("Puxando", false);
        anim.SetBool("Empurrar", false);
        anim.SetBool("Levantar",false);
        PodeAndar = true;
        apertouEspaco = false;
    }

    //***********************************************************\\
    //METODOS PULO

    void Pulo(){
        if (Input.GetKeyDown(KeyCode.UpArrow) && nochao == true){  
            nochao = false;
            rb.AddForce(Vector2.up * VelocidadePulo, ForceMode2D.Impulse); 
            Instantiate(particula_pulo,check.position,Quaternion.identity);
        }
    }

    void Spawn_Pulo(){
        Spawn_Particula_Pulo = true;
    }

    void Spawn_Particula_Chao(){
        if(nochao && Spawn_Particula_Pulo){
            Spawn_Particula_Pulo = false;
            Instantiate(particula_pulo,check.position,Quaternion.identity);    
            Cam.SetTrigger("shake");          
        } 
    }

    void Controle_Gravidade_Pulo(){
        //Modificação de Gravidade no pulo de acordo com o quando o usuario pressionou o botao
        if (rb.velocity.y < 0 && Gravidade){ 
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow)){
            rb.gravityScale = lowjumpmultiplayer;
        }
        else{
            rb.gravityScale = 2f;
        }      
    }

    //***********************************************************\\
    //METODOS MOVIMENTAÇÃO

    void Move(){
        transform.Translate(inputVertical*Time.deltaTime * VelCorrendo,0,0);
    }

    void AnimAndar(){
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)){
          // transform.Translate(VelCorrendo * Time.deltaTime, 0, 0);
            if (nochao){
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)){
            if (nochao){
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
            }
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)){
            if (nochao){
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)){
            if (nochao){
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
        }
    }

    void Flip (){
		face = !face;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

    void Controle_Flip(){
        if (Input.GetKey(KeyCode.RightArrow) && !face){
            Flip();
            instanciadorPedra.Rotate(0, -180, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && face){
            Flip();
            instanciadorPedra.Rotate(0, 180, 0);               
        }        
    }   

    void Climb(){
        if (ClimbDir) {
            transform.position = new Vector3(ClimbPos.x + 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            ClimbDir = false;
        } else if (ClimbEsq) {
            transform.position = new Vector3(ClimbPos.x - 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            ClimbEsq = false;
        }
        anim.SetBool("Idle", true);
        anim.SetBool("escalar", false);
        PodeAndar = true;
        Gravidade = true;   
        rb.isKinematic = false; 
    }

    //***********************************************************\\
    //METODOS COLISÃO

    void OnTriggerEnter2D(Collider2D collision){ 
        if (collision.CompareTag("morte"))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        if (collision.CompareTag("municao"))
        {
            Destroy(collision.gameObject);
            quantidadeMunicao++;
        }

        if(collision.CompareTag("parallax")){
            pararParallar = true;
        }

        //Inicia o Climb()
        if (collision.CompareTag("escalar")){
            Gravidade = false;
            PodeAndar = false; 
            rb.isKinematic = true;
            ClimbPos = GetComponent<Transform>().position;
            anim.SetBool("escalar", true);
            anim.SetBool("Pulo", false);
            anim.SetBool("Idle", false);   
            if(collision.transform.position.x >= transform.position.x){
                ClimbDir = true;
            } else {
                ClimbEsq = true;
            }
        }
    }   

    void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag("parallax")){
            pararParallar = false;
        }
    }
}