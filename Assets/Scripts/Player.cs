using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PLAYER
    {
        PLAYER1,
        PLAYER2,
    };

    [SerializeField] private PLAYER playerType;

    private PlayerManager _playerManager;
    private PlayerControler _playerControler;
    private PlayerHealth _playerHealth;

    [SerializeField] private GameObject _chara1;
    [SerializeField] private GameObject _chara2;

    [Header("player 1 stats")]
    [SerializeField] private float _speed;

    [SerializeField] private AnimationClip _p1AnimationClip;
    [SerializeField] private Animator _p1Animatior;

    [SerializeField] private float _shootInterval;
    [SerializeField] private float _shootRange;

    [SerializeField] private int _maxAmmo;

    // Health
    [SerializeField] private Slider _layer1SliderP1;
    [SerializeField] private Slider _layer2SliderP1;
    [SerializeField] private Animator _healthBarAnimatorP1;


    [Header("player 2 stats")]

    [SerializeField] private float _speed2;
    [SerializeField] private AnimationClip _p2AnimationClip;
    [SerializeField] private Animator _p2Animatior;

    [SerializeField] private float _shootInterval2;
    [SerializeField] private float _shootRange2;

    [SerializeField] private int _maxAmmo2;

    //Health
    [SerializeField] private Slider _layer1SliderP2;
    [SerializeField] private Slider _layer2SliderP2;
    [SerializeField] private Animator _healthBarAnimatorP2;

    private Player1ShootBehaviour _shootComponent;

    public PlayerManager PlayerManager { get => _playerManager; }
    public PLAYER PlayerType { get => playerType; set => playerType = value; }
    public GameObject Chara1 { get => _chara1; set => _chara1 = value; }
    public GameObject Chara2 { get => _chara2; set => _chara2 = value; }

    private void Start()
    {
        _shootComponent = GetComponent<Player1ShootBehaviour>();
        _playerControler = GetComponent<PlayerControler>();
        _playerHealth = GetComponent<PlayerHealth>();

        _playerManager = PlayerManager.instance;
        transform.parent = _playerManager.transform;
        _playerManager.Players.Add(gameObject);
        if(_playerManager.Players.Count == 1)
        {
            playerType = PLAYER.PLAYER1;
        }
        else
        {
            playerType = PLAYER.PLAYER2;
        }

        if(playerType == PLAYER.PLAYER1)
        {
            _playerControler.Speed = _speed;
            _playerControler.Animator = _p1Animatior
                ;
            _shootComponent.ShootInterval = _shootInterval;
            _shootComponent.ShootRange = _shootRange;
            _shootComponent.MaxAmmo = _maxAmmo;

            _shootComponent.Animator = _p1Animatior;
            _shootComponent.ReloadTime = _p1AnimationClip.length;

            
        }
        else
        {
            _playerControler.Speed = _speed2;
            _playerControler.Animator = _p2Animatior;

            _shootComponent.ShootInterval = _shootInterval2;
            _shootComponent.ShootRange = _shootRange2;
            _shootComponent.MaxAmmo = _maxAmmo2;

            _shootComponent.Animator = _p2Animatior;
            _shootComponent.ReloadTime = _p2AnimationClip.length + 0.2f;

        }

        


    }

}
