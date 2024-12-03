using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BulletScript : EnemyScript
{
    public float turnRate;
    public float turnRateDecline;
    public float speed;
    public Vector3 PushVector;
    private bool deflected = false;
    public GameObject fireOrigin;
    void Start()
    {
        StartCoroutine(ActivateCollider());
    }
    // Update is called once per frame
    void Update()
    {
        if (!deflected)
        {
            turnRate = turnRate / Mathf.Pow(2, Time.deltaTime);
            Vector3 direction = (player.GetComponent<PlayerMovement>().playerPosition - transform.position).normalized;
            PushVector = Vector3.RotateTowards(PushVector, direction, turnRate * Time.deltaTime, 0.0f);
        }
        float angle = Mathf.Atan2(PushVector.y, PushVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, -angle));
        transform.position = transform.position + PushVector * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            
            deflected = true;
            if (!fireOrigin.gameObject)
            {
                Destroy(gameObject);
            }else{
                PushVector = (fireOrigin.transform.position - transform.position).normalized;
                speed = speed * 2;
            }
            
        }else{
            Destroy(gameObject);
        }
    }
    protected override void OnEnable(){
        base.OnEnable();
        if(!GetComponent<Collider2D>().enabled){
            StartCoroutine(ActivateCollider());
        }
    }

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider2D>().enabled = true;
    }
}
