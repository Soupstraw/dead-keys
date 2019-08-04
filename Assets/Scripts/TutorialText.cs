using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using TMPro;

public class TutorialText : MonoBehaviour
{

    public GameObject spiderEnemyPrefab;

    private TextMeshPro text;
    private Jump player;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Jump>();
        StartCoroutine(TutorialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnTutorialEnemies(){
        GameObject.FindObjectsOfType<Key>()
            .Where(k => k.alive && Vector3.Distance(player.transform.position, k.transform.position) >= 1)
            .OrderBy(a => Guid.NewGuid())
            .Take(2)
            .ToList()
            .ForEach(k => Instantiate(spiderEnemyPrefab, k.transform.position, Quaternion.identity));
    }

    private IEnumerator TutorialCoroutine(){
        text.text = "Press any key to begin..";
        while(player.state == Jump.PlayerState.IDLE){
            yield return null;
        }
        text.text = "";
        yield return new WaitForSeconds(2);
        text.text = "You can hop to each key only once";
        yield return new WaitForSeconds(3);
        text.text = "";
        yield return new WaitForSeconds(1);
        text.text = "Hop through enemies to kill them";
        yield return new WaitForSeconds(1);
        SpawnTutorialEnemies();
        while (FindObjectsOfType<Enemy>().Length > 0){
            yield return null;
        }
        text.text = "";
        yield return new WaitForSeconds(1);
        text.text = "Good job";
        yield return new WaitForSeconds(2);
        text.text = "";
        yield return new WaitForSeconds(1);
        text.text = "Destroy this minotaur";
    }
}
