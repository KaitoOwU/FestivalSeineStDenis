using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] Transform _aimPoint;

    private Rigidbody2D _rb;

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
    
    }

    private void SkipDialogue()
    {
        PlayerManager.instance.SkipDialogue();
    }

    private void OnEnable()
    {
        //_playerMovement.Enable();   
    }
    private void Update()
    {
        //Debug.Log(_playerMovement.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        _playerMovement.Disable();
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
            _rb.velocity = _playerMovement.ReadValue<Vector2>() * Time.fixedDeltaTime * _speed;

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
