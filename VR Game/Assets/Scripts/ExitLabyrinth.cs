using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitLabyrinth : MonoBehaviour
{
    public AudioClip press1;
    public AudioClip press2;
    private int Clicked = 0;
    private bool isPlaying = false;
    private AudioSource aSource;

    public UnityEvent exit_fired;
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
        if (!isPlaying)
        {
            Clicked += 1;
            switch (Clicked)
            {
                case 1 when !isPlaying:
                    aSource.clip = press1;
                    StartCoroutine(playSound());
                    break;
                case 2 when !isPlaying:
                    aSource.clip = press2;
                    StartCoroutine(playSound());
                    exit_fired.Invoke();
                    break;
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
}
