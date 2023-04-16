using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private Transform _target;

    [SerializeField] private float _speed;
    [SerializeField] private float _nextWayPointDistance;

    private Path _path;
    private int _currentWayPoint;
    private bool _recheadEndOfPath;

    [SerializeField] private Seeker _seeker;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Detection")]
    [SerializeField] private float _detectionSize;
    [SerializeField] private float _attackDetectionSize;


    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDetectionSize);
    }
}
