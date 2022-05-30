using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsLevel1 : MonoBehaviour
{
    public List<AudioClip> clips;

    private AudioSource aSource;
    private bool isPlaying = false;
    private int hintNumber=0;
    private bool plankSolvedb = false;
    private bool librarySolvedb = false;
    private bool batteryPlacedb = false;
    // Start is called before the first frame update
    void Start()
    {
        aSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextHint()
    {
        if (!isPlaying)
        {
            if (!plankSolvedb && hintNumber==3) {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
            }
            else if (!librarySolvedb && hintNumber == 5)
            {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
            }
            else if (!batteryPlacedb && hintNumber == 7)
            {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
            }
            else if (hintNumber == clips.Count)
            {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
            }
            else {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
                hintNumber += 1;
            }
            
        }

    }

    IEnumerator playSound()
    {
        isPlaying = true;
        aSource.Play();
        yield return new WaitWhile(() => aSource.isPlaying);
        isPlaying = false;
    }

    public void plankSolved()
    {
        plankSolvedb = true;
        hintNumber = 4;
    }

    public void librarySolved()
    {
        librarySolvedb = true;
        hintNumber = 6;
    }

    public void batteryPlaced()
    {
        batteryPlacedb = true;
        hintNumber = 8;
    }
}
