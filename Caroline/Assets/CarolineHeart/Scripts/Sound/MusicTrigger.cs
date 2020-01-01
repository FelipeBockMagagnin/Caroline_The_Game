using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string collisionTag;
    public int musicIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(collisionTag)){
            GameObject.Find("MusicManager").GetComponent<MusicManager>().changeSongTo(musicIndex);
        }
    }
}
