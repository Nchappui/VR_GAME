using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsLastLevel : MonoBehaviour
{
    public List<AudioClip> clips;

    private AudioSource aSource;
    private bool isPlaying = false;
    private int hintNumber = 0;
    private bool flowerSolvedb = false;
    private bool bowSolvedb = false;
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
            if (!flowerSolvedb && hintNumber == 2)
            {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
            }
            else if (!bowSolvedb && hintNumber == 6)
            {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
            }
            else if (hintNumber == clips.Count)
            {
                aSource.clip = clips[hintNumber];
                StartCoroutine(playSound());
            }
            else
            {
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

    public void flowerSolved()
    {
        flowerSolvedb = true;
        hintNumber = 4;
    }

    public void bowSolved()
    {
        bowSolvedb = true;
        hintNumber = 6;
    }
}
