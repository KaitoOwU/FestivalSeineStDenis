using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IShootableEnemy
{
    [SerializeField] private float _playerMaxHealth;
    private float _playerHealth;

    private Slider _layer1Slider;
    private Slider _layer2Slider;
    private Animator _healthBarAnimator;

    private Coroutine decreaseRoutine;

    public Slider Layer1Slider { get => _layer1Slider; set => _layer1Slider = value; }
    public Slider Layer2Slider { get => _layer2Slider; set => _layer2Slider = value; }
    public Animator HealthBarAnimator { get => _healthBarAnimator; set => _healthBarAnimator = value; }

    private void Start()
    {
        _playerHealth = _playerMaxHealth;
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

        if(_playerHealth < 0)
        {
            _layer1Slider.gameObject.SetActive(false);
            _layer2Slider.gameObject.SetActive(false);
            Debug.Log("t'es mort");
        }
    }

    private IEnumerator DecreaseLifeRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        while(_layer2Slider.value < _layer1Slider.value)
        {
            _layer1Slider.value -= Time.deltaTime/2;
            yield return null;
        }
        _layer1Slider.value = _layer2Slider.value;
        decreaseRoutine = null;
    }
}
