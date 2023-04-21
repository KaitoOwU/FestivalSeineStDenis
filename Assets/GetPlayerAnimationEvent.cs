using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetPlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    private int _shootindex;
    public void Reload1P1()
    {
        AudioManager.instance.Play("Reload1P1");
    }

    public void Reload2P1()
    {
        AudioManager.instance.Play("Reload2P1");
    }

    public void Reload1P2()
    {
        AudioManager.instance.Play("Reload1P2");
    }

    public void Reload2P2()
    {
        AudioManager.instance.Play("Reload2P2");
    }

    public void Slash()
    {
        if (DialogueManager.instance.DialogueCanva.activeSelf == true)
        {
            return;
        }
        AudioManager.instance.Play("Slash");
    }

    public void SlashReverse()
    {
        if (DialogueManager.instance.DialogueCanva.activeSelf == true)
        {
            return;
        }
        AudioManager.instance.Play("SlashReverse");
    }

    public void ShootEnemy()
    {
        if (DialogueManager.instance.DialogueCanva.activeSelf == true)
        {
            return;
        }
        int i = _shootindex % 3;
        _shootindex++;
        switch (i)
        {
            case 0:
                AudioManager.instance.Play("EShoot1");
                break;
            case 1:
                AudioManager.instance.Play("EShoot2");
                break;
            case 2:
                AudioManager.instance.Play("EShoot3");
                break;

        }
    }

    public void ShootPlayer()
    {
        
        int i = _shootindex % 3;
        _shootindex++;
        switch (i)
        {
            case 0:
                AudioManager.instance.Play("PShoot1");
                break;
            case 1:
                AudioManager.instance.Play("PShoot2");
                break;
            case 2:
                AudioManager.instance.Play("PShoot3");
                break;

        }
    }

    public void ShootPlayer1()
    {
        AudioManager.instance.Play("P1Shoot");
    }

    public void StopShootPlayer1()
    {
        AudioManager.instance.Stop("P1Shoot");
    }


    public void ChooseP1()
    {
        audioManager.Play("P1");
    }

    public void ChooseP2()
    {
        audioManager.Play("P2");
    }

}
