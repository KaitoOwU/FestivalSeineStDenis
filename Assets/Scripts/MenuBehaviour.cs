using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _p1;
    [SerializeField] private GameObject _p2;


    [SerializeField] private GameObject _fade;
    [SerializeField] private GameObject _ship;
    [SerializeField] private GameObject _startButton;


    [SerializeField] private AudioManager _audio;

    public void StartGameRoutine()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        _startButton.SetActive(false);
        _ship.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _p1.SetActive(false);
        _p2.SetActive(false);
        _audio.Play("Ship");
        yield return new WaitForSeconds(1.5f);
        _fade.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _audio.Play("Annonce");
        yield return new WaitForSeconds(3.75f);
        GameManager.instance.OpenGameScene();
    }
}
