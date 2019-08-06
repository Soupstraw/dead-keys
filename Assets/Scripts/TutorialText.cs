using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

using TMPro;

public class TutorialText : MonoBehaviour
{

    public GameObject spiderEnemyPrefab;
    public GameObject waspEnemyPrefab;

    public int roundNum = 0;

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
            .Take(1)
            .ToList()
            .ForEach(k => Instantiate(spiderEnemyPrefab, k.transform.position, Quaternion.identity));
    }

    void RestoreKeys(){
        GameObject.FindObjectsOfType<Key>()
            .Where(k => !k.alive)
            .Take(2)
            .ToList()
            .ForEach(k => k.Restore());
    }

    void SpawnEnemies(){
        // Respawn some keys
        if(roundNum > 0)
            RestoreKeys();

        // Spawn spiders
        GameObject.FindObjectsOfType<Key>()
            .Where(k => k.alive && Vector3.Distance(player.transform.position, k.transform.position) >= 1)
            .OrderBy(a => Guid.NewGuid())
            .Take((int) Mathf.Log(roundNum) + 1)
            .ToList()
            .ForEach(k => Instantiate(spiderEnemyPrefab, k.transform.position, Quaternion.identity));

        // Spawn wasps
        for(int i = 0; i < roundNum/3; i++){
            Vector3 spawnPos = player.transform.position;
            while(Vector3.Distance(player.transform.position, spawnPos) < 2f){
                spawnPos = new Vector3(UnityEngine.Random.Range(-1f, 6f), UnityEngine.Random.Range(-0.7f, 1.5f));
            }
            Instantiate(waspEnemyPrefab, spawnPos, Quaternion.identity);
        }

        roundNum++;
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
        text.text = "You will regain some keys after each round";
        yield return new WaitForSeconds(1);
        RestoreKeys();
        yield return new WaitForSeconds(1);
        text.text = "";
        yield return new WaitForSeconds(1);
        text.text = "Try to get as far as you can";
        yield return new WaitForSeconds(1);
        StartCoroutine(GameLoopCoroutine());
        yield return new WaitForSeconds(1);
        text.text = "";
    }

    IEnumerator GameLoopCoroutine(){
        while(true){
            while(GameObject.FindGameObjectsWithTag("Enemy").Length > 0){
                yield return new WaitForSeconds(0.5f);
                if(GameObject.FindGameObjectWithTag("Player") == null){
                    text.text = "Press any key to try again";
                    while(true){
                        if(Input.anyKeyDown){
                            text.text = "Please wait..";
                            Time.timeScale = 1f;
                            yield return null;
                            SceneManager.LoadScene("MainScene");
                        }
                        yield return null;
                    }
                }
            }
            SpawnEnemies();
        }
    }

    public void Quit(){
        Application.Quit();
    }
}
