using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Jump : MonoBehaviour
{
    public enum PlayerState{
        IDLE,
        JUMPING,
        DEAD
    }

    public float jumpTime = 0.5f;
    public PlayerState state = PlayerState.IDLE;
    public Key currentKey = null;
    public Key nextKey = null;

    public Color deathColor;
    public AudioClip hitSound;

    public SpriteRenderer sprite;
    public AudioClip jumpSound;

    private Animator anim;
    private AudioSource audioSource;

    void Start(){
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void JumpTo(Key key){
        if (state == PlayerState.IDLE && currentKey != key){
            nextKey = key;
            state = PlayerState.JUMPING;
            anim.SetTrigger("Jump");
        } else if (state == PlayerState.JUMPING){
            nextKey = key;
        }
    }

    public void DoJump(){
        if(state == PlayerState.JUMPING){
            audioSource.PlayOneShot(jumpSound);
            StartCoroutine(JumpToCoroutine(nextKey.transform.position));
        }
    }

    public void Hit(){
        StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine(){
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        Time.timeScale = 0.5f;
        sprite.color = deathColor;
        sprite.flipY = true;
        state = PlayerState.DEAD;
        Vector3 moveVec = new Vector3(Random.Range(-1f, 1f), 3f);
        float shakeIntensity = 1f;
        ScreenShake shake = FindObjectOfType<ScreenShake>();
        while(transform.position.y > -3f){
            transform.position += moveVec * Time.deltaTime;
            moveVec += Vector3.down * Time.deltaTime * 10;
            shakeIntensity -= Time.deltaTime;
            shake.intensity = Mathf.Clamp01(shakeIntensity);
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator JumpToCoroutine(Vector3 targetPos){
        sprite.flipX = targetPos.x < transform.position.x;

        if (currentKey != null){
            currentKey.Drop();
        }
        currentKey = nextKey;
        nextKey = null;
        Vector3 startPos = transform.position;
        float time = 0;
        float rel_pos = 0;
        while(rel_pos < 1.0){
            time += Time.deltaTime;
            rel_pos = time/jumpTime;
            transform.position = Vector3.Lerp(startPos, targetPos, rel_pos);
            yield return null;
        }
        state = PlayerState.IDLE;
        if(nextKey != null){
            JumpTo(nextKey);
        }
    }
}
