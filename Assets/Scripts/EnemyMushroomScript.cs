using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMushroomScript : EnemyScript
{
    public float speed;
    public float attackSpeed;
    private bool busy = false;
    public Animator animator;
    void Update()
    {
        if (!busy)
        {
            animator.SetBool("busy", false);
            Vector3 distance = player.GetComponent<PlayerMovement>().playerPosition - transform.position;
            if (Mathf.Abs(distance.y) < 0.3 && Mathf.Abs(distance.x) < 1)
            {
                StartCoroutine(Attack());
            }
            else
            {   
                bool lookright = (player.GetComponent<PlayerMovement>().playerPosition - transform.position).normalized.x > 0;
                transform.rotation = Quaternion.Euler(0, lookright ? 0 : 180, 0);
                
                Vector2 moveDirection = (player.GetComponent<PlayerMovement>().playerPosition - transform.position).normalized;
                GetComponent<Rigidbody2D>().velocity = moveDirection * speed; // cainos movement
                //transform.Translate(moveDirection * speed * Time.deltaTime, Space.World); // my movement
            }
        }else{
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            animator.SetBool("busy", true);
        }
        
    }
    protected override void OnEnable(){
        base.OnEnable();
        busy = false;
    }

    IEnumerator Attack(){
        animator.SetTrigger("Attack");
        busy = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        busy = false;
    }
    IEnumerator Die(){
        busy = true;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
    IEnumerator TakeDamageAnimation(){
        busy = true;
        animator.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        busy = false;
    }

    protected override void TakeDamage()
    {
        busy = true;
        base.TakeDamage();
        
        if (health <= 0)
        {
            StartCoroutine(Die());
            
        }else{
            StartCoroutine(TakeDamageAnimation());
        }
    }

}
