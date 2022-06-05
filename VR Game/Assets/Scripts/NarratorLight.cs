using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorLight : MonoBehaviour
{
    public AudioClip off1;
    public AudioClip off2;
    public AudioClip off3;
    public AudioClip off4;
    private int Clicked=0;
    private bool isPlaying = false;
    private AudioSource aSource;
    // Start is called before the first frame update
    void Start()
    {
        aSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buttonClicked()
    {
        Clicked += 1;
        switch (Clicked)
        {
            case 1 when !isPlaying:
                aSource.clip = off1;
                StartCoroutine(playSound());
                break;
            case 3 when !isPlaying:
                aSource.clip = off2;
                StartCoroutine(playSound());
                break;
            case 5 when !isPlaying:
                aSource.clip = off3;
                StartCoroutine(playSound());
                break;
            case 7 when !isPlaying:
                aSource.clip = off4;
                StartCoroutine(playSound());
                break;
        }
    }

    IEnumerator playSound()
    {
        isPlaying = true;
        aSource.Play();
        yield return new WaitWhile(() => aSource.isPlaying);
        isPlaying = false;
    }
}
