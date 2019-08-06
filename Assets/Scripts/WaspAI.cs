using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspAI : MonoBehaviour
{

    public bool up, right;
    public float movementSpeed = 1.0f;

    public GameObject projectilePrefab;

    private bool shooting = false;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        up = Random.value < 0.5f;
        right = Random.value < 0.5f;
        StartCoroutine(WaspCoroutine()); 
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
            return;
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
        rend.flipX = right;
    }

    IEnumerator WaspCoroutine(){
        while(true){
            shooting = false;
            yield return new WaitForSeconds(2);
            shooting = true;
            StartCoroutine(BlinkCoroutine());
            yield return new WaitForSeconds(2);
            shooting = false;
        }
    }

    IEnumerator BlinkCoroutine(){
        Vector3 playerPos = FindObjectOfType<Jump>().transform.position;
        rend.color = Color.gray;
        yield return new WaitForSeconds(0.2f);
        rend.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        rend.color = Color.gray;
        yield return new WaitForSeconds(0.2f);
        rend.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        Instantiate(projectilePrefab, transform.position + Vector3.back, Quaternion.LookRotation(Vector3.forward, playerPos - transform.position));
    }
}
