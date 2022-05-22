using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitAllTargets : MonoBehaviour
{
    public UnityEvent spawn_key;

    private bool target1_hit = false;
    private bool target2_hit = false;
    private bool target3_hit = false;
    private bool target4_hit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void checkTargets()
    {
        if(target1_hit && target2_hit && target3_hit && target4_hit)
        {
            spawn_key.Invoke();
        }
    }
    public void tg1_hit()
    {
        target1_hit = true;
        checkTargets();
    }
    public void tg2_hit()
    {
        target2_hit = true;
        checkTargets();
    }
    public void tg3_hit()
    {
        target3_hit = true;
        checkTargets();
    }
    public void tg4_hit()
    {
        target4_hit = true;
        checkTargets();
    }
}
