using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI", menuName = "Enemies/Create New Enemy/Gunner AI", order = 1)]
public class GunnerAI : EnemyAI
{
    [Header("Gunner Specifics")]
    public float _stopDist;
    public float _range;
    public int _bulletsAtOnce;
    public float _bulletSpeed;
    public float _inaccuracy;
    public GameObject _projectilePrefab;
    public Vector2 _projOrigin;
}