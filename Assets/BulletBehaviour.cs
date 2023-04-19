using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _damage;
    [SerializeField] private float _hitBoxRadius;
    [SerializeField] private LayerMask _playerLayer;

    private Rigidbody2D _rb;
    private Coroutine _bulletRoutine;
    private Coroutine _lifeRoutine;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lifeRoutine = StartCoroutine(LifeRoutine());
    }

    public void StartBehaviour(Vector2 dir)
    {
        _bulletRoutine = StartCoroutine(BulletRoutine(dir));
    }

    private IEnumerator BulletRoutine(Vector2 dir)
    {
        _rb.velocity = dir;

        while (true)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, _hitBoxRadius, _playerLayer);
            if(hit != null)
            {
                hit.GetComponent<IShootableEnemy>()?.Damage(_damage, 0, Vector2.zero);
                Destroy(gameObject);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _hitBoxRadius);
    }
}
