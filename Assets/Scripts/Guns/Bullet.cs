using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Gun/Create New Bullet", order = 2)]
public class Bullet : ScriptableObject
{
    [Header("Graphics")]
    public Sprite _bulletSprite;
    public GameObject _bloodEffect;
    
}
