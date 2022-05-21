using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckTotalWeight : MonoBehaviour
{
    public UnityEvent correct_weights;
    public Transform pipe1;
    public Transform pipe2;
    public Transform pipe3;
    private CalculateWeight weight1;
    private CalculateWeight weight2;
    private CalculateWeight weight3;
    // Start is called before the first frame update
    void Start()
    {
        weight1 = pipe1.GetComponent<CalculateWeight>();
        weight2 = pipe2.GetComponent<CalculateWeight>();
        weight3 = pipe3.GetComponent<CalculateWeight>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckWeights()
    {
        if (weight1.enough_weight() && weight2.enough_weight() && weight3.enough_weight()) correct_weights.Invoke();
    }
}
