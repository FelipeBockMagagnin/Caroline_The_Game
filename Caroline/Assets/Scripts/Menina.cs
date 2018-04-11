using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menina : MonoBehaviour {

	//Inverte escala do  personagem de acordo com os inputs
	public bool face = true;

	//Atribuição de partes da menina para uso no codigo
	public Transform meninaT;
	public Animator anim;
	Rigidbody2D rb;

	//Var de movimentação
	public float VelocidadePulo;
	public float VelCorrendo;

	//PULO
	//Gravidade aplicada, aumenta quando velocidade y < 0
	private float fallMultiplier = 2.5f;
	private float lowjumpmultiplayer = 2f;
	//Checar se esta no chão, se estiver pula
	private bool nochao;
	public Transform check;
	public LayerMask OqueEChao;
	float raio = 0.2f;
	//Var de pulo que contem a força apricada, é iniciada quando apertar espaço e estiver no chão
	private bool Jumprequest;

	//Tiro
	public Transform localTiro;
	public GameObject SpawnTiro;

	//Pegar pedra do chão
	public int quantidadePedras;
	public TextMesh contagemPedras;

	//pedras
	public GameObject[] PedrasTiro = new GameObject[4];
	int NroTiposPedras;

	//Vida menina
	public int VidaMenina = 5;
	public TextMesh VidaMeninatxt;
	public bool estaViva = true;

	//Pega var da menina
	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
		
	void Update () {

		//UI provisório
		contagemPedras.text = quantidadePedras.ToString ();
		VidaMeninatxt.text = VidaMenina.ToString ();

		//Checa se esta no chão
		nochao =  Physics2D.OverlapCircle(check.position, raio, OqueEChao);

		//se menina esta vida continua o script
	
			

		//Flip() de acordo com o input
		//Flip() inverter escala do personagem
		if (Input.GetKey (KeyCode.RightArrow) && !face) {
			Flip ();
			SpawnTiro.transform.Rotate (0, 0, 180);
		}
		if (Input.GetKey (KeyCode.LeftArrow) && face) {
			Flip ();
			SpawnTiro.transform.Rotate (0, 0, 180);
		}
			
		//se apertar E e tiver pedras ele cria um objeto no gameobject tiro anexado a menina, se destroy em 0.5 seg
		if (Input.GetKeyDown (KeyCode.E) && quantidadePedras > 0) {
			NroTiposPedras = Random.Range (0, 4);
			GameObject Tiro = Instantiate (PedrasTiro[NroTiposPedras], localTiro.position, localTiro.rotation);
			quantidadePedras--;
			Destroy (Tiro, 0.5f);
			}
			
		//se estiver no chão transforma a var nochao em falsa e inicia o script de pulo
		if (Input.GetKeyDown (KeyCode.Space) && nochao == true) {
			nochao = false;
			Jumprequest = true;
		}

		//morte
		if(VidaMenina <= 0){
			gameObject.SetActive (false);
			Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
		}

		//Se nochão for false inicia animação de pulo
		if (nochao == false) {
			anim.SetBool ("Idle", false);
			anim.SetBool ("Andando", false);
			anim.SetBool ("Pulo", true);
		} else {
			anim.SetBool ("Pulo", false);
		}
	}
		
	void FixedUpdate(){
		//Modifica a gravidade para o personagem cair mais rapido do que sobe
		//Se velocidade.y for menor que 0 aumenta a gravidade
		if (rb.velocity.y < 0) {
			rb.gravityScale = fallMultiplier;
		} else if (rb.velocity.y > 0 && !Input.GetKey (KeyCode.Space)) {
			rb.gravityScale = lowjumpmultiplayer;
		} else {
			rb.gravityScale = 1f;
		}

		//se menina esta vida continua o script

		//Movimentação horizontal no RightArrow e LeftArrow
		if (Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.LeftArrow)) {
			transform.Translate (VelCorrendo * Time.deltaTime, 0, 0);
			anim.SetBool ("Idle", false);
			anim.SetBool ("Andando", true);
		} else if (Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
			anim.SetBool ("Idle", false);
			anim.SetBool ("Andando", true);
			transform.Translate (-VelCorrendo * Time.deltaTime, 0, 0);
		} else if (!Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
			anim.SetBool ("Idle", true);
			anim.SetBool ("Andando", false);
		} else if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.RightArrow)) {
			anim.SetBool ("Idle", true);
			anim.SetBool ("Andando", false);
		}

		//Script de pulo, inplementando força para cima
		if (Jumprequest) {
			rb.AddForce (Vector2.up * VelocidadePulo, ForceMode2D.Impulse);
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

	//Coletar pedra do chão
	void OnTriggerEnter2D(Collider2D pedra) {
		if (pedra.CompareTag ("pedra")){
			if (quantidadePedras < 5) {
			quantidadePedras++;
			Destroy (pedra.gameObject);
			}
		}
		if (pedra.CompareTag("morte")){
			VidaMenina = 0;
		}
	}
	void OnCollisionEnter2D(Collision2D inimigo){
		if (inimigo.gameObject.CompareTag ("inimigo")) {
			Destroy (inimigo.gameObject);
			if (VidaMenina > 0) {
				VidaMenina--;
			}

			}
		}
	}

		

