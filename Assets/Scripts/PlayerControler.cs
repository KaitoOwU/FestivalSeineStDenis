using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rb;

    private PlayerControlerInput _inputActions;
    private InputAction _playerMovement;

    private Action OnStartMove;
    private Action OnStopMove;

    private Coroutine MoveRoutine;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _inputActions = new PlayerControlerInput();
        _playerMovement = _inputActions.Player1.Move;
        Debug.Log(_playerMovement);

        OnStartMove += StartMove;
        OnStartMove += StopMove;

        _playerMovement.started += ff => StartMove();
        _playerMovement.canceled += ff => StopMove();

        _playerMovement.Enable();
    
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

    private IEnumerator PlayerMoveRoutine()
    {
        while (true)
        {
            _rb.velocity = _playerMovement.ReadValue<Vector2>() * Time.fixedDeltaTime * _speed;

            yield return new WaitForFixedUpdate();
        }
    }
}
