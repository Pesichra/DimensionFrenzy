using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMobScript : EnemyScript
{
    
    public float speed;
    public float aggroRange;
    public float attackRange;
    public float attackSpeed;
    protected bool busy = false;
    public Animator animator;
    protected readonly int runHash = Animator.StringToHash("Run");
    protected readonly int takeDamageHash = Animator.StringToHash("TakeDamage");
    protected readonly int deathHash = Animator.StringToHash("Death");

    protected override void OnEnable(){
        base.OnEnable();
        busy = false;
    }
    void Update()
    {
        if (!busy)
        {
            animator.SetBool("busy", false);
            Vector3 distance = player.GetComponent<PlayerMovement>().playerPosition - transform.position;
            if (distance.magnitude < aggroRange)
            {   
                if(distance.magnitude < attackRange){
                    StartCoroutine(StartAttack());
                }else{
                    AggroTowardsPlayer();

                }
            }
        }else{
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            animator.SetBool("busy", true);
        }
        
    }

    protected abstract void AggroTowardsPlayer();
    protected virtual IEnumerator StartAttack(){
        busy = true;
        yield return Attack();
        busy = false;
        yield return null;
    }
    protected abstract IEnumerator Attack();

    
    protected virtual IEnumerator Die(){
        busy = true;
        animator.CrossFade(deathHash, 0, 0);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
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

    protected virtual IEnumerator TakeDamageAnimation(){
        busy = true;
        animator.CrossFade(takeDamageHash, 0, 0);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        busy = false;
    }
}
