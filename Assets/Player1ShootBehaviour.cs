using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1ShootBehaviour : MonoBehaviour
{
    public enum SHOOTSTATE
    {
        Shooting,
        Reload,
        None,
    };
    private SHOOTSTATE _shootState = SHOOTSTATE.None;

    [SerializeField] private Transform _shootPoint;


    [SerializeField] private float _shootInterval;
    [SerializeField] private float _shootRange;

    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _reloadTime;
    private int _currentAmmo;

    [SerializeField] private LayerMask _enemyLayer;

    private PlayerInput _inputActions;
    private InputAction _shootInput;
    private InputAction _reloadInput;

    private Coroutine ShootRoutine;
    private Coroutine CoolDownRoutine;
    private Coroutine ReloadRoutine;

    public SHOOTSTATE ShootState { get => _shootState; set => _shootState = value; }

    private void Start()
    {
        _inputActions = GetComponent<PlayerInput>();
        _shootInput = _inputActions.actions["fire"];
        _reloadInput = _inputActions.actions["reload"];

        _shootInput.started += f => Shoot();
        _shootInput.canceled += f => StopShoot();

        _reloadInput.started += ff => Reload();

        _shootInput.Enable();

        GameManager.instance.OnDialogue += OnPause;
        GameManager.instance.OnGamePause += OnPause;
        GameManager.instance.OnStopDialogue += OnStopPause;
        GameManager.instance.OnGameUnPause += OnStopPause;

        _currentAmmo = _maxAmmo;
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

    private void Reload()
    {
        if(ReloadRoutine == null)
        {
            ReloadRoutine = StartCoroutine(ShootReloadRoutine());
        }
    }

    private void ReloadCompleted()
    {
        if(ReloadRoutine != null)
        {
            StopCoroutine(ReloadRoutine);
            ReloadRoutine = null;
            ShootState = SHOOTSTATE.None;
        }

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
            if(ShootState != SHOOTSTATE.Reload)
            {
                ShootState = SHOOTSTATE.None;
            }
            StopCoroutine(ShootRoutine);
            ShootRoutine = null;
        }
    }

    private IEnumerator PlayerShootRoutine()
    {
        while(CoolDownRoutine != null || ShootState == SHOOTSTATE.Reload )
        {
            yield return null;
        }

        ShootState = SHOOTSTATE.Shooting;
        while (true)
        {
            while(_currentAmmo <= 0)
            {
                ReloadRoutine = StartCoroutine(ShootReloadRoutine());
                yield return new WaitUntil( () => _currentAmmo == _maxAmmo);
            }

            RaycastHit2D raycast = Physics2D.Raycast(transform.position, _shootPoint.up, _shootRange, _enemyLayer);
            Debug.DrawRay(transform.position, _shootPoint.up * _shootRange, Color.red, 0.5f);

            if (raycast.collider != null)
            {
                raycast.collider.gameObject.GetComponent<IShootableEnemy>()?.Damage(10, 5, ( raycast.collider.transform.position - transform.position).normalized);
                Debug.Log("Toucher");
            }

            _currentAmmo--;
            Debug.Log("Ammo: " + _currentAmmo);
            CoolDownRoutine = StartCoroutine(ShootCoolDownRoutine());
            yield return new WaitUntil( () => CoolDownRoutine == null);


        }
    }

    private IEnumerator ShootCoolDownRoutine()
    {
        yield return new WaitForSeconds(_shootInterval);
        CoolDownRoutine = null;
    }

    private IEnumerator ShootReloadRoutine()
    {
        Debug.Log("Reload");
        ShootState = SHOOTSTATE.Reload;
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _maxAmmo;
        ReloadCompleted();
        Debug.Log("EndReload");
    }

}
