using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPools
{
    [Header("Lists and Arrays")]
    public static Gun[] allGuns;
    public static List<Gun> allPistols = new List<Gun>();
    public static List<Gun> allAssaultRifles = new List<Gun>();
    public static List<Gun> allLMGs = new List<Gun>();
    public static List<Gun> allSMGs = new List<Gun>();
    public static List<Gun> allShotguns = new List<Gun>();
    public static List<Gun> allDMRs = new List<Gun>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void AssignPools()
    {
        allGuns = Resources.LoadAll<Gun>("Data");
        foreach(Gun g in allGuns)
        {
            switch(g._gunType)
            {
                case Gun.GunType.Pistol:
                allPistols.Add(g);
                break;
                case Gun.GunType.AssaultRifle:
                allAssaultRifles.Add(g);
                break;
                case Gun.GunType.MachineGun:
                allLMGs.Add(g);
                break;
                case Gun.GunType.SubmachineGun:
                allSMGs.Add(g);
                break;
                case Gun.GunType.Shotgun:
                allShotguns.Add(g);
                break;
                case Gun.GunType.DMR:
                allDMRs.Add(g);
                break;
            }
        }
    }
}

