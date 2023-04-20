using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealComponentBehaviour : MonoBehaviour
{
    [SerializeField] private float _trackRadius;
    [SerializeField] private float _colliderRadius;
    [SerializeField] private float _speed
        ;
    [SerializeField] private LayerMask _player;
    [SerializeField] private Rigidbody2D _rb;

    private float _healValue;

    public float HealValue { get => _healValue; set => _healValue = value; }

    private void Start()
    {
        StartCoroutine(HealTrackRoutine());
    }

    private IEnumerator HealTrackRoutine()
    {
        while (true)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, _trackRadius, _player);
            if (hit != null)
            {
                _rb.velocity = (hit.transform.position - transform.position).normalized * Time.deltaTime * _speed;

                Collider2D hit2 = Physics2D.OverlapCircle(transform.position, _colliderRadius, _player);
                if(hit2 != null)
                {
                    hit2.GetComponent<IHealable>()?.Heal(_healValue);
                    Destroy(gameObject);
                }
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _trackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _colliderRadius);
    }
}
