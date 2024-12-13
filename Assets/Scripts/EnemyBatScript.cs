using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatScript : EnemyMobScript
{
    private int rollHash = Animator.StringToHash("Roll");
    private bool attacking = false;

    protected override void Update()
    {
        if (!busy)
        {
            animator.SetBool("busy", false);
            Vector3 distance = player.GetComponent<PlayerMovement>().playerPosition - transform.position;
            if (distance.magnitude < aggroRange)
            {   
                if(distance.magnitude < attackRange && (Mathf.Abs(distance.x) < 0.5f || Mathf.Abs(distance.y) < 0.5f)){
                    StartCoroutine(base.StartAttack());
                }else{
                    MoveToPlayerBreadCrumb();

                }
            }
        }else{
            if(!attacking)
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            animator.SetBool("busy", true);
        }
        
    }
    protected override void MoveToPlayerBreadCrumb()
    {
        Vector3 direction = player.GetComponent<PlayerMovement>().playerPosition  - transform.position;
        if (Mathf.Abs(direction.x) <= Mathf.Abs(direction.y))
        {
            direction.y = 0;
        }
        else
        {
            direction.x = 0;
        }
        direction.z = 0;
        GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        attacking = false;
    }


    protected override IEnumerator Attack()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        Vector3 direction = (player.GetComponent<PlayerMovement>().playerPosition  - transform.position).normalized;
        animator.CrossFade(rollHash, 0, 0);
        float rollContinuation= 1.0f; // Duration of the attack in seconds
        float elapsedTime = 0f;
        attacking = true;
        // Determine the primary direction of the roll
        Vector3 playerPosition = player.GetComponent<PlayerMovement>().playerPosition ;
        Vector3 rollDirection = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? new Vector3(direction.x, 0, 0) : new Vector3(0, direction.y, 0);
        bool playerPassed = false;
        while (!(playerPassed && elapsedTime > rollContinuation))
        {
            if (!playerPassed && Vector3.Dot(playerPosition - transform.position, rollDirection) < 0)
            {
                playerPassed = true;
            }
            if(playerPassed){
                elapsedTime += Time.deltaTime;
            }
            GetComponent<Rigidbody2D>().linearVelocity = rollDirection * speed * 2;
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        attacking = false;
    }
}
