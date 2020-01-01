using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] sceneMusics;
    private AudioSource musicPlayer;
    private int actualIndex;


    //play the first song
    private void Start()
    {
        musicPlayer = this.GetComponent<AudioSource>();
        musicPlayer.clip = sceneMusics[0];
        musicPlayer.Play();
        actualIndex = 0;
    }

    public void changeSongTo(int songIndex)
    {
        //play only if is a diferent music
        if (songIndex != actualIndex)
        {
            musicPlayer.Stop();
            musicPlayer.clip = sceneMusics[songIndex];
            musicPlayer.Play();
            actualIndex = songIndex;
        }
    }
}
