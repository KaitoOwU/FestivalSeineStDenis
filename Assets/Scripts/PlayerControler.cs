using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using static Player;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerControler : MonoBehaviour
{
    private float _speed;

    [SerializeField] Transform _aimPoint;
    [SerializeField] Transform _playerGfx;
    [SerializeField] Transform _playerGfxother;

    private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    private Player1ShootBehaviour _playerShoot;
    private Player _player;

    private PlayerInput _inputActions;
    private InputAction _playerMovement;
    private InputAction _playerAim;
    private InputAction _playerValidate;

    private Action OnStartMove;
    private Action OnStopMove;

    private Coroutine MoveRoutine;
    private Coroutine AimRoutine;

    public float Speed { get => _speed; set => _speed = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public InputAction PlayerAim { get => _playerAim; set => _playerAim = value; }

    private void Start()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
        _playerShoot = GetComponent<Player1ShootBehaviour>();

        _playerGfxother.gameObject.SetActive(false);

        _inputActions = GetComponent<PlayerInput>();
        _playerMovement = _inputActions.actions["move"];
        PlayerAim = _inputActions.actions["aim"];
        _playerValidate = _inputActions.actions["validate"];


        OnStartMove += StartMove;
        OnStartMove += StopMove;

        _playerMovement.started += ff => StartMove();
        _playerMovement.canceled += ff => StopMove();

        PlayerAim.started += ff => StartAim();
        PlayerAim.canceled += ff => StopAim();

        _playerValidate.started += ff => SkipDialogue();

        GameManager.instance.OnDialogue += OnPause;
        GameManager.instance.OnGamePause += OnPause;
        GameManager.instance.OnStopDialogue += OnStopPause;
        GameManager.instance.OnGameUnPause += OnStopPause;
        SceneManager.activeSceneChanged += OnSceneChange;

        _playerMovement.Disable();
        PlayerAim.Disable();

        

    }

    private void OnSceneChange(Scene s1, Scene s2)
    {
        if(SceneManager.GetActiveScene().name == "Tom")
        {
            _playerShoot.ShootInput.Enable();
            _playerGfxother.gameObject.SetActive(true);
            _playerMovement.Enable();
            PlayerAim.Enable();

            if (PlayerManager.instance.Players[0] == gameObject)
            {
                _player.Chara1.SetActive(true);
            }
            else
            {
                _player.Chara2.SetActive(true);
            }
        }
    }

    private void OnPause()
    {
        StopMove();
        _playerMovement.Disable();
        PlayerAim.Disable();
    }

    private void OnStopPause()
    {
        _playerMovement.Enable();
        PlayerAim.Enable();
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
        PlayerAim.Disable();

        GameManager.instance.OnDialogue -= OnPause;
        GameManager.instance.OnGamePause -= OnPause;
        GameManager.instance.OnStopDialogue -= OnStopPause;
        GameManager.instance.OnGameUnPause -= OnStopPause;
        SceneManager.activeSceneChanged -= OnSceneChange;
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
                _rb.velocity = _playerMovement.ReadValue<Vector2>() * Time.fixedDeltaTime * Speed;
                Animator.SetFloat("Speed", MathF.Abs( _rb.velocity.x) + MathF.Abs(_rb.velocity.y));

                if(_playerMovement.ReadValue<Vector2>().x > 0)
                {
                    _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 0, _playerGfx.eulerAngles.z);
                }
                else if (_playerMovement.ReadValue<Vector2>().x < 0)
                {
                    _playerGfx.eulerAngles = new Vector3(_playerGfx.eulerAngles.x, 180, _playerGfx.eulerAngles.z);
                }
            }
            else
            {
                _rb.velocity = Vector2.zero;
                Animator.SetFloat("Speed", MathF.Abs(_rb.velocity.x) + MathF.Abs(_rb.velocity.y));
            }


            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator PlayerAimRoutine()
    {
        while (true)
        {
            float aimDirection = Vector3.Angle(new Vector3(0f, 1f, 0f), PlayerAim.ReadValue<Vector2>());

            if (PlayerAim.ReadValue<Vector2>().x > 0.0f)
            {
                aimDirection = -aimDirection;
                aimDirection = aimDirection + 360;
            }

            _aimPoint.rotation = Quaternion.Euler(0f, 0f, aimDirection);

            yield return null;
        }
    }
}
