using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Enemies/Create New Enemy Projectile/Generic Projectile", order = 2)]
public class EnemyProjectile : ScriptableObject
{
    [Header("Graphics")]
    public Sprite _projectileSprite;
    
    public int _damage;
    public int _armorPierce;
}
