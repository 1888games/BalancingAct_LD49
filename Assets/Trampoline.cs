using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

    float speed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {

        float x = Input.GetAxis("Horizontal");

        transform.localPosition = new Vector3(transform.localPosition.x + (x * speed), transform.localPosition.y, transform.localPosition.z);

        
    }
}
