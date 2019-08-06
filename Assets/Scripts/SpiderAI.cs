using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class SpiderAI : MonoBehaviour
{
    public float jumpTime = 0.5f;

    public Key currentKey = null;

    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpiderCoroutine());
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable(){
        if(currentKey != null)
            currentKey.occupied = false;
    }

    IEnumerator SpiderCoroutine(){
        yield return new WaitForSeconds(1);
        while(true){
            if(enemy.dead)
                yield break;
            Jump p = FindObjectOfType<Jump>();
            Key target = GameObject.FindObjectsOfType<Key>()
                .Where(k => k.alive && !k.occupied && Vector3.Distance(transform.position, k.transform.position) <= 1)
                .OrderBy(a => Vector3.Distance(a.transform.position, p.transform.position))
                .First();
            if(currentKey != null){
                currentKey.occupied = false;
            }
            target.occupied = true;
            currentKey = target;
            StartCoroutine(JumpCoroutine(target));
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator JumpCoroutine(Key target){
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;
        float time = 0;
        float dist = 999999;
        do{
            if(enemy.dead)
                yield break;
            time += Time.deltaTime;
            dist = Vector3.Distance(transform.position, targetPos);
            transform.position = Vector3.Lerp(startPos, targetPos, time/jumpTime);
            yield return null;
        }while(dist > 0);
    }
}
