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
        THROWING
    }

    public float jumpTime = 0.5f;
    public PlayerState state = PlayerState.IDLE;
    public Key currentKey = null;
    public Key nextKey = null;

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
        Debug.Log("Player hit!");
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
