using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


	private AudioManager instance;

    public AudioSource audioSource;
    public AudioClip girlHitSound;
    public AudioClip girlShootSound;
    public AudioClip enemyHitSound;
    public AudioClip girlJumpsound;
    public AudioClip[] girlFootSteps;

    int footStepIndex = 0;

    void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		} else {
			Destroy(gameObject);
		}
	}

    //futura verificação se esta no grass ou na madeira etc
    public void PlayGirlFootSteps()
    {
        audioSource.clip = girlFootSteps[footStepIndex];
        audioSource.Play();
        if(footStepIndex >= (girlFootSteps.Length - 1))
        {
            footStepIndex = 0;
        }
        else
        {
            footStepIndex++;
        }        
    }

    public void PlayGirlHitSound()
    {
        audioSource.clip = girlHitSound;
        audioSource.Play();
    }

    public void PlayGirlShootSound()
    {
        audioSource.clip = girlShootSound;
        audioSource.Play();
    }

    public void PlayEnemyHitSound()
    {
        audioSource.clip = enemyHitSound;
        audioSource.Play();
    }

    public void PlayGirlJumpSound()
    {
        audioSource.clip = girlJumpsound;
        audioSource.Play();
    }

}
