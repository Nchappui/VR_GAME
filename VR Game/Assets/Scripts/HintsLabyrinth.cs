using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HintsLabyrinth : MonoBehaviour { 

    public AudioClip press1;
    public AudioClip press2;
    public AudioClip press3;

    private int Clicked = 0;
private bool isPlaying = false;
private AudioSource aSource;

public UnityEvent show_layout;
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
                show_layout.Invoke();
                break;
              default:
                    aSource.clip = press3;
                    StartCoroutine(playSound());
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
