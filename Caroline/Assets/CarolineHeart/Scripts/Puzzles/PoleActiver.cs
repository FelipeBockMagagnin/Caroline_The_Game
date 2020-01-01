using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleActiver : MonoBehaviour
{

    public PoleManager polemanager;
    public Animator anim;
    public ParticleSystem hitParticle;
    bool activated = false;

    private void Start()
    {
        activated = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Spell_Menina_Empurrar") || collision.CompareTag("pedra")) && !activated)
        {
            activated = true;
            polemanager.activePole(this.gameObject, anim);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            this.GetComponent<AudioSource>().Play();
        }
    }
}
