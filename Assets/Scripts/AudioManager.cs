using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    


    [Header("----------AudioSource----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----AudioClip------")]
    public AudioClip background;
    public AudioClip Owlhooting;
    public AudioClip Wolfhowling;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();

        // Start the coroutine to play SFX at interval 
        //StartCoroutine(PlaySFXAtInterval(Owlhooting, 20f,0.5f));
        StartCoroutine(PlaySFXAtInterval(Wolfhowling, 30f, 1f));
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
       
    }

    private IEnumerator PlaySFXAtInterval(AudioClip clip, float interval,float Volume)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            //PlaySFX(clip);
            SFXSource.PlayOneShot(clip, Volume);
            
        }
    }
}
