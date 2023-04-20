using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : EnemyAI
{

    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletSpeed;
    public override void AttackLogic()
    {
        GameObject bullet =  Instantiate(_bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletBehaviour>().StartBehaviour((_target.position - transform.position).normalized * _bulletSpeed);
        Debug.Log("Pan");
    }
}
