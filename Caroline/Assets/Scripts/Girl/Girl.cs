using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Girl : MonoBehaviour {

    
    //FIGHT ATTRIBUTES
    [HideInInspector]
    public  bool        pressedSpace;                   //saber se errou
            bool        riseGirl;                       //muda a var podelevantar por meio de animação no jogo
    [HideInInspector]
    public  float       time;                           //tempo a ficar parado  
    [HideInInspector]
    public  bool        pushing;                        //define se esta no ato de empurrar
    [HideInInspector]
    public  bool        shooting;                       //define se esta no ato de atirar

    //MOVIMENTATION ATTRIBUTES
    [HideInInspector]
    public  bool        canMove;                        //movimentação precisa dessa var    
    [HideInInspector]       
    public  bool        stopParallax;                   //evitar paralax de andar enquanto menina trancada em parede
    [HideInInspector]
    public  bool        face;                           //define a movimentação dir/esq invertendo a escala quando chamado
            bool        inDialogue;                     //define se esta em dialogo
            bool        ClimbRight;                     //define a direção que ira se telopertar apos climb
            bool        ClimbLeft;                      //define a direção que ira se teleprotar apos climb
    [HideInInspector]
    public  bool        gravity;                        //climb(), acaba com a movimentação do personagem                      
            float       jumpVelocity;                   //padrão de altura possivel com pulo
            float       runVelocity;                    //padrão de velocidade possivel com pulo
    [HideInInspector]
    public  float       verticalInput;                  //contem o input, 1 se dir/-1se esq/0 de parado
            Rigidbody2D rb;                             //aplicar forças
            Vector3     ClimbPos;                       //localização de onde o personagem ficará apos o climb()
    
    //JUMO ATRRIBUTES          
    public  Transform   check;                          //checar o chao
    public  LayerMask   whatIsGround;                   //raycast check ground
    public  GameObject  jumpParticle;                   //contem a particula liberada ao pular
    [HideInInspector]
    public  bool        inGround;                       //ver se esta no chão por meio de raycast   
            float       radius;                         //tamanho do raycast
            float       fallMultiplier;                 //modificação da gravidade
            float       lowjumpmultiplayer;             //modificação da gravidade
            bool        spawnJumpParticle;              //define se ira spawnar particula durante o pulo
       
    //SHOOTING ATTRIBUTES    
    public  GameObject  rock;                           //tiro prefab    
    public  Transform   rockInstancePosition;           //onde o tiro sera iniciado
    [HideInInspector]
    public  int         ammunition;                     //quantidade de monição
    [HideInInspector]
    public  float       shootingForce;                  //aumenta quando pressionado control
    [HideInInspector]
    public  float       absoluteShootingForce;          //guarda força total do tiro
    public  GameObject  pushingSpeel;                   //spell lançado ao empurrar;
    [HideInInspector]
    public  bool        canUseSpell;                    //define se o poder spell poderá se spawnado;
 
    //ANIMATION ATTRIBUTES
            Change_camera_atributes cam;
            Animator    anim;                           //animações   

    //UI ATTIBUTES
            GirlUI      GirlUi;                         //script de controle de ui da menina
    public AudioManager audioManager;

    void Awake(){
        //inicializar ps componentes do jogo
        rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        GirlUi = GetComponent<GirlUI>();
        cam = GetComponent<Change_camera_atributes>();

        //inicializar as variaveis do jogo
        resetAtributtes();
    }
   
    void FixedUpdate ()
    {
        if(canMove && !inDialogue)
        {
            Move();
            FlipControl(); 
        }
        SpawnGroundParticle();    
        JumpGravityControl();
        CalcTime();         

        //Controla animação quando parada
        if (inDialogue)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", false);
        }
    }  

    private void Update()
    {       
        if(canMove && !inDialogue)
        {
            Jump();
            WalkAnim();
        }   
        CheckRiseInput();
        InputShootRock();
        CheckPushingInput();

        //Checar se esta no chao
        inGround = Physics2D.OverlapCircle(check.position, radius, whatIsGround);

        //movimentação
        verticalInput = Input.GetAxisRaw("Horizontal");        
        
        //Se nochão for false inicia animação de pulo
        if (inGround == false && canMove)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", true);
        }
        else
        {
            anim.SetBool("Pulo", false);
        }        

        //Reset de Cena
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        if(gravity == false)
        {
            rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// restart all atributtes of the girl
    /// </summary>
    void resetAtributtes()
    {
        pushing = false;
        shooting = false;
        jumpVelocity = 9f;
        runVelocity = 7.5f;
        radius = 0.30f;
        canUseSpell = true;
        spawnJumpParticle = false;
        lowjumpmultiplayer = 4.3f;
        fallMultiplier = 5f;
        gravity = true;
        inDialogue = false;
        face = true;
        stopParallax = false;
        canMove = true;
        pressedSpace = false;
        riseGirl = true;
    }

    //*******************************************************************\\
    //FIGHT METHODS

    /// <summary>
    /// check if the input for pushing is presionated, if true: call Push() method 
    /// </summary>
    void CheckPushingInput(){
        if (Input.GetKeyDown(KeyCode.X) && canMove && canUseSpell && !shooting)
        {
            Push();
        }
    }

    /// <summary>
    /// launch the pushing spell
    /// </summary>
    void Push()
    {
        Vector3 instanciador12 = rockInstancePosition.transform.position;
        Instantiate(pushingSpeel, instanciador12, rockInstancePosition.rotation);
        time = 1.5f;
        riseGirl = true;
        GirlUi.AtivarEmpurrar();
        anim.SetBool("Empurrar", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Pulo", false);
        anim.SetBool("Andando", false);
        canMove = false;
        pushing = true;
    }

    /// <summary>
    /// Check if the rise input was pressed in the right time
    /// </summary>
    void CheckRiseInput(){
        //Right time
        if (time >= 0.8f && time <= 1f){           
            if (Input.GetKeyDown(KeyCode.X) && !pressedSpace){
                GirlUi.DesativarUI();                
                rb.velocity = new Vector2(0, 0);
                pushing = true;
                time = 0;
                anim.SetBool("Levantar", true); 
            }
        
        //wrong time
        } else if (time <= 1.4f && time >= 1f || time >= 0.2f && time <= 0.8f){            
            if(Input.GetKeyDown(KeyCode.X)){
                pressedSpace = true;
                GirlUi.DesativarUI();
            }
        }
    }

    /// <summary>
    /// ensure there are no bugs in the character's stoppage
    /// </summary>
    void CalcTime(){        
        if (time <= 5 && time > Time.deltaTime)
        {              
            time -= Time.deltaTime;               
            if(time <= Time.deltaTime && riseGirl)
            {
                GirlUi.DesativarUI();
                anim.SetBool("Levantar", true);
                riseGirl = false;
                pushing = false;                     
            }
        }
    }

    /// <summary>
    /// check if the shoot input was pressed
    /// </summary>
    void InputShootRock()
    {
        //load strength of the stone if you have ammo and pressing the push key
        if (Input.GetKey(KeyCode.Z) && canMove && ammunition >= 1 && !pushing)
        {
            GirlUi.AtivarAtirarPedra();
            shooting = true;
            if (shootingForce <= 1)
            {
                shootingForce = shootingForce + Time.deltaTime;
            }
        }

        //atirar pedra
        if (Input.GetKeyUp(KeyCode.Z) && ammunition >= 1 && !pushing)
        {
            ShootRock();
            shooting = false;
        }
    }

    /// <summary>
    /// triggers the act of throwing the stone
    /// </summary>
    void ShootRock()
    {
        Vector3 instanciador12 = rockInstancePosition.transform.position;
        Instantiate(rock, instanciador12, rockInstancePosition.rotation);
        GirlUi.DesativarUI();
        absoluteShootingForce = shootingForce;
        shootingForce = 0;
        ammunition--;
    }

    /// <summary>
    /// raises the girl changing animation and variables
    /// </summary>
    void RiseGirl(){
        anim.SetBool("Puxando", false);
        anim.SetBool("Empurrar", false);
        anim.SetBool("Levantar",false);
        canMove = true;
        pressedSpace = false;
        pushing = false;
    }


    //*************************JUMP METHODS**********************************\\
    /// <summary>
    /// add strength up and check if 2 jumps are done
    /// </summary>
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && inGround == true){
            inGround = false;
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse); 
            Instantiate(jumpParticle, check.position,Quaternion.identity);
            audioManager.PlayGirlJumpSound();
        }
    }

    public void PlayFootStepSound()
    {
        audioManager.PlayGirlFootSteps();
    } 
   

    /// <summary>
    ///  set variable "spawnJumoParticle", activated/called in animation
    /// </summary>
    void SetSpawnJumpParticle()
    {
        spawnJumpParticle = true;
    }

    /// <summary>
    /// spawn Jump Particle and disable variable spawnJumpParticle
    /// </summary>
    void SpawnGroundParticle()
    {
        if(inGround && spawnJumpParticle)
        {
            spawnJumpParticle = false;
            Instantiate(jumpParticle, check.position,Quaternion.identity);    
            cam.shake();         
        } 
    }


    /// <summary>
    /// Gravity change in the jump according to how much the user pressed the jump button
    /// </summary>
    void JumpGravityControl()
    {
        if (rb.velocity.y < 0 && gravity)
        { 
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow))
        {
            rb.gravityScale = lowjumpmultiplayer;
        }
        else
        {
            rb.gravityScale = 2f;
        }      
    }

    //*********************MOVIMENTATION METHODS*************************************\\

    /// <summary>
    /// moves in the direction of the input, positive or negative
    /// </summary>
    void Move()
    {
        transform.Translate(verticalInput * Time.deltaTime * runVelocity, 0,0);
    }

    //seta anim de andar, dir esquera 
    void WalkAnim()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if (inGround)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (inGround)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
            }
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (inGround)
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            if (inGround)
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
        }
    }

    /// <summary>
    /// changes the direction of the character according to the input
    /// </summary>
    void Flip ()
    {
		face = !face;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

    /// <summary>
    /// controls the character's direction and the location of the stone instancier
    /// </summary>
    void FlipControl()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !face){
            Flip();
            rockInstancePosition.Rotate(0, -180, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && face){
            Flip();
            rockInstancePosition.Rotate(0, 180, 0);               
        }        
    }

    /// <summary>
    /// controls the direction of climb, right / left
    /// </summary>
    void Climb()
    {
        if (ClimbRight)
        {
            transform.position = new Vector3(ClimbPos.x + 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            ClimbRight = false;
        }
        else if (ClimbLeft)
        {
            transform.position = new Vector3(ClimbPos.x - 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            ClimbLeft = false;
        }
        spawnJumpParticle = false;
        anim.SetBool("Idle", true);
        anim.SetBool("escalar", false);
        canMove = true;
        gravity = true;    
        rb.isKinematic = false; 
    }

    //********************COLLISION METHODS***************************************\\

    private void OnCollisionStay2D(Collision2D collision)
    {
        //the girl turns into a child of the enemy2 while under the enemy2
        if (collision.gameObject.tag == "enemy2")
        {
            transform.parent = collision.transform;
        }
        else
        {
            transform.parent = null;

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    { 
        //dead
        if (collision.CompareTag("morte"))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
        //colect ammo
        if (collision.CompareTag("municao"))
        {
            Destroy(collision.gameObject);
            ammunition++;
        }
        //stop parallax
        if(collision.CompareTag("parallax"))
        {
            stopParallax = true;
        }
        //can't use spell in this area
        if(collision.CompareTag("no_speel_area"))
        {
            canUseSpell = false;
        }

        //starts the Climb()
        if (collision.CompareTag("escalar") && canMove)
        {
            gravity = false;
            canMove = false; 
            rb.isKinematic = true;
            ClimbPos = GetComponent<Transform>().position;
            anim.SetBool("escalar", true);         
            anim.SetBool("Pulo", false);
            anim.SetBool("Idle", false);   
            if(collision.transform.position.x >= transform.position.x)
            {
                ClimbRight = true;
            }
            else
            {
                ClimbLeft = true;
            }
        }
    }   

    void OnTriggerExit2D(Collider2D collision)
    {
        //can use spell out of this area
        if(collision.CompareTag("no_speel_area"))
        {
            canUseSpell = true;
        }
        //ao sair pode continuar tendo parallax
        if(collision.CompareTag("parallax"))
        {
            stopParallax = false;
        }
    }
}