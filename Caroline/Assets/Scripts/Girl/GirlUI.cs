using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlUI : MonoBehaviour
{

    public GameObject uiCompleto;          //contem o template basico do UI tanto para atira quanto para empurrar
    public GameObject uiAtirar;            //representação da força do tiro
    public GameObject uiEmpurrar;          //demonstração do tempo  

    private void Start()
    {
        DesativarUI();
    }

    public void DesativarUI()
    {
        uiCompleto.SetActive(false);
        uiEmpurrar.SetActive(false);
        uiAtirar.SetActive(false);
    }

    public void AtivarEmpurrar()
    {
        uiCompleto.SetActive(true);
        uiEmpurrar.SetActive(true);
        uiAtirar.SetActive(false);
    }

    public void AtivarAtirarPedra()
    {
        uiCompleto.SetActive(true);
        uiEmpurrar.SetActive(false);
        uiAtirar.SetActive(true);
    }
}
