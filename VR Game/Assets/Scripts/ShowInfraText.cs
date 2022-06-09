using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInfraText : MonoBehaviour
{
    private bool projo_on = false;
    private bool lights_on = true;
    private Color new_color;
    private double new_albedo = 0;
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
       new_color = text.color;
    }

    // Update is called once per frame
    void Update()
    { 
        if(projo_on && !lights_on)
        {
            float roty = this.transform.rotation.y;
            if(-1f<=roty && roty <= -0.7f)
            {
                new_albedo = 9/3 * roty + 9/3;
                new_color.a = (float)new_albedo;
                text.color = new_color;
            }
            else if (-0.7 < roty && roty <= -0.4)
            {
                new_albedo = -9/3 * roty -4/3;
                new_color.a = (float)new_albedo;
                text.color = new_color;
            }

        }
        else if (lights_on)
        {
            new_albedo = 0;
            new_color.a = (float)new_albedo;
            text.color = new_color;
        }
    }

    public void ChangeProjoOn()
    {
        projo_on = !projo_on;
    }
    public void ChangeLightsOn()
    {
        lights_on = !lights_on;
    }
}
