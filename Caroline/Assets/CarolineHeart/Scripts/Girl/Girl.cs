using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Girl : MonoBehaviour
{
    //MOVIMENTATION ATTRIBUTES    
    private Rigidbody2D rb;                             //aplicar forças
    private Vector3 ClimbPos;                       //localização de onde o personagem ficará apos o climb()

    //JUMO ATRRIBUTES          
    public Transform check;                          //checar o chao
    public LayerMask whatIsGround;                   //raycast check ground
    public LayerMask enemy2LayerMask;
    public GameObject jumpParticle;                   //contem a particula liberada ao pular7      

    //SHOOTING ATTRIBUTES    
    public GameObject rock;                           //tiro prefab    
    public Transform rockInstancePosition;           //onde o tiro sera iniciado    
    public GameObject heartSpeel;                     //spell lançado ao empurrar;
    public ParticleSystem heartSpellParticles;

    //ANIMATION ATTRIBUTES
    private Change_camera_atributes cam;
    public Animator anim;                           //animações   

    //UI ATTRIBUTES
    private GirlUI GirlUi;                         //script de controle de ui da menina

    //MISC ATTRIBUTES
    public GameObject interactBaloon;
    private GameObject interactiveObj;

    private bool touchEnemy2;                       //Se tocou o inimigo 2 ava    

    private void Awake()
    {
        //inicializar ps componentes do jogo
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GirlUi = GetComponent<GirlUI>();
        cam = GetComponent<Change_camera_atributes>();

        //inicializar as variaveis do jogo
        GirlManager.instance.ResetAtributtes();
    }

    private void FixedUpdate()
    {
        if (GirlManager.instance.time <= Time.deltaTime && !GirlManager.instance.canUseSpell)
        {
            StartCoroutine(FinishCastHeartSpell());
        }

        if (GirlManager.instance.canMove)
        {
            Move();
            FlipControl();
        }
        SpawnGroundParticle();
        JumpGravityControl();
    }

    private void Update()
    {                
        if (GirlManager.instance.canMove)
        {
            WalkAnim();
        }
        CheckInteract();
        ManageInputs();
 
        if (!touchEnemy2)
        {
            GirlManager.instance.inGround = Physics2D.OverlapCircle(check.position, GirlManager.instance.radius, whatIsGround);
        }

        if (!GirlManager.instance.invertDirection)
        {
            GirlManager.instance.verticalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            GirlManager.instance.verticalInput = -Input.GetAxisRaw("Horizontal");
        }

        //Se nochão for false inicia animação de pulo
        if (!GirlManager.instance.inGround && GirlManager.instance.canMove)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Andando", false);
            anim.SetBool("Pulo", true);
        }
        else
        {
            anim.SetBool("Pulo", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        if (GirlManager.instance.gravity == false)
        {
            rb.isKinematic = true;
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    //*******************************************************************\\
    //INPUTS METHODS
    private void ManageInputs()
    {
        CheckFinishSpellInput();

        if(!GirlManager.instance.canMove)
            return; 

        CheckInputCanBeAttackable();

        if(GirlManager.instance.hide)
            return;  

        CheckJumpInput();

        if(!GirlManager.instance.canUseSpell)
            return;    
        
        CheckHeartSpellInput();        
        CheckInputShootRock();        
    }

    private void CheckInputCanBeAttackable()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeCanBeAttackable();
        }
    }

    private void CheckHeartSpellInput()
    {
        if(GirlManager.instance.shooting)
            return;

        if(GirlManager.instance.interacting)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            KnockBackGirl();
            CastHeartSpell();
        }
    }

    private void CheckFinishSpellInput()
    {
        //Right time
        if (GirlManager.instance.time >= 0.8f && GirlManager.instance.time <= 1f)
        {
            if (Input.GetKeyDown(KeyCode.X) && !GirlManager.instance.pressedSpace)
            {
                StartCoroutine(FinishCastHeartSpell());
            }

            //wrong time
        }
        else if (GirlManager.instance.time <= 1.4f && GirlManager.instance.time >= 1f
              || GirlManager.instance.time >= 0.2f && GirlManager.instance.time <= 0.8f)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                GirlManager.instance.pressedSpace = true;
                GirlUi.DesativarUI();
            }
        }
    }

    private void CheckInputShootRock()
    {
        if(GirlManager.instance.ammunition < 1)
            return;

        //atirar pedra
        if (Input.GetKeyUp(KeyCode.Z))
        {
            ShootRock();
            GirlManager.instance.shooting = false;
        }

        //load strength of the stone if you have ammo and pressing the push key
        if (Input.GetKey(KeyCode.Z))
        {
            GirlUi.AtivarAtirarPedra();
            GirlManager.instance.shooting = true;
            if (GirlManager.instance.shootingForce <= 1)
            {
                GirlManager.instance.shootingForce = GirlManager.instance.shootingForce + Time.deltaTime;
            }
        }        
    }

    private void CheckJumpInput()
    {   
        if(!GirlManager.instance.inGround)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GirlManager.instance.inGround = false;
            rb.AddForce(Vector2.up * GirlManager.instance.jumpVelocity, ForceMode2D.Impulse);
            Instantiate(jumpParticle, check.position, Quaternion.identity);
            AudioManager.instance.PlayGirlJumpSound();
        }
    }

    //*******************************************************************\\
    //INTERACT METHODS
    private void CheckInteract()
    {
        if (!GirlManager.instance.hide)
        {
            interactBaloon.SetActive(GirlManager.instance.interacting);
        }

        if (Input.GetKeyDown(KeyCode.X) &&
        GirlManager.instance.interacting &&
        !GirlManager.instance.shooting &&
        GirlManager.instance.canUseSpell && interactiveObj != null && !GirlManager.instance.hide)
        {
            GirlManager.instance.canMove = false;
            interactiveObj.SendMessage("Interact");
            anim.SetBool("Idle", true);
            anim.SetBool("Andando", false);
        }
    }    

    //*******************************************************************\\
    //FIGHT METHODS 

    private void ChangeCanBeAttackable()
    {
        GirlManager.instance.canBeAttacked = !GirlManager.instance.canBeAttacked;
        if (GirlManager.instance.canBeAttacked)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            GirlManager.instance.hide = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            GirlManager.instance.hide = true;
        }
    }    

    private void CastHeartSpell()
    {
        GirlUi.AtivarEmpurrar();
        transform.parent = null;
        GirlManager.instance.time = 1.5f;
        anim.SetBool("Idle", true);
        anim.SetBool("Pulo", false);
        anim.SetBool("Andando", false);
        GirlManager.instance.canMove = false;
        GirlManager.instance.canUseSpell = false;
        anim.SetBool("StopHeartSpell", false);
        anim.SetTrigger("HeartSpell");
    }

    private void KnockBackGirl()
    {
        if (GirlManager.instance.face == Face.Right)
        {
            rb.AddForce(new Vector2(-8, 2), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(8, 2), ForceMode2D.Impulse);
        }
    }

    private void CreateHeartSpell()
    {
        Vector3 instanciador12 = rockInstancePosition.transform.position;
        Instantiate(heartSpeel, instanciador12, rockInstancePosition.rotation);
        Instantiate(heartSpellParticles, instanciador12, rockInstancePosition.rotation);
        transform.parent = null;
    }

    private IEnumerator FinishCastHeartSpell()
    {
        yield return new WaitForSeconds(0.1f);
        GirlUi.DesativarUI();
        anim.SetBool("Idle", true);
        anim.SetBool("Pulo", false);
        anim.SetBool("Andando", false);
        anim.SetBool("StopHeartSpell", true);
        rb.velocity = new Vector2(0, 0);
        GirlManager.instance.time = 0.3f;
        GirlManager.instance.pressedSpace = false;
        GirlManager.instance.canMove = true;
        GirlManager.instance.canUseSpell = true;
        cam.NormalShake();
    }    

    private void ShootRock()
    {
        Vector3 instanciador12 = rockInstancePosition.transform.position;
        Instantiate(rock, instanciador12, rockInstancePosition.rotation);
        GirlUi.DesativarUI();
        GirlManager.instance.absoluteShootingForce = GirlManager.instance.shootingForce;
        GirlManager.instance.shootingForce = 0;
        GirlManager.instance.ammunition -= 1;
    }

    //*************************JUMP METHODS**********************************\\

    private void SetSpawnJumpParticle()
    {
        GirlManager.instance.spawnJumpParticle = true;
    }

    private void SpawnGroundParticle()
    {
        if (GirlManager.instance.inGround && GirlManager.instance.spawnJumpParticle)
        {
            GirlManager.instance.spawnJumpParticle = false;
            Instantiate(jumpParticle, check.position, Quaternion.identity);
            cam.NormalShake();
        }
    }

    private void JumpGravityControl()
    {
        if (rb.velocity.y < 0 && GirlManager.instance.gravity)
        {
            rb.gravityScale = GirlManager.instance.fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow) && GirlManager.instance.gravity)
        {
            rb.gravityScale = GirlManager.instance.lowjumpmultiplayer;
        }
        else
        {
            rb.gravityScale = 2f;
        }
    }

    //*********************MOVIMENTATION METHODS*************************************\\
    private void Move()
    {
        transform.Translate(GirlManager.instance.verticalInput * Time.deltaTime * GirlManager.instance.runVelocity, 0, 0);
    }

    //seta anim de andar, dir esquera 
    void WalkAnim()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if (GirlManager.instance.inGround)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (GirlManager.instance.inGround)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Andando", true);
            }
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (GirlManager.instance.inGround)
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            if (GirlManager.instance.inGround)
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Andando", false);
            }
        }
    }

    private void Flip(Face direction)
    {
        GirlManager.instance.face = direction;        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void FlipControl()
    {
        if (Input.GetKey(KeyCode.RightArrow) && GirlManager.instance.face == Face.Left)
        {
            Flip(Face.Right);
            rockInstancePosition.Rotate(0, -180, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && GirlManager.instance.face == Face.Right)
        {
            Flip(Face.Left);
            rockInstancePosition.Rotate(0, 180, 0);
        }
    }

    private void Climb()
    {
        if (GirlManager.instance.climbDirection == ClimbDirection.Rigth)
        {
            transform.position = new Vector3(ClimbPos.x + 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            GirlManager.instance.climbDirection = ClimbDirection.None;
        }
        else if (GirlManager.instance.climbDirection == ClimbDirection.Left)
        {
            transform.position = new Vector3(ClimbPos.x - 1.3f, ClimbPos.y + 2.4f, transform.position.z);
            GirlManager.instance.climbDirection = ClimbDirection.None;
        }

        GirlManager.instance.spawnJumpParticle = false;
        anim.SetBool("Idle", true);
        anim.SetBool("escalar", false);
        GirlManager.instance.canMove = true;
        GirlManager.instance.gravity = true;
        rb.isKinematic = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    static public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    //********************COLLISION METHODS***************************************\\
    public void DetachCarolineChildren()
    {
        transform.parent = null;
        GirlManager.instance.canBeChildOfEnemy = true;
        GirlManager.instance.inGround = false;
        touchEnemy2 = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //the girl turns into a child of the enemy2 while under the enemy2
        if (collision.gameObject.tag == "enemy2" && GirlManager.instance.canBeChildOfEnemy)
        {
            //se o inimigo estiver indo para esquerda, tornar a menina um filho dele faria com ela invertesse a direção
            if (!collision.gameObject.GetComponent<Enemy2>().faceRight)
            {
                GirlManager.instance.invertDirection = true;
            }
            else
            {
                GirlManager.instance.invertDirection = false;
            }
            GirlManager.instance.inGround = true;
            transform.parent = collision.transform;
        }
        else
        {
            //transform.parent = null;
            GirlManager.instance.invertDirection = false;
        }

        if (collision.gameObject.CompareTag("fallplataform"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                GirlManager.instance.radius = 0.3f;
            }
            GirlManager.instance.radius = 0.7f;
        }
        else
        {
            GirlManager.instance.radius = 0.3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy2") & GirlManager.instance.canBeChildOfEnemy)
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

        if (collision.gameObject.CompareTag("fallplataform"))
        {
            GirlManager.instance.radius = 0.3f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //interract
        if (collision.CompareTag("Interactive"))
        {
            interactiveObj = collision.gameObject;
            GirlManager.instance.interacting = true;
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
            GirlManager.instance.ammunition++;
        }
        //stop parallax
        if (collision.CompareTag("parallax"))
        {
            GirlManager.instance.stopParallax = true;
        }
        //can't use spell in this area
        if (collision.CompareTag("no_speel_area"))
        {
            GirlManager.instance.canUseSpell = false;
        }

        //starts the Climb()
        if (collision.CompareTag("escalar") && GirlManager.instance.canMove)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.83f, 0.83f, 0.83f, 1);
            GirlManager.instance.gravity = false;
            GirlManager.instance.canMove = false;
            rb.isKinematic = true;
            ClimbPos = GetComponent<Transform>().position;
            anim.SetBool("escalar", true);
            anim.SetBool("Pulo", false);
            anim.SetBool("Idle", false);
            if (collision.transform.position.x >= transform.position.x)
            {
                GirlManager.instance.climbDirection = ClimbDirection.Rigth;
            }
            else
            {
                GirlManager.instance.climbDirection = ClimbDirection.Left;
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
        if (collision.CompareTag("Interactive"))
        {
            interactiveObj = null;
            GirlManager.instance.interacting = false;
        }

        //can use spell out of this area
        if (collision.CompareTag("no_speel_area"))
        {
            GirlManager.instance.canUseSpell = true;
            cam.shake();
        }
        //ao sair pode continuar tendo parallax
        if (collision.CompareTag("parallax"))
        {
            GirlManager.instance.stopParallax = false;
        }
    }
}