using UnityEngine;
using System;

[Serializable]
public class TurretLevelInfo
{
    public int Level;
    public float LevelCoinUpgrade;
    public float AttackRange;
    public float FireRate;
    public float ProjectileDamage;
    
}

[Serializable]
public class TurretLevelList
{
    public TurretLevelInfo[] TurretLevelInfos;
}


[CreateAssetMenu(fileName ="TurretConfig", menuName = "Configs/Turret")]
public class TurretConfig : ScriptableObject
{
    public TurretLevelList _turretLevelList = new TurretLevelList();
}
