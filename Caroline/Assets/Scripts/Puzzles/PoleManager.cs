using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pole
{
    public GameObject Pole;
    public bool activated;
}




public class PoleManager : MonoBehaviour
{
    public pole[] poles;
    public bool gateTurntoRight;
    public Animator gateAnim;

    public void OpenTheGate()
    {
        if (gateTurntoRight)
        {
            gateAnim.SetTrigger("OpenRight");
        } else
        {
            gateAnim.SetTrigger("OpenLeft");
        }
    }

    public void activePole(GameObject _pole, Animator anim)
    {
        foreach (pole _pol in poles)
        {
            if(_pol.Pole == _pole)
            {
                _pol.activated = true;
                setAnimActive(anim);
                checkIfActivated();
                break;
            }
        }
    }

    void setAnimActive(Animator anim)
    {
        anim.SetTrigger("active");
        Debug.Log("animation set");
    }

    void checkIfActivated()
    {
        int cont = 0;
        foreach (pole pol in poles)
        {
            if(pol.activated == true)
            {
                cont++;
            }
        }

        if(cont == poles.Length)
        {
            OpenTheGate();
        }
    }
}
