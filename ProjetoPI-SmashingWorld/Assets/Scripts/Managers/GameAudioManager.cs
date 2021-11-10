using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    AudioClip ClickSound;

    [SerializeField]
    AudioClip JumpSound;
    [SerializeField]
    AudioClip Hit1Sound;
    [SerializeField]
    AudioClip Hit2Sound;
    [SerializeField]
    AudioClip DamageSound;
    [SerializeField]
    AudioClip DashSound;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<AudioSource>();

    }


    public void PlayClickSound()
    {
       audioSource.PlayOneShot(ClickSound);
    }

    public void PlayJumpSound()
    {
        audioSource.clip = JumpSound;
        audioSource.time = 0.4f;
        audioSource.Play();
        
        
    }

    public void PlayHit1Sound()
    {
        audioSource.PlayOneShot(Hit1Sound);
    }

    public void PlayHit2Sound()
    {
        audioSource.PlayOneShot(Hit2Sound);
    }

    public void PlayDamagesound()
    {
 
        audioSource.PlayOneShot(DamageSound);

    }

    public void PlayDashSound()
    {
        
        audioSource.PlayOneShot(DashSound);
    }

    public void PlayDeathSound()
    {


        audioSource.gameObject.AddComponent<AudioEchoFilter>().delay= 300;
        audioSource.gameObject.AddComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.StoneCorridor;
        
        audioSource.clip = DamageSound;
        audioSource.Play();
        
    }


}
