using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyCannonScript : EnemyScript
{
    public GameObject bulletPrefab;
    private float fireRate = 4.0f;
    private float nextFire = 1.5f;
    private Vector3 bulletPositionOffset = new Vector3(0.691f, 0.646f, 0);
    void Update()
    {
        nextFire += Time.deltaTime;
        if (nextFire > fireRate)
        {
            nextFire = 0.0f;
            StartCoroutine(WaitAndFire());
        }
    }

    IEnumerator WaitAndFire()
    {
        bool lookright = (player.GetComponent<PlayerMovement>().playerPosition - transform.position).normalized.x > 0;
        transform.rotation = Quaternion.Euler(0, lookright ? 0 : 180, 0);
        yield return new WaitForSeconds(0.2f);
        yield return Fire(lookright);
    }
    IEnumerator Fire(bool lookright){
        transform.DOPunchPosition(new Vector3(lookright? -0.2f: 0.2f, 0, 0), 0.2f, 10, 1);
        yield return new WaitForSeconds(0.2f);
        if (!lookright){
            bulletPositionOffset = new Vector3(-0.691f, bulletPositionOffset.y, bulletPositionOffset.z);
        }
        GameObject bullet = Instantiate(bulletPrefab, transform.position + bulletPositionOffset, transform.rotation);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.PushVector = transform.rotation * Vector3.right;
        bulletScript.fireOrigin = gameObject;
    }
    protected override void TakeDamage()
    {
        base.TakeDamage();
        
        if (health <= 0)
        {
            transform.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InBounce).OnComplete(() => {
                Destroy(gameObject);
            });
        }else{
            transform.DOShakeRotation(0.2f, new Vector3(0, 0, 10), 10, 90, false);
        }
    }
}
