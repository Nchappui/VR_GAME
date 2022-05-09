using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlankPuzzle : MonoBehaviour
{
    // Start is called before the first frame update

    public int wellPlacedPlanks;
    public UnityEvent wellPlacedRow;
    void Start()
    {
        wellPlacedPlanks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        print(wellPlacedPlanks);
        if (wellPlacedPlanks == 4)
        {
            wellPlacedRow.Invoke();
            //Destroying after soling the puzzle reduces the computations of the game
            //StartCoroutine(DestroyRamp());
        }

    }

    IEnumerator DestroyRamp()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }

}
