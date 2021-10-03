using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{

    public Vector3 LastPosition;

    float speed = 0.1f;

    public Rigidbody2D Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        LastPosition = transform.localPosition;

        Rigidbody = GetComponent < Rigidbody2D>();

        
    }



    private void OnCollisionStay2D(Collision2D collision)
    {


        if (collision.rigidbody.gameObject.name == "Plank")
        {
            transform.parent = collision.rigidbody.transform;
            Rigidbody.mass = 0.03f;

          
        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody.gameObject.name == "Plank")
        {
            transform.parent = collision.rigidbody.transform.parent.parent;
            Rigidbody.mass = 4;

            Rigidbody.AddRelativeForce(Vector2.up * 500f);
        }
    }

    // Update is called once per frame
    void Update()
    {


        LastPosition = transform.localPosition;

    }
}
