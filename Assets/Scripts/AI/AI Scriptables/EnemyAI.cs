using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI", menuName = "Enemies/Create New Enemy/Generic AI", order = 1)]
public class EnemyAI : ScriptableObject
{
    [Header("Main")]
    public string _enemyName;
    public enum EnemyType
    {
        None,
        Pursuer,
        Turret,
        Sniper,
        Charger,
        Custom
    }
    public EnemyType _enemyType;
    public int _enemyID;
    //public var enemyBehaviour;

    [Header("Stats")]
    public int _enemyHP;
    public int _enemyArmor;
    public int _enemyDamage;
    public int _enemyArmorPierce;
    public float _enemySpeed;
    public float _enemyAttackRate;

    [Header("Behavior")]
    public bool _doesContactDamage;
    public int _contactDamage;
    public int _contactAP;

    [Header("Graphics and Prefabs")]
    public Sprite _enemySprite;
}
