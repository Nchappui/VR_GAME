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
    // Start is called before the first frame update
    void Start()
    {
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
        audios.Play();
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
}
