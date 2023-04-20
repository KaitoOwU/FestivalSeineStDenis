using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerGetUi : MonoBehaviour
{
    [SerializeField] private Slider _layer1SliderP1;
    [SerializeField] private Slider _layer2SliderP1;
    [SerializeField] private Animator _healthBarAnimatorP1;

    [SerializeField] private Slider _layer1SliderP2;
    [SerializeField] private Slider _layer2SliderP2;
    [SerializeField] private Animator _healthBarAnimatorP2;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        PlayerHealth player1 = PlayerManager.instance.Players[0].GetComponent<PlayerHealth>();
        PlayerHealth player2 = PlayerManager.instance.Players[1].GetComponent<PlayerHealth>();

        player1.Layer1Slider = _layer1SliderP1;
        player1.Layer2Slider = _layer2SliderP1;
        player1.HealthBarAnimator = _healthBarAnimatorP1;

        player2.Layer1Slider = _layer1SliderP2;
        player2.Layer2Slider = _layer2SliderP2;
        player2.HealthBarAnimator = _healthBarAnimatorP2;
        Debug.Log("VArianble");
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    private void OnSceneChange(Scene s1, Scene s2)
    {
        if (SceneManager.GetActiveScene().name == "Tom")
        {
            
        }
    }
}
