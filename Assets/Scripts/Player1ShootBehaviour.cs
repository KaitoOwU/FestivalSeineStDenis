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
    [SerializeField] private Transform _playerGfx;


    [SerializeField] private float _shootInterval;
    [SerializeField] private float _shootRange;

    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _reloadTime;
    private int _currentAmmo;

    [SerializeField] private LayerMask _enemyLayer;

    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _reloadAnimation;

    private PlayerInput _inputActions;
    private InputAction _shootInput;
    private InputAction _reloadInput;

    private Coroutine ShootRoutine;
    private Coroutine CoolDownRoutine;
    private Coroutine ReloadRoutine;

    public SHOOTSTATE ShootState { get => _shootState; set => _shootState = value; }
    public InputAction ShootInput { get => _shootInput; set => _shootInput = value; }

    private void Start()
    {
        _inputActions = GetComponent<PlayerInput>();
        ShootInput = _inputActions.actions["fire"];
        _reloadInput = _inputActions.actions["reload"];

        ShootInput.started += f => Shoot();
        ShootInput.canceled += f => StopShoot();

        _reloadInput.started += ff => Reload();

        ShootInput.Enable();

        GameManager.instance.OnDialogue += OnPause;
        GameManager.instance.OnGamePause += OnPause;
        GameManager.instance.OnStopDialogue += OnStopPause;
        GameManager.instance.OnGameUnPause += OnStopPause;

        _currentAmmo = _maxAmmo;
        ShootInput.Disable();
    }

    private void OnPause()
    {
        ShootInput.Disable();
    }

    private void OnStopPause()
    {
        ShootInput.Enable();
    }

    private void OnDisable()
    {
        ShootInput.Disable();
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
            _animator.SetBool("IsFire", false);
            if (ShootState != SHOOTSTATE.Reload)
            {
                ShootState = SHOOTSTATE.None;
            }
            StopCoroutine(ShootRoutine);
            ShootRoutine = null;
        }
    }

    private IEnumerator PlayerShootRoutine()
    {
        _animator.SetBool("IsFire", true);
        while(CoolDownRoutine != null || ShootState == SHOOTSTATE.Reload )
        {
            yield return null;
        }

        ShootState = SHOOTSTATE.Shooting;
        while (true)
        {
            while(_currentAmmo <= 0)
            {
                _animator.SetBool("IsFire", false);
                ReloadRoutine = StartCoroutine(ShootReloadRoutine());
                yield return new WaitUntil( () => _currentAmmo == _maxAmmo);
                _animator.SetBool("IsFire", true);
            }
            Debug.Log(_shootPoint.eulerAngles.z);
            if (_shootPoint.eulerAngles.z <= 180)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 180, _playerGfx.eulerAngles.z);
            }
            else if (_shootPoint.eulerAngles.z > 180)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 0, _playerGfx.eulerAngles.z);
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
        _animator.SetTrigger("Reload");
        Debug.Log("Reload");
        ShootState = SHOOTSTATE.Reload;

        yield return new WaitForSeconds(_reloadAnimation.length);


        _currentAmmo = _maxAmmo;
        ReloadCompleted();
        Debug.Log("EndReload");
    }

}
