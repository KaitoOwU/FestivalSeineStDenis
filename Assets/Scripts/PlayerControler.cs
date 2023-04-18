using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] Transform _aimPoint;
    [SerializeField] Transform _playerGfx;

    private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    private Player1ShootBehaviour _playerShoot;

    private PlayerInput _inputActions;
    private InputAction _playerMovement;
    private InputAction _playerAim;
    private InputAction _playerValidate;

    private Action OnStartMove;
    private Action OnStopMove;

    private Coroutine MoveRoutine;
    private Coroutine AimRoutine;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerShoot = GetComponent<Player1ShootBehaviour>();

        _inputActions = GetComponent<PlayerInput>();
        _playerMovement = _inputActions.actions["move"];
        _playerAim = _inputActions.actions["aim"];
        _playerValidate = _inputActions.actions["validate"];


        OnStartMove += StartMove;
        OnStartMove += StopMove;

        _playerMovement.started += ff => StartMove();
        _playerMovement.canceled += ff => StopMove();

        _playerAim.started += ff => StartAim();
        _playerAim.canceled += ff => StopAim();

        _playerValidate.started += ff => SkipDialogue();

        _playerMovement.Enable();
        _playerAim.Enable();

        GameManager.instance.OnDialogue += OnPause;
        GameManager.instance.OnGamePause += OnPause;
        GameManager.instance.OnStopDialogue += OnStopPause;
        GameManager.instance.OnGameUnPause += OnStopPause;
    
    }

    private void OnPause()
    {
        StopMove();
        _playerMovement.Disable();
        _playerAim.Disable();
    }

    private void OnStopPause()
    {
        _playerMovement.Enable();
        _playerAim.Enable();
    }

    private void SkipDialogue()
    {
        PlayerManager.instance.SkipDialogue();
    }

    private void OnEnable()
    {
        //_playerMovement.Enable();   
    }
    private void OnDisable()
    {
        _playerMovement.Disable();
        _playerAim.Disable();

        GameManager.instance.OnDialogue -= OnPause;
        GameManager.instance.OnGamePause -= OnPause;
        GameManager.instance.OnStopDialogue -= OnStopPause;
        GameManager.instance.OnGameUnPause -= OnStopPause;
    }

    private void StartMove()
    {
        MoveRoutine = StartCoroutine(PlayerMoveRoutine());
    }

    private void StopMove()
    {
        _rb.velocity = Vector2.zero;
        MoveRoutine = null;
    }

    private void StartAim()
    {
        AimRoutine = StartCoroutine(PlayerAimRoutine());
    }

    private void StopAim()
    {
        StopCoroutine(AimRoutine);
        AimRoutine = null;
    }

    private IEnumerator PlayerMoveRoutine()
    {
        while (true)
        {
            if(_playerShoot.ShootState != Player1ShootBehaviour.SHOOTSTATE.Shooting && _playerShoot.ShootState != Player1ShootBehaviour.SHOOTSTATE.Reload)
            {
                _rb.velocity = _playerMovement.ReadValue<Vector2>() * Time.fixedDeltaTime * _speed;
                _animator.SetFloat("Speed", MathF.Abs( _rb.velocity.x) + MathF.Abs(_rb.velocity.y));
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _animator.SetFloat("Speed", MathF.Abs(_rb.velocity.x) + MathF.Abs(_rb.velocity.y));
            }

            if(_rb.velocity.x > 0)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 0, _playerGfx.eulerAngles.z);
            }
            else if (_rb.velocity.x < 0)
            {
                _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 180, _playerGfx.eulerAngles.z);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator PlayerAimRoutine()
    {
        while (true)
        {
            float aimDirection = Vector3.Angle(new Vector3(0f, 1f, 0f), _playerAim.ReadValue<Vector2>());

            if (_playerAim.ReadValue<Vector2>().x > 0.0f)
            {
                aimDirection = -aimDirection;
                aimDirection = aimDirection + 360;
            }

            _aimPoint.rotation = Quaternion.Euler(0f, 0f, aimDirection);

            yield return null;
        }
    }
}
