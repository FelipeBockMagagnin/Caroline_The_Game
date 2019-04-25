using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlataform : MonoBehaviour
{
    public float time;
    public float fallSpeed;
    float speed;

    bool collide = false;

    Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Menina") && !collide)
        {
            collide = true;
            Debug.Log("Fallplataform colidiu com menina");
            StartCoroutine(countTime());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Menina") && !collide)
        {
            collide = true;
            Debug.Log("Fallplataform colidiu com menina");
            StartCoroutine(countTime());
        }
    }

    IEnumerator countTime()
    {
        anim.SetTrigger("tilt");
        yield return new WaitForSeconds(0.7f);
        speed = -10;
        anim.SetTrigger("tilt");
        yield return new WaitForSeconds(0.05f);
        speed = 0;
        yield return new WaitForSeconds(0.7f);
        anim.SetTrigger("tilt");
        speed = -15;
        yield return new WaitForSeconds(0.05f);
        speed = 0;
        yield return new WaitForSeconds(0.7f);
        fall();
    }

    void fall()
    {
        speed = fallSpeed;
        Destroy(this.GetComponent<Collider2D>());
    }

    private void FixedUpdate()
    {
        transform.Translate(0, speed*Time.deltaTime, 0);
    }
}
