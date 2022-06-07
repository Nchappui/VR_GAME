using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorLabyButton : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip first_button;
    public AudioClip second_button;
    private AudioSource aSource;
    private bool isPlaying = false;
    public bool isSecond = false;
    void Start()
    {
        aSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator playSound()
    {
        isPlaying = true;
        aSource.Play();
        yield return new WaitWhile(() => aSource.isPlaying);
        isPlaying = false;
    }

    public void OtherPressed()
    {
        isSecond = true;
    }

    public void Pressed()
    {
        if (!isPlaying)
        {
            if (isSecond)
            {
                aSource.clip = second_button;
                StartCoroutine(playSound());
            }
            else
            {
                aSource.clip = first_button;
                StartCoroutine(playSound());
            }
        }
    }
}
