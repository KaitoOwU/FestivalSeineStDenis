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
    public float ReloadTime { get => _reloadTime; set => _reloadTime = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public AnimationClip ReloadAnimation { get => _reloadAnimation; set => _reloadAnimation = value; }

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
        if(ShootRoutine == null)
        {
            ShootRoutine = StartCoroutine(PlayerShootRoutine());
        }
    }

    private void StopShoot()
    {
        if(ShootRoutine != null)
        {
            Animator.SetBool("IsFire", false);
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
        Animator.SetBool("IsFire", true);
        while(CoolDownRoutine != null || ShootState == SHOOTSTATE.Reload )
        {
            yield return null;
        }

        ShootState = SHOOTSTATE.Shooting;
        while (true)
        {
            while(_currentAmmo <= 0)
            {
                Animator.SetBool("IsFire", false);
                ReloadRoutine = StartCoroutine(ShootReloadRoutine());
                yield return new WaitUntil( () => _currentAmmo == MaxAmmo);
                Animator.SetBool("IsFire", true);
            }
            //Debug.Log(_shootPoint.eulerAngles.z);
            if (_playerControler.PlayerAim.ReadValue<Vector2>().x < 0)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 180, _playerGfx.eulerAngles.z);
            }
            else if (_playerControler.PlayerAim.ReadValue<Vector2>().x > 0)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 0, _playerGfx.eulerAngles.z);
            }

            RaycastHit2D raycast = Physics2D.Raycast(transform.position, _shootPoint.up, ShootRange, _enemyLayer);
            Debug.DrawRay(transform.position, _shootPoint.up * ShootRange, Color.red, 0.5f);

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
        yield return new WaitForSeconds(ShootInterval);
        CoolDownRoutine = null;
    }

    private IEnumerator ShootReloadRoutine()
    {
        Animator.SetTrigger("Reload");
        Debug.Log("Reload");
        ShootState = SHOOTSTATE.Reload;

        yield return new WaitForSeconds(ReloadAnimation.length);


        _currentAmmo = MaxAmmo;
        ReloadCompleted();
        Debug.Log("EndReload");
    }

}
