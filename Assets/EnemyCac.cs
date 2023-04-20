using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCac : EnemyAI
{
    [SerializeField] private float _damage;
    public  override void AttackLogic()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _attackDetectionSize, _detectionLayer);
        if(hit != null)
        {
            foreach(Collider2D c in hit)
            {
                c.GetComponent<IShootableEnemy>()?.Damage(_damage, 0, Vector2.zero);
            }
        }
    }
}
