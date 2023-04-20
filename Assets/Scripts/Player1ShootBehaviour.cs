using System;
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
    [SerializeField] private Transform _shootPointLaser;
    [SerializeField] private Transform _playerGfx;

    private float _kockBack;
    private float _damage;


    private float _shootInterval;
    private float _shootRange;

    private int _maxAmmo;
    private float _reloadTime;
    private int _currentAmmo;


    private Animator _animator;
    private AnimationClip _reloadAnimation;
    private Player _player;

    private LineRenderer _laser;

    [SerializeField] private LayerMask _enemyLayer;

    private PlayerControler _playerControler;

    private PlayerInput _inputActions;
    private InputAction _shootInput;
    private InputAction _reloadInput;

    private Coroutine ShootRoutine;
    private Coroutine CoolDownRoutine;
    private Coroutine ReloadRoutine;

    private ParticleSystem _reloadParticules;


    public Action OnStartReload;
    public Action OnStopReload;

    public Action OnShoot;
    public Action OnStopShoot;

    public SHOOTSTATE ShootState { get => _shootState; set => _shootState = value; }
    public InputAction ShootInput { get => _shootInput; set => _shootInput = value; }
    public float ShootInterval { get => _shootInterval; set => _shootInterval = value; }
    public float ShootRange { get => _shootRange; set => _shootRange = value; }
    public int MaxAmmo { get => _maxAmmo; set => _maxAmmo = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public AnimationClip ReloadAnimation { get => _reloadAnimation; set => _reloadAnimation = value; }
    public float ReloadTime { get => _reloadTime; set => _reloadTime = value; }
    public Coroutine ShootRoutine1 { get => ShootRoutine; set => ShootRoutine = value; }
    public ParticleSystem ReloadParticules { get => _reloadParticules; set => _reloadParticules = value; }
    public float KockBack { get => _kockBack; set => _kockBack = value; }
    public float Damage { get => _damage; set => _damage = value; }
    public LineRenderer Laser { get => _laser; set => _laser = value; }

    private void Start()
    {
        _player = GetComponent<Player>();
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
            if (_player.PlayerType == Player.PLAYER.PLAYER1)
            {
                _laser.gameObject.SetActive(false);
            }

            OnStopShoot?.Invoke();
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
        if (_player.PlayerType == Player.PLAYER.PLAYER1)
        {
            _laser.gameObject.SetActive(true);
        }
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


            if(raycast.collider != null && _player.PlayerType == Player.PLAYER.PLAYER1)
            {
                DrawLaser(_shootPointLaser.position, raycast.point);
            }
            else if(_player.PlayerType == Player.PLAYER.PLAYER1)
            {
                DrawLaser(transform.position + _shootPoint.up * 1.5f, _shootPoint.up * ShootRange);
            }
            OnShoot?.Invoke();

            if (raycast.collider != null)
            {
                raycast.collider.gameObject.GetComponent<IShootableEnemy>()?.Damage(Damage, KockBack, ( raycast.collider.transform.position - transform.position).normalized);
                Debug.Log("Toucher");
                Debug.Log("KB" + KockBack);
            }

            _currentAmmo--;
            //Debug.Log("Ammo: " + _currentAmmo);
            CoolDownRoutine = StartCoroutine(ShootCoolDownRoutine());
            yield return new WaitUntil( () => CoolDownRoutine == null);


        }
    }

    private void DrawLaser(Vector2 startPos, Vector2 endPos)
    {
        _laser.SetPosition(0, startPos);
        _laser.SetPosition(1, endPos);
    }

    private IEnumerator ShootCoolDownRoutine()
    {
        yield return new WaitForSeconds(ShootInterval);
        CoolDownRoutine = null;
    }

    private IEnumerator ShootReloadRoutine()
    {
        OnStartReload?.Invoke();
        Animator.SetTrigger("Reload");
        Debug.Log("Reload");
        ShootState = SHOOTSTATE.Reload;

        yield return new WaitForSeconds(ReloadTime * 0.5f);

        OnStopReload?.Invoke();
        yield return new WaitForSeconds(ReloadTime * 0.5f);

        _currentAmmo = MaxAmmo;
        ReloadCompleted();
        Debug.Log("EndReload");
    }

}
