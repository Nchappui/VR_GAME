using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CellNumber : MonoBehaviour
{
    public TextMeshPro text;
    public int number = 0;

    // Start is called before the first frame update
    void Start()
    {
        number = Int32.Parse(text.text); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Up()
    {
        //print("Up has been pressed");
        if (number == 9) number = 0;
        else number += 1;

        text.text = number.ToString();
    }

    public void Down()
    {
        //print("Down has been pressed");
        if (number == 0) number = 9;
        else number -= 1;

        text.text = number.ToString();
    }
}
