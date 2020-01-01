using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance = null;

    public AudioSource audioSource;
    public AudioClip girlHitSound;
    public AudioClip girlShootSound;
    public AudioClip enemyHitSound;
    public AudioClip girlJumpsound;
    public AudioClip[] girlFootSteps;

    private int footStepIndex = 0;

    void Awake(){
		if(instance == null)
        {
			instance = this;
		} 
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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

    private AudioManager()
    {
        
    }

}
