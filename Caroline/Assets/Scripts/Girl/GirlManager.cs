using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlManager : MonoBehaviour
{
    public static GirlManager instance = null;

    public bool canMove;                        //movimentação precisa dessa var           
    public bool stopParallax;                   //evitar paralax de andar enquanto menina trancada em parede
    public bool inGround;                       //ver se esta no chão por meio de raycast 
    public bool interacting;
    public bool canBeChildOfEnemy = true;
    public bool inDialogue;
    public bool canBeAttacked;
    public bool hide;
    public float fallMultiplier;                 //modificação da gravidade
    public float lowjumpmultiplayer;             //modificação da gravidade
    public bool spawnJumpParticle;              //define se ira spawnar particula durante o pulo
    public int ammunition;                     //quantidade de monição
    public float shootingForce;                  //aumenta quando pressionado control
    public float absoluteShootingForce;          //guarda força total do tiro
    public bool canUseSpell;                    //define se o poder spell poderá se spawnado;
    public bool face;                           //define a movimentação dir/esq invertendo a escala quando chamado
    public bool ClimbRight;                     //define a direção que ira se telopertar apos climb
    public bool ClimbLeft;                      //define a direção que ira se teleprotar apos climb
    public bool gravity;                        //climb(), acaba com a movimentação do personagem                      
    public float jumpVelocity;                   //padrão de altura possivel com pulo
    public float runVelocity;                    //padrão de velocidade possivel com pulo
    public bool invertDirection = false;        //
    public bool shooting;
    public float time;
    public bool pressedSpace;
    public float verticalInput;                  //contem o input, 1 se dir/-1se esq/0 de parado

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        CalcTime();
    }

    public void CalcTime()
    {
        if (time <= 5 && time > Time.deltaTime)
        {
            time -= Time.deltaTime;
        }
    }

    private GirlManager()
    {

    }
}
