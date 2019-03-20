using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnim : MonoBehaviour
{
    public string nameAnim;
    public bool statusAnim;
    public string collisionTag;
    public Animator animator;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(collisionTag))
        {
            animator.SetBool(nameAnim, statusAnim);
            Destroy(this.gameObject);
        }
    }



}
