using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCac : EnemyAI
{
    [SerializeField] private float _bulletSpeed;
    protected override void AttackLogic()
    {
        Debug.Log("Slack");
    }
}
