using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float intensity = 1f;

    private Vector3 startPos;

    void Start(){
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPos + intensity * new Vector3(Mathf.Cos(30.0352f*Time.time), Mathf.Cos(60.8236f*Time.time));
        intensity -= Time.deltaTime;
        intensity = Mathf.Max(0, intensity);
    }
}
