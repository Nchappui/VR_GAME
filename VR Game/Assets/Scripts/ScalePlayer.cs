using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlayer : MonoBehaviour
{

    public bool scalePlayer = false;
    private float yScale=0;

    public GameObject vrCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (scalePlayer)
        {
            StartCoroutine(ScalePlayerHeight());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (vrCamera.transform.position.y < 1.1 && scalePlayer)
        {
            yScale = this.transform.localScale.y;
            yScale += 0.05f;
            //print(yScale);
            this.transform.localScale = new Vector3(1, yScale, 1);
        }
        else if (vrCamera.transform.position.y > 1.2 && scalePlayer)
        {
            yScale = this.transform.localScale.y;
            yScale -= 0.05f;
            //print(yScale);
            this.transform.localScale = new Vector3(1, yScale, 1);
        }
    }

    IEnumerator ScalePlayerHeight()
    {
        yield return new WaitForSeconds(2);
        scalePlayer = false;
    }
}
