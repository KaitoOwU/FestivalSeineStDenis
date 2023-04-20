using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IShootableEnemy, IHealable
{
    [SerializeField] private float _playerMaxHealth;

    [SerializeField] private Dialogue _p1Dead;
    [SerializeField] private Dialogue _p2Dead;

    [SerializeField] private GameObject _canvaDead;

    [Scene]
    [SerializeField] private int _daedScene;

    private float _playerHealth;
    private bool isDead;

    private Slider _layer1Slider;
    private Slider _layer2Slider;
    private Animator _healthBarAnimator;
    private Player _player;

    private Coroutine decreaseRoutine;
    private Coroutine increaseRoutine;

    public Slider Layer1Slider { get => _layer1Slider; set => _layer1Slider = value; }
    public Slider Layer2Slider { get => _layer2Slider; set => _layer2Slider = value; }
    public Animator HealthBarAnimator { get => _healthBarAnimator; set => _healthBarAnimator = value; }

    private void Start()
    {
        _playerHealth = _playerMaxHealth;
        _player = GetComponent<Player>();
    }

    public void Damage(float damage, float knockBackForce, Vector2 hitDirection)
    {
        _playerHealth -= damage;
        _layer2Slider.value = _playerHealth / _playerMaxHealth;
        _healthBarAnimator.SetTrigger("Shake");

        if(decreaseRoutine == null)
        {
            decreaseRoutine = StartCoroutine(DecreaseLifeRoutine());
        }

        if(_playerHealth < 0 && !isDead)
        {
            isDead = true;
            _layer1Slider.gameObject.SetActive(false);
            _layer2Slider.gameObject.SetActive(false);

            if(_player.PlayerType == Player.PLAYER.PLAYER1)
            {
                DialogueManager.instance.QueueDialogue(_p1Dead);
            }
            else
            {
                DialogueManager.instance.QueueDialogue(_p2Dead);
            }
            StartCoroutine(WaitCanva());
            Debug.Log("t'es mort");
        }
    }
    private IEnumerator WaitCanva()
    {
        while (true)
        {
            if(DialogueManager.instance.DialogueCanva.activeSelf == false)
            {
                SceneManager.LoadScene(_daedScene);
                GameManager.instance.OnGamePause?.Invoke();
                GameManager.instance.OpenMenuScene();
            }
            yield return null;
        }
    }
    private IEnumerator DecreaseLifeRoutine()
    {
        yield return new WaitForSeconds(0.75f);
        while(_layer2Slider.value < _layer1Slider.value)
        {
            _layer1Slider.value -= Time.deltaTime/2;
            yield return null;
        }
        _layer1Slider.value = _layer2Slider.value;
        decreaseRoutine = null;
    }

    private IEnumerator IncreaseLifeRoutine()
    {
        while (_layer2Slider.value < _playerHealth / _playerMaxHealth)
        {
            _layer2Slider.value += Time.deltaTime * 2 * Mathf.PI;
            yield return null;
        }
        _layer1Slider.value = _layer2Slider.value;
        increaseRoutine = null;
    }

    public void Heal(float value)
    {
        if(value <= 0)
            return;

        if(_playerHealth + value > _playerMaxHealth)
        {
            _playerHealth = _playerMaxHealth;
        }
        else
        {
            _playerHealth += value;
        }
        if(increaseRoutine == null)
        {
            increaseRoutine = StartCoroutine(IncreaseLifeRoutine());
        }
    }
}
