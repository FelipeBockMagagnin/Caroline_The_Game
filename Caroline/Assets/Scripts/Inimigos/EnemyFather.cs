using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFather : MonoBehaviour {

 	[Range(0, 10)]
    public  float maxSpeed = 0;     
    [Range(0, 10)]
    public  float minSpeed = 0; 

	public  Transform OqueSeguir;         //dar follow

	protected float speed;                //contem a velocidade do inimigo
    protected float escalaX;              //contem escala x do inimigo
	protected Vector3 escala;             //contem a escala do inimigo
    protected float time = 0;             //tempo em que o inimigo fica parado

	protected bool empurrado = false;     //se ele for empurrado não pode atacar
    protected bool contador = false;      //define de o tempo continuara descendo

    public  GameObject menina;          //contem a definição de menina

	public  ParticleSystem particula_atingido;

	protected Animator anim;              //contem as animações do inimigo

	//*************************************************
    //movimentação

	protected void Seguir(Transform Seguido){
        if(!empurrado){
            if (Seguido.transform.position.x >= transform.position.x){
                speed = Random.Range(minSpeed,maxSpeed);
                escala = transform.localScale;
                escala.x = Mathf.Abs(escalaX);
                transform.localScale = escala;        
            } else {
                speed = -(Random.Range(minSpeed,maxSpeed));
                escala = transform.localScale;
                escala.x = -(Mathf.Abs(escalaX));
                transform.localScale = escala;       
            }
        }
    }

	protected void corEnemy(float corInicial){
        float cor;
        cor = 1 - corInicial/3.33f;
        GetComponent<SpriteRenderer>().color = new Color(1, cor, cor);    
    }

	protected void SpawnParticulaAtingido(){
        Instantiate(particula_atingido, transform.position, Quaternion.identity);
    }

}
