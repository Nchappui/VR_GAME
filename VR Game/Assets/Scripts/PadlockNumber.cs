using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PadlockNumber : MonoBehaviour
{
    private static int SolutionNumber = 1234;
    public CellNumber cell_1;
    public CellNumber cell_2;
    public CellNumber cell_3;
    public CellNumber cell_4;
    public UnityEvent rightCode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckSolution()
    {
        if (cell_1.number *1000 + cell_2.number*100 + cell_3.number*10 + cell_4.number == SolutionNumber)
        {
            rightCode.Invoke();

        }
    }
}
