using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "EnemyAI", menuName = "Enemies/Create New Enemy/Boss AI", order = 1)]
public class BossAI : EnemyAI
{
    [Header("Boss Specifics")]
    public EventReference _bossMusic;

    public string _bossTitle;
    public int _numPhases;
}