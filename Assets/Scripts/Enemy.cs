using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{

    public int health = 1;
    public bool dead = false;
    public AudioClip hitAudio;
    public Color hitColor, deathColor;

    private SpriteRenderer rend;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll){
        Jump jump = coll.GetComponent<Jump>();
        if(jump != null && coll.gameObject.tag.Equals("Player")){
            if(jump.state == Jump.PlayerState.JUMPING){
                StartCoroutine(HitCoroutine());
            } else {
                jump.Hit();
            }
        }
    }

    IEnumerator HitCoroutine(){
        health--;
        audioSource.PlayOneShot(hitAudio);
        FindObjectOfType<ScreenShake>().intensity = 0.2f;
        if(health == 0){
            foreach (Collider2D c in GetComponentsInChildren<Collider2D>()){
                c.enabled = false;
            }
            rend.color = deathColor;
            rend.flipY = true;
            dead = true;
            Vector3 moveVec = new Vector3(Random.Range(-1f, 1f), 3f);
            while(transform.position.y > -3f){
                transform.position += moveVec * Time.deltaTime;
                moveVec += Vector3.down * Time.deltaTime * 10;
                yield return null;
            }
            Destroy(gameObject);
        }else{
            rend.color = hitColor;
            yield return new WaitForSeconds(0.5f);
            rend.color = Color.white;
        }
    }
}
