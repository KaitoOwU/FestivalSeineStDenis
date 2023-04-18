using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        MOVING,
        KNOCBACK
    }

    private ENEMYSTATE _enemyState;

    private Transform _target;

    [SerializeField] private float _speed;
    [SerializeField] private float _nextWayPointDistance;

    private Path _path;
    private int _currentWayPoint;
    private bool _recheadEndOfPath;

    private bool _disable;
    private bool _pause;

    private Seeker _seeker;
    private Rigidbody2D _rb;

    [Header("Detection")]
    [SerializeField] private float _detectionSize;
    [SerializeField] private float _attackDetectionSize;
    [SerializeField] private LayerMask _detectionLayer;

    private Coroutine FollowRoutine;
    private Coroutine PathRoutine;

    public ENEMYSTATE EnemyState { get => _enemyState; set => _enemyState = value; }

    private void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        _rb.velocity = Vector3.zero;

        GameManager.instance.OnDialogue += OnPause;
        GameManager.instance.OnGamePause += OnPause;
        GameManager.instance.OnGameUnPause += UnPause;
        GameManager.instance.OnStopDialogue += UnPause;
    }

    private void OnDisable()
    {
        GameManager.instance.OnDialogue -= OnPause;
        GameManager.instance.OnGamePause -= OnPause;
        GameManager.instance.OnGameUnPause -= UnPause;
        GameManager.instance.OnGameUnPause -= UnPause;
    }

    private void OnPause()
    {
        _pause = true;
        _rb.velocity = Vector3.zero;
        if(FollowRoutine != null)
        {
            StopCoroutine(FollowRoutine);
            FollowRoutine = null;
        }

        if (PathRoutine != null)
        {
            StopCoroutine(PathRoutine);
            PathRoutine = null;
        }
        _path = null;
    }

    private void UnPause()
    {
        _pause = false;
    }


    private void Update()
    {
        if (_disable || _pause)
            return;

        Collider2D target = Physics2D.OverlapCircle(transform.position, _detectionSize, _detectionLayer);
        if(target != null && FollowRoutine == null)
        {
            FollowRoutine = StartCoroutine(EnemyFollowRoutine(target.transform));
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWayPoint = 0;
        }
    }


    IEnumerator EnemyFollowRoutine(Transform target)
    {
        _target = target;
        if(PathRoutine != null)
        {
            StopCoroutine(PathRoutine);
            PathRoutine = null;
        }
        PathRoutine = StartCoroutine(EnemyPathRoutine());

        while (true)
        {
            if(_path == null || _currentWayPoint >= _path.vectorPath.Count)
            {
                _recheadEndOfPath = true;
                FollowRoutine = null;
                _rb.velocity = Vector2.zero;
                yield break;
            }
            else
            {
                _recheadEndOfPath = false;
            }

            Vector2 direction = ((Vector2)_path.vectorPath[_currentWayPoint] - _rb.position).normalized;

            if(_enemyState != ENEMYSTATE.KNOCBACK)
            {
                _rb.velocity = direction * _speed * Time.deltaTime;
            }

            float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWayPoint]);

            if(distance < _nextWayPointDistance)
            {
                _currentWayPoint++;
            }

            yield return new WaitForFixedUpdate();
        }

    }
    private IEnumerator EnemyPathRoutine()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(transform.position, _target.position, OnPathComplete);
        yield return new WaitForSeconds(.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDetectionSize);
    }
}
