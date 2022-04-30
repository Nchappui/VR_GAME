using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plank_puzzle_ball : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 Respawn_pos;
    public GameObject EndPipe;
    public UnityEvent ProblemSolved;
    void Start()
    {
        Respawn_pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendBall()
    {
        this.transform.position = Respawn_pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == EndPipe.gameObject.name)
        {
            ProblemSolved.Invoke();
        }
    }
}
