using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menina : MonoBehaviour {

    
    [Header("LUTA")]
    [HideInInspector]
    public bool         apertouEspaco = false;      //saber se errou
           bool         Set_Levantar_true = true;   //muda a var podelevantar por meio de animação no jogo
    [HideInInspector]
    public float        time;                       //tempo a ficar parado  
    
    [Header("MOVIMENTAÇÃO")]
    [HideInInspector]
    public bool         PodeAndar = true;           //movimentação precisa dessa var    
    [HideInInspector]       
    public bool         pararParallar = false;      //evitar paralax de andar enquanto menina trancada em parede
    [HideInInspector]
    public bool         face = true;                //define a movimentação dir/esq invertendo a escala quando chamado
           bool         emdialogo = false;          //define se esta em dialogo
           bool         ClimbDir;                   //define a direção que ira se telopertar apos climb
           bool         ClimbEsq;                   //define a direção que ira se teleprotar apos climb
    [HideInInspector]
    public bool         Gravidade = true;           //climb(), acaba com a movimentação do personagem                      
           float        VelocidadePulo;             //padrão de altura possivel com pulo
           float        VelCorrendo;                //padrão de velocidade possivel com pulo
    [HideInInspector]
    public float        inputVertical;              //contem o input, 1 se dir/-1se esq/0 de parado
           Rigidbody2D  rb;                         //aplicar forças
           Vector3      ClimbPos;                   //localização de onde o personagem ficará apos o climb()
    
    [Header("PULO")]           
    public Transform    check;                      //checar o chao
    public LayerMask    OqueEChao;                  //raycast check ground
    public GameObject   particula_pulo;             //contem a particula liberada ao pular
    [HideInInspector]
    public bool         nochao;                     //ver se esta no chão por meio de raycast   
           float        raio = 0.30f;               //tamanho do raycast
           float        fallMultiplier = 5f;        //modificação da gravidade
           float        lowjumpmultiplayer = 4.3f;  //modificação da gravidade
           bool         Spawn_Particula_Pulo=false; //define se ira spawnar particula durante o pulo
       
    [Header("TIRO")]    
    public GameObject   Pedra;                      //tiro prefab
    public GameObject   Spell_Empurrar;             //spell lançado ao empurrar;
    public Transform    instanciadorPedra;          //onde o tiro sera iniciado
    [HideInInspector]
    public int          quantidadeMunicao;          //quantidade de monição
    [HideInInspector]
    public float        forcaTiro;                  //aumenta quando pressionado control
    [HideInInspector]
    public float        forcaTiroAbsoluta;          //guarda força total do tiro
    [HideInInspector]
    public bool         podeUsarSpeel = true;       //define se o poder spell poderá se spawnado;
 
    [Header("ANIMAÇÕES")]
    Change_camera_atributes cam;
    Animator            anim;                       //animações   

    [Header("UI")]
    GirlUI            GirlUi;

    [HideInInspector]
    public bool empurrando;
    [HideInInspector]
    public bool atirando;

    void Awake(){
        //inicializar ps componentes do jogo
        rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        GirlUi = GetComponent<GirlUI>();
        cam = GetComponent<Change_camera_atributes>();

        //inicializar as variaveis do jogo
        empurrando = false;
        atirando = false;		
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
        if (emdialogo){
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
        Check_Input_Empurrar();

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

    //checar se o botão de empurrar foi pressionado
    void Check_Input_Empurrar(){
        if (Input.GetKeyDown(KeyCode.X) && PodeAndar && podeUsarSpeel && !atirando){
            Vector3 instanciador12 = instanciadorPedra.transform.position;
            Instantiate(Spell_Empurrar, instanciador12,instanciadorPedra.rotation);
            time = 1.5f;
            Set_Levantar_true = true;
            GirlUi.AtivarEmpurrar();        
            anim.SetBool("Empurrar", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Pulo", false);
            anim.SetBool("Andando", false);
            PodeAndar = false;
            empurrando = true;
        }
    }

    //checar se o bitão de levantar foi pressionado no momento certo
    void Check_Input_Levantar(){
        //MOMENTO CERTO
        if (time >= 0.8f && time <= 1f){           
            if (Input.GetKeyDown(KeyCode.X) && !apertouEspaco){
                GirlUi.DesativarUI();                
                rb.velocity = new Vector2(0, 0);
                empurrando = true;
                time = 0;
                anim.SetBool("Levantar", true); 
            }
        
        //MOMENTO ERRADO
        } else if (time <= 1.4f && time >= 1f || time >= 0.2f && time <= 0.8f){            
            if(Input.GetKeyDown(KeyCode.X)){
                apertouEspaco = true;
                GirlUi.DesativarUI();
            }
        }
    }  

    //garantir que não haja bugs na paralização do personagem
    void Calc_time(){        
        if (time <= 5 && time > Time.deltaTime){              
            time -= Time.deltaTime;               
            if(time <= Time.deltaTime && Set_Levantar_true){
                GirlUi.DesativarUI();
                anim.SetBool("Levantar", true); 
                Set_Levantar_true = false;   
                empurrando = false;                     
            }
        }
    }   

    //checar se o input de atirar foi pressionado
    void Input_AtirarPedra(){
        
        //carregar força da pedra se tiver munição e apertando a tecla de empurrar
        if (Input.GetKey(KeyCode.Z) && PodeAndar && quantidadeMunicao>=1 && !empurrando){
            GirlUi.AtivarAtirarPedra();   
            atirando = true;
            if (forcaTiro <= 1){
                forcaTiro = forcaTiro + Time.deltaTime;
            }
        }

        //atirar pedra
        if (Input.GetKeyUp(KeyCode.Z) && quantidadeMunicao >=1 && !empurrando){
            atirarPedra();
            atirando = false;
        }
    }

    //aciona o ato de atirar a pedra
    void atirarPedra(){
        Vector3 instanciador12 = instanciadorPedra.transform.position;
        Instantiate(Pedra, instanciador12,instanciadorPedra.rotation);
        GirlUi.DesativarUI();
        forcaTiroAbsoluta = forcaTiro;
        forcaTiro = 0;
        quantidadeMunicao--;
    }

    //levanta a menina mudando anim e variaveis
    void levantar(){
        anim.SetBool("Puxando", false);
        anim.SetBool("Empurrar", false);
        anim.SetBool("Levantar",false);
        PodeAndar = true;
        apertouEspaco = false;
        empurrando = false;
    }

    //***********************************************************\\
    //METODOS PULO

    //adiciona força para cima e verifica se 2 pulos efetuados
    void Pulo(){
        if (Input.GetKeyDown(KeyCode.UpArrow) && nochao == true){  
            nochao = false;
            rb.AddForce(Vector2.up * VelocidadePulo, ForceMode2D.Impulse); 
            Instantiate(particula_pulo,check.position,Quaternion.identity);
        }
    }

    //seta var particula pulo, ativado/chamado na animação
    void Spawn_Pulo(){
        Spawn_Particula_Pulo = true;
    }

    //spawnar particula de pulo e desativvar variavel
    void Spawn_Particula_Chao(){
        if(nochao && Spawn_Particula_Pulo){
            Spawn_Particula_Pulo = false;
            Instantiate(particula_pulo,check.position,Quaternion.identity);    
            cam.shake();         
        } 
    }

    //Modificação de Gravidade no pulo de acordo com o quanto o usuario pressionou o botao
    void Controle_Gravidade_Pulo(){
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

    //se move na direção do input, positivo ou negativo
    void Move(){
        transform.Translate(inputVertical*Time.deltaTime * VelCorrendo,0,0);
    }

    //seta anim de andar, dir esquera 
    void AnimAndar(){
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)){
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

    //muda a diração do personagem de acordo com o input
    void Flip (){
		face = !face;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

    //controla direção do personagem e a localização do instanciador de pedra
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

    //controla a direção do climb, dir/esq
    void Climb(){
        if (ClimbDir) {
            transform.position = new Vector3(ClimbPos.x + 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            ClimbDir = false;
        } else if (ClimbEsq) {
            transform.position = new Vector3(ClimbPos.x - 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            ClimbEsq = false;
        }

        Spawn_Particula_Pulo = false;
        anim.SetBool("Idle", true);
        anim.SetBool("escalar", false);
        PodeAndar = true;
        Gravidade = true;    
        rb.isKinematic = false; 
    }

    //***********************************************************\\
    //METODOS COLISÃO

    void OnTriggerEnter2D(Collider2D collision){ 
        //morre
        if (collision.CompareTag("morte")){
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
        //coleta munição e aumenta variavel
        if (collision.CompareTag("municao")){
            Destroy(collision.gameObject);
            quantidadeMunicao++;
        }
        //para o parallax
        if(collision.CompareTag("parallax")){
            pararParallar = true;
        }
        //Não pode utilizar feituço nesta area
        if(collision.CompareTag("no_speel_area")){
            podeUsarSpeel = false;
        }

        //Inicia o Climb()
        if (collision.CompareTag("escalar") && PodeAndar){
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
        //não pode usar spell nesta area, ao sair pode
        if(collision.CompareTag("no_speel_area")){
            podeUsarSpeel = true;
        }
        //ao sair pode continuar tendo parallax
        if(collision.CompareTag("parallax")){
            pararParallar = false;
        }
    }
}