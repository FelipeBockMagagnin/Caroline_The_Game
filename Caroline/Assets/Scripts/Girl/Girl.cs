using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Girl : MonoBehaviour {    
    //FIGHT ATTRIBUTES
    [HideInInspector]
    public  bool        pressedSpace;                   //saber se errou
    [HideInInspector]
    public  float       time;                           //tempo a ficar parado  
    [HideInInspector]
    public  bool        shooting;                       //define se esta no ato de atirar

    //MOVIMENTATION ATTRIBUTES
    [HideInInspector]
    public  bool        canMove;                        //movimentação precisa dessa var    
    [HideInInspector]       
    public  bool        stopParallax;                   //evitar paralax de andar enquanto menina trancada em parede
    [HideInInspector]
    public  bool        face;                           //define a movimentação dir/esq invertendo a escala quando chamado
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
            bool        invertDirection = false;                //
    
    //JUMO ATRRIBUTES          
    public  Transform   check;                          //checar o chao
    public  LayerMask   whatIsGround;                   //raycast check ground
    public  LayerMask   enemy2LayerMask;
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
    public  GameObject  heartSpeel;                   //spell lançado ao empurrar;
    public ParticleSystem heartSpellParticles;
    [HideInInspector]
    public  bool        canUseSpell;                    //define se o poder spell poderá se spawnado;
 
    //ANIMATION ATTRIBUTES
           Change_camera_atributes cam;
    [HideInInspector]
    public Animator    anim;                           //animações   

    //UI ATTRIBUTES
            GirlUI      GirlUi;                         //script de controle de ui da menina
    public  AudioManager audioManager;
            bool        touchEnemy2;

    //MISC ATTRIBUTES
    public  GameObject interactBaloon;
    private GameObject interactiveObj; 
    private bool interacting; 
    private bool canBeChildOfEnemy = true;
    private bool inDialogue;

    void Awake(){
        //inicializar ps componentes do jogo
        rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        GirlUi = GetComponent<GirlUI>();
        cam = GetComponent<Change_camera_atributes>();

        try
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        catch (System.Exception)
        {
            print("não foi acahado o audio manager");
        }

        //inicializar as variaveis do jogo
        resetAtributtes();
    }
   
    void FixedUpdate ()
    {
        if(canMove)
        {
            Move();
            FlipControl(); 
        }
        SpawnGroundParticle();    
        JumpGravityControl();
        CalcTime();      
        
        interactBaloon.SetActive(interacting);   
    }  

    private void Update()
    {       
        if(canMove)
        {
            Jump();
            WalkAnim();
        }
        CheckFinishSpellInput();
        InputShootRock();
        CheckHeartSpellInput();
        CheckInteract();

        //Checar se esta no chao 
        if (touchEnemy2 == false)
        {
            inGround = Physics2D.OverlapCircle(check.position, radius, whatIsGround);
        }

        //movimentação
        if (!invertDirection)
        {
            verticalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            verticalInput = -Input.GetAxisRaw("Horizontal");
        }
        
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
            rb.isKinematic = true;
            rb.velocity = new Vector2(0, 0);
        } else
        {
            rb.isKinematic = false;
        }
    }

    /// <summary>
    /// restart all atributtes of the girl
    /// </summary>
    void resetAtributtes()
    {
        shooting = false;
        jumpVelocity = 9f;
        runVelocity = 7.5f;
        radius = 0.30f;
        canUseSpell = true;
        spawnJumpParticle = false;
        lowjumpmultiplayer = 4.3f;
        fallMultiplier = 5f;
        gravity = true;
        face = true;
        stopParallax = false;
        canMove = true;
        pressedSpace = false;
        interacting = false;
    }

    //*******************************************************************\\
    //INTERACT METHODS
    private void CheckInteract()
    {
        if(Input.GetKeyDown(KeyCode.X) && interacting && !shooting && canUseSpell && interactiveObj != null)
        {
            canMove = false;
            interactiveObj.SendMessage("Interact"); 
            anim.SetBool("Idle", true);   
            anim.SetBool("Andando", false);        
        }
    }

    //*******************************************************************\\
    //FIGHT METHODS

    /// <summary>
    /// check if the input for pushing is presionated, if true: call Push() method 
    /// </summary>
    void CheckHeartSpellInput(){
        if (Input.GetKeyDown(KeyCode.X) && canMove && canUseSpell && !shooting && !interacting)
        {
            knockBack();
            CastHeartSpell();
        }
    }

    /// <summary>
    /// cast a heart spell that kill enemys
    /// </summary>
    void CastHeartSpell()
    {                
        GirlUi.AtivarEmpurrar();
        transform.parent = null;
        time = 1.5f;
        anim.SetBool("Idle", true);
        anim.SetBool("Pulo", false);
        anim.SetBool("Andando", false);
        canMove = false;
        canUseSpell = false;
        anim.SetBool("StopHeartSpell", false);
        anim.SetTrigger("HeartSpell");               
    }

    private void knockBack()
    {
        if(face)
        {
            rb.AddForce(new Vector2(-8,2),ForceMode2D.Impulse);
        }
        else 
        {
            rb.AddForce(new Vector2(8,2),ForceMode2D.Impulse);
        }
    }

    public void CreateHeartSpell()
    {
        Vector3 instanciador12 = rockInstancePosition.transform.position;
        Instantiate(heartSpeel, instanciador12, rockInstancePosition.rotation);
        Instantiate(heartSpellParticles, instanciador12, rockInstancePosition.rotation);     
        transform.parent = null;   
    }

    /// <summary>
    /// make the girl move again and can shoot again too
    /// </summary>
    /// <returns></returns>
    IEnumerator FinishCastHeartSpell()
    {
        yield return new WaitForSeconds(0.1f);
        GirlUi.DesativarUI();
        anim.SetBool("Idle", true);
        anim.SetBool("Pulo", false);
        anim.SetBool("Andando", false);
        anim.SetBool("StopHeartSpell", true);
        rb.velocity = new Vector2(0,0);
        time = 0.3f;
        pressedSpace = false;
        canMove = true;
        //gravity = true;
        canUseSpell = true;
        cam.NormalShake();
    }

    /// <summary>
    /// Check if the finish spell input was pressed in the right time
    /// </summary>
    void CheckFinishSpellInput(){
        //Right time
        if (time >= 0.8f && time <= 1f){           
            if (Input.GetKeyDown(KeyCode.X) && !pressedSpace){
                StartCoroutine(FinishCastHeartSpell());
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
            if(time <= Time.deltaTime && !canUseSpell)
            {
                StartCoroutine(FinishCastHeartSpell());                       
            }
        }
    }

    /// <summary>
    /// check if the shoot input was pressed
    /// </summary>
    void InputShootRock()
    {
        //load strength of the stone if you have ammo and pressing the push key
        if (Input.GetKey(KeyCode.Z) && canMove && ammunition >= 1 && canUseSpell)
        {
            GirlUi.AtivarAtirarPedra();
            shooting = true;
            if (shootingForce <= 1)
            {
                shootingForce = shootingForce + Time.deltaTime;
            }
        }

        //atirar pedra
        if (Input.GetKeyUp(KeyCode.Z) && ammunition >= 1 && canUseSpell)
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
            playJumpSound();
        }
    }

    private void playGirlHitSound()
    {
        try
        {
            audioManager.PlayGirlHitSound(); 
        } 
        catch (NullReferenceException e)
        {
            Debug.Log("Error " + e.Message);
        }
    }

    public void PlayFootStepSound()
    {
        try
        {
            audioManager.PlayGirlFootSteps();
        }
        catch(NullReferenceException e)
        {
            Debug.Log("Error " + e.Message);
        }
    } 

    public void playJumpSound()
    {
        try
        {
            audioManager.PlayGirlJumpSound();
        } 
        catch (NullReferenceException e)
        {
            Debug.Log("Error " + e.Message);
        }
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
            cam.NormalShake();        
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
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow) && gravity)
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

    /// <summary>
    /// simple test reload level function
    /// </summary>
    static public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    //********************COLLISION METHODS***************************************\\
    public void DetachCarolineChildren()
    {
        transform.parent = null;
        canBeChildOfEnemy = true;
        inGround = false;
        touchEnemy2 = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //the girl turns into a child of the enemy2 while under the enemy2
        if (collision.gameObject.tag == "enemy2" && canBeChildOfEnemy)
        {
            //se o inimigo estiver indo para esquerda, tornar a menina um filho dele faria com ela invertesse a direção
            if (!collision.gameObject.GetComponent<Enemy2>().faceRight)
            {
                invertDirection = true;
            }
            else 
            {
                invertDirection = false;
            }
            inGround = true;            
            transform.parent = collision.transform;            
        }
        else
        {
            //transform.parent = null;
            invertDirection = false;
        }

        if(collision.gameObject.CompareTag("fallplataform"))
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = new Vector2 (rb.velocity.x,0);
                radius = 0.3f;
            }
            radius = 0.7f;
        }
        else 
        {
            radius = 0.3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy2") & canBeChildOfEnemy)
        {
            touchEnemy2 = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy2"))
        {
            DetachCarolineChildren();
        }

        if(collision.gameObject.CompareTag("fallplataform"))
        {
            radius = 0.3f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {   
        //interract
        if(collision.CompareTag("Interactive"))
        {
            interactiveObj = collision.gameObject;
            interacting = true;
        }

        //dead
        if (collision.CompareTag("morte"))
        {
            Reload();
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

        if (collision.CompareTag("enemy2"))
        {
            Reload();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //interract
        if(collision.CompareTag("Interactive"))
        {
            interactiveObj = null;
            interacting = false;
        }

        //can use spell out of this area
        if (collision.CompareTag("no_speel_area"))
        {
            canUseSpell = true;
            cam.shake();
        }
        //ao sair pode continuar tendo parallax
        if (collision.CompareTag("parallax"))
        {
            stopParallax = false;
        }
    }
}