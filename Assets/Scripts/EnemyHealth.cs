using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IShootableEnemy
{

    [SerializeField] private float _maxHp;
    [SerializeField] private float _knockBackResistance;


    public Action OnDamage;
    public UnityEvent OnDamageGD;

    public Action OnDeath;
    public UnityEvent OnDeathGD;

    private Rigidbody2D _rb;
    private EnemyAI _enemyAI;
    private float _hp;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _hp = _maxHp;
    }
    public void Damage(float damage, float knockBackForce, Vector2 hitDirection)
    {
        Debug.Log("Damage before" + _hp);
        _hp -= damage;
        if(_hp <= 0)
        {
            Death();
            OnDeath?.Invoke();
            OnDeathGD?.Invoke();
        }

        Debug.Log("Damage after" + _hp);
        StartCoroutine(KnockBackRoutine());
        _rb.velocity = (hitDirection * (knockBackForce - _knockBackResistance));
    }


    private void Death()
    {
        Destroy(gameObject);
    }

    private IEnumerator KnockBackRoutine()
    {
        _enemyAI.EnemyState = EnemyAI.ENEMYSTATE.KNOCBACK;
        yield return new WaitForSeconds(0.1f);
        _enemyAI.EnemyState = EnemyAI.ENEMYSTATE.IDLE;
    }
}
