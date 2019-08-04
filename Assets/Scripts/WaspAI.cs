using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspAI : MonoBehaviour
{

    public bool up, right;
    public float movementSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(right ? 1 : -1, up ? 1 : -1) * Time.deltaTime * movementSpeed;
        if (transform.position.x < -0.7f){
            right = true;
        }
        if (transform.position.x > 5.5f){
            right = false;
        }
        if (transform.position.y < -1){
            up = true;
        }
        if (transform.position.y > 1.5f){
            up = false;
        }
    }
}
