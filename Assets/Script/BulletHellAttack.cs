using UnityEngine;
using System;

public class BulletHellAttack : EnemyAttackBase
{
    public GameObject bulletPrefab;
    public int bulletCount = 8;
    public float fireInterval = 1f;
    public float bulletSpeed = 5f;
    public int StopAttackTime = 3; 

    private bool attacking = false;
    public Action OnAttackFinished;

    public override void StartAttack()
    {
        attacking = true;
        InvokeRepeating(nameof(Fire), 0f, fireInterval);

        if (StopAttackTime > 0)
            Invoke(nameof(StopAttack), StopAttackTime);
    }

    public override void StopAttack()
    {
        if (!attacking) return;

        attacking = false;
        CancelInvoke(nameof(Fire));
        Debug.Log("Enemy attack stopped.");
        OnAttackFinished?.Invoke();
    }

    void Fire()
    {
        if (!attacking) return;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360f / bulletCount);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet b = bullet.GetComponent<Bullet>();
            b.direction = dir;
            b.speed = bulletSpeed;
        }
    }
}
