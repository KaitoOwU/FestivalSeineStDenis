using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _image;

    [SerializeField] private bool isP1;

    private void Start()
    {
        if (isP1)
        {
            PlayerManager.instance.Players[0].GetComponent<Player1ShootBehaviour>().Ammo = this;
        }
        else
        {
            PlayerManager.instance.Players[1].GetComponent<Player1ShootBehaviour>().Ammo = this;
        }
    }

    public void UpdateImage(float percentage)
    {
        //Debug.Log(percentage);
        if(percentage == 1)
        {
            _image.sprite = _sprites[5];
        }
        else if(percentage >= 0.8)
        {
            _image.sprite = _sprites[4];
        }
        else if (percentage >= 0.6)
        {
            _image.sprite = _sprites[3];
        }
        else if (percentage >= 0.4)
        {
            _image.sprite = _sprites[2];
        }
        else if (percentage >= 0.2)
        {
            _image.sprite = _sprites[1];
        }
        else
        {
            _image.sprite = _sprites[0];
        }
    }
}
