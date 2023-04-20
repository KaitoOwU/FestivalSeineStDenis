using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetPlayerAnimationEvent : MonoBehaviour
{

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

    public void ShootEnemy()
    {
        int i = Random.Range(0, 3);
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

}
