using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleActiver : MonoBehaviour
{

    PoleManager polemanager;
    public Animator anim;
    public ParticleSystem hitParticle;

    private void Start()
    {
        polemanager = GameObject.Find("PoleManager").GetComponent<PoleManager>();
        Debug.Log("Pole started");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spell_Menina_Empurrar") || collision.CompareTag("pedra"))
        {
            polemanager.activePole(this.gameObject, anim);
            Debug.Log("Collision");
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            activeParticle();
            this.GetComponent<AudioSource>().Play();
        }
    }

    void activeParticle()
    {
        this.GetComponent<ParticleSystem>().Play();
    }
}
