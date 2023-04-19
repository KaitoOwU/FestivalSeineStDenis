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

    [SerializeField] private float _kockBack;
    [SerializeField] private float _damage;


    private float _shootInterval;
    private float _shootRange;

    private int _maxAmmo;
    private float _reloadTime;
    private int _currentAmmo;


    private Animator _animator;
    private AnimationClip _reloadAnimation;

    [SerializeField] private LayerMask _enemyLayer;

    private PlayerControler _playerControler;

    private PlayerInput _inputActions;
    private InputAction _shootInput;
    private InputAction _reloadInput;

    private Coroutine ShootRoutine;
    private Coroutine CoolDownRoutine;
    private Coroutine ReloadRoutine;

    public SHOOTSTATE ShootState { get => _shootState; set => _shootState = value; }
    public InputAction ShootInput { get => _shootInput; set => _shootInput = value; }
    public float ShootInterval { get => _shootInterval; set => _shootInterval = value; }
    public float ShootRange { get => _shootRange; set => _shootRange = value; }
    public int MaxAmmo { get => _maxAmmo; set => _maxAmmo = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public AnimationClip ReloadAnimation { get => _reloadAnimation; set => _reloadAnimation = value; }
    public float ReloadTime { get => _reloadTime; set => _reloadTime = value; }
    public Coroutine ShootRoutine1 { get => ShootRoutine; set => ShootRoutine = value; }

    private void Start()
    {
        _playerControler = GetComponent<PlayerControler>();
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

        _currentAmmo = MaxAmmo;
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
        if(ShootRoutine1 == null)
        {
            ShootRoutine1 = StartCoroutine(PlayerShootRoutine());
        }
    }

    private void StopShoot()
    {
        if(ShootRoutine1 != null)
        {
            Animator.SetBool("IsFire", false);
            if (ShootState != SHOOTSTATE.Reload)
            {
                ShootState = SHOOTSTATE.None;
            }
            StopCoroutine(ShootRoutine1);
            ShootRoutine1 = null;
        }
    }

    private IEnumerator PlayerShootRoutine()
    {
        Animator.SetBool("IsFire", true);
        while(CoolDownRoutine != null || ShootState == SHOOTSTATE.Reload )
        {
            yield return null;
        }

        ShootState = SHOOTSTATE.Shooting;
        while (true)
        {
            while(_currentAmmo <= 0 || ReloadRoutine != null)
            {
                Animator.SetBool("IsFire", false);
                ReloadRoutine = StartCoroutine(ShootReloadRoutine());
                yield return new WaitUntil( () => _currentAmmo == MaxAmmo);
                Animator.SetBool("IsFire", true);
            }
            _playerControler.Rb.velocity = Vector2.zero;
            //Debug.Log(_shootPoint.eulerAngles.z);
            if (_playerControler.JoyStickLastPosition < 0)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 180, _playerGfx.eulerAngles.z);
            }
            else if (_playerControler.JoyStickLastPosition > 0)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 0, _playerGfx.eulerAngles.z);
            }

            RaycastHit2D raycast = Physics2D.Raycast(transform.position, _shootPoint.up, ShootRange, _enemyLayer);
            Debug.DrawRay(transform.position, _shootPoint.up * ShootRange, Color.red, 0.5f);

            if (raycast.collider != null)
            {
                raycast.collider.gameObject.GetComponent<IShootableEnemy>()?.Damage(_damage, _kockBack, ( raycast.collider.transform.position - transform.position).normalized);
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
        yield return new WaitForSeconds(ShootInterval);
        CoolDownRoutine = null;
    }

    private IEnumerator ShootReloadRoutine()
    {
        Animator.SetTrigger("Reload");
        Debug.Log("Reload");
        ShootState = SHOOTSTATE.Reload;

        yield return new WaitForSeconds(ReloadTime);


        _currentAmmo = MaxAmmo;
        ReloadCompleted();
        Debug.Log("EndReload");
    }

}
