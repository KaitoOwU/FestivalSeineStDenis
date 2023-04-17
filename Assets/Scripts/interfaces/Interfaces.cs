using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IShootableEnemy
{
    void Damage(float damage, float knockBackForce, Vector2 hitDirection);
}
