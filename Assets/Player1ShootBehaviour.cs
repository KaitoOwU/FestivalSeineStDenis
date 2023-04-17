using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1ShootBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;


    [SerializeField] private float _shootInterval;
    [SerializeField] private float _shootRange;

    [SerializeField] private LayerMask _enemyLayer;

    private PlayerInput _inputActions;
    private InputAction _shootInput;

    private Coroutine ShootRoutine;
    private Coroutine CoolDownRoutine;


    private void Start()
    {
        _inputActions = GetComponent<PlayerInput>();
        _shootInput = _inputActions.actions["fire"];

        _shootInput.started += f => Shoot();
        _shootInput.canceled += f => StopShoot();

        _shootInput.Enable();

        GameManager.instance.OnDialogue += OnPause;
        GameManager.instance.OnGamePause += OnPause;
        GameManager.instance.OnStopDialogue += OnStopPause;
        GameManager.instance.OnGameUnPause += OnStopPause;
    }

    private void OnPause()
    {
        _shootInput.Disable();
    }

    private void OnStopPause()
    {
        _shootInput.Enable();
    }

    private void OnDisable()
    {
        _shootInput.Disable();
    }

    private void Shoot()
    {
        if(ShootRoutine == null)
        {
            ShootRoutine = StartCoroutine(PlayerShootRoutine());
        }
    }

    private void StopShoot()
    {
        if(ShootRoutine != null)
        {
            StopCoroutine(ShootRoutine);
            ShootRoutine = null;
        }
    }

    private IEnumerator PlayerShootRoutine()
    {
        while(CoolDownRoutine != null)
        {
            yield return null;
        }

        while (true)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, _shootPoint.up, _shootRange, _enemyLayer);
            Debug.DrawRay(transform.position, _shootPoint.up * _shootRange, Color.red, 0.5f);

            if (raycast.collider != null)
            {
                raycast.collider.gameObject.GetComponent<IShootableEnemy>()?.Damage(10, 5, ( raycast.collider.transform.position - transform.position).normalized);
                Debug.Log("Toucher");
            }

            CoolDownRoutine = StartCoroutine(ShootCoolDownRoutine());
            yield return new WaitUntil( () => CoolDownRoutine == null);


        }
    }

    private IEnumerator ShootCoolDownRoutine()
    {
        yield return new WaitForSeconds(_shootInterval);
        CoolDownRoutine = null;
    }

}
