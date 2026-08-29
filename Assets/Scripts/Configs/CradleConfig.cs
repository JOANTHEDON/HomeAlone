using UnityEngine;
using System;

[Serializable]
public class CradleLevelInfo
{
    public int Level;
    public int CoinSpawnCount;
    public float LevelCoinUpgrade;
   
}

[Serializable]
public class CradleLevelList
{
    public CradleLevelInfo[] _cradleLevelInfo;
}

[CreateAssetMenu(fileName = "CradleConfig", menuName = "Configs/CradleConfig")]
public class CradleConfig : ScriptableObject
{

    public CradleLevelList _cradleLevelList = new  CradleLevelList();
    
}
