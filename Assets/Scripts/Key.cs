using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Key : MonoBehaviour
{
    public string keycode;
    public bool alive = true;
    public bool occupied = false;
    public float dropSpeed = 3.0f;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!keycode.Equals("") && alive){
            KeyCode kc = (KeyCode) Enum.Parse(typeof(KeyCode), keycode);
            anim.SetBool("KeyDown", Input.GetKey(kc));
            if(Input.GetKeyDown(kc)){
                GameObject.FindGameObjectWithTag("Player").GetComponent<Jump>().JumpTo(this);
            }
        }
    }

    public void Drop(){
        alive = false;
        StartCoroutine(DropCoroutine());
    }

    private IEnumerator DropCoroutine(){
        while(!alive){
            transform.position += Vector3.down*Time.deltaTime*dropSpeed;
            yield return null;
        }
    }
}
