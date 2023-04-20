using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] UnityEvent OnAreaEnter;
    [SerializeField] UnityEvent OnAreaExit;

    [SerializeField] private Vector2 _topRight;
    [SerializeField] private Vector2 _botleft;

    private bool _isEntered;
    private bool _isExited;

    [SerializeField] private LayerMask _layer;


    //private void Start()
    //{
    //    Invoke("Prout", 1f);
    //}

    //private void Prout() => GameManager.instance.OpenMenuScene();
    private void Update()
    {
        if (_isExited)
            return;

        Collider2D[] numPlayer = Physics2D.OverlapAreaAll(new Vector2(transform.position.x, transform.position.y) + _topRight, new Vector2(transform.position.x, transform.position.y) + _botleft, _layer);

        if(!_isEntered && numPlayer.Length >= 2)
        {
            _isEntered = true;
            OnAreaEnter?.Invoke();
        }
        else if(_isEntered && !_isExited && numPlayer.Length < 2)
        {
            OnAreaExit?.Invoke();
            _isExited = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        CustomDebug.DrawRectange(new Vector2(transform.position.x, transform.position.y) + _topRight, new Vector2(transform.position.x, transform.position.y) + _botleft);
    }

    public void Porut()
    {
        Debug.Log("ss");
    }

    public void Porsut()
    {
        Debug.Log("ssss");
    }
}
