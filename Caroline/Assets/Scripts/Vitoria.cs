using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitoria : MonoBehaviour {

    public int          childs;         //numero de inimigos em batalha
    public GameObject   Youwin;         //objeto que contem o final de jogo
    public TextMesh     tempo;          //texto que contem tempo decorido
    public float        tempodeJogo;    //var guarda tempo decorrido
      
	void Update () {
        childs = transform.childCount;


        //finalizar batalha
        if(childs == 0)
        {
            childs--;
            float tempodeJogo2;
            tempodeJogo2 = tempodeJogo;
            Youwin.SetActive(true);
            tempo.text = tempodeJogo2.ToString();
            
        }
        //caso não finalizado
        if(childs >=0){
            tempodeJogo = tempodeJogo + Time.deltaTime;
        }
    }
}
