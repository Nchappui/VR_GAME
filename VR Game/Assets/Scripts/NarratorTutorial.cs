using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class NarratorTutorial : MonoBehaviour
{
    public AudioClip clip;
    public AudioClip secondClip;
    public AudioClip thirdClip;

    public UnityEvent donespeaking;

    private AudioSource audios;
    private bool isPlaying = false;
    private HashSet<AudioClip> already_played;

    public UnityEvent last;
    // Start is called before the first frame update
    void Start()
    {
        already_played = new HashSet<AudioClip>();
        audios = this.GetComponent<AudioSource>();
        audios.clip = clip;
        audios.volume = 0.3f;
        audios.Play();
        StartCoroutine(WaitSecond());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadAndPlaySound(AudioClip newAudio)
    {
        audios.clip = newAudio;
        if (!isPlaying) {
            StartCoroutine(playSound());
        }
        
    }

    public void PlaySoundOnce(AudioClip newAudio)
    {
        
        if (!isPlaying && !already_played.Contains(newAudio))
        {
            audios.clip = newAudio;
            already_played.Add(newAudio);
            StartCoroutine(playSound());
        }

    }

    public void PlayLastSound(AudioClip newAudio)
    {
        if (!isPlaying && !already_played.Contains(newAudio))
        {
            audios.clip = newAudio;
            already_played.Add(newAudio);
            StartCoroutine(playLastSound());
        }
    }

    IEnumerator playSound()
    {
        isPlaying = true;
        audios.Play();
        yield return new WaitWhile(() => audios.isPlaying);
        isPlaying = false;
    }

    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(clip.length+2f);
        audios.clip = secondClip;
        audios.Play();
        StartCoroutine(WaitThird());
    }

    IEnumerator WaitThird()
    {
        yield return new WaitForSeconds(secondClip.length+3f);
        audios.clip = thirdClip;
        audios.Play();
        StartCoroutine(WaitDoor());
    }

    IEnumerator WaitDoor()
    {
        yield return new WaitForSeconds(thirdClip.length);
        donespeaking.Invoke();

    }

    IEnumerator playLastSound()
    {
        isPlaying = true;
        audios.Play();
        yield return new WaitWhile(() => audios.isPlaying);
        isPlaying = false;
        last.Invoke();
    }
}
