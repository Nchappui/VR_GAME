using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf_opening : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 init_pos;
    private Vector3 open_pos;
    public Transform open_pos_tranform;
    private bool is_openning = false;
    public float speed = 1.0F;
    private float startTime;
    private float distance;

    public int number_of_books_to_place;
    private int current_number_of_books=0;
    
    void Start()
    {
        init_pos = this.transform.position;
        open_pos = open_pos_tranform.position;
        distance = Vector3.Distance(init_pos, open_pos);
    }

    // Update is called once per frame
    void Update()
    {
        if (is_openning)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / distance;
            this.transform.position = Vector3.Lerp(init_pos, open_pos, fractionOfJourney);
            if (this.transform.position == open_pos)
            {
                is_openning = false;
            }
        }
    }

    public void TriggerOpenning()
    {
        if (current_number_of_books == number_of_books_to_place) {

            startTime = Time.time;
            is_openning = true;
        }
    }

    public void One_more_book()
    {
        current_number_of_books += 1;
        TriggerOpenning();
    }
    public void One_less_book()
    {
        current_number_of_books -= 1;
        TriggerOpenning();
    }
}
