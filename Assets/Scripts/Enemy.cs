using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll){
        Jump jump = coll.GetComponent<Jump>();
        if(jump != null && coll.gameObject.tag.Equals("Player")){
            if(jump.state == Jump.PlayerState.JUMPING){
                Die();
            } else {
                jump.Hit();
            }
        }
    }

    public void Die(){
        Destroy(gameObject);
    }
}
