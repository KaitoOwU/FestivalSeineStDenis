using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetAnimationEvent : MonoBehaviour
{
    public UnityEvent OnAttack;


    public void Attack()
    {
        if(DialogueManager.instance.DialogueCanva.activeSelf == false)
        {
            OnAttack?.Invoke();
        }
    }
}
