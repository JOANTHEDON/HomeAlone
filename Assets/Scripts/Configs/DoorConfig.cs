using UnityEngine;
using System;

[Serializable]
public class DoorLevelInfo
{
    public int level;
    public int doorHealth;
}

[Serializable]
public class DoorLevelList
{
    public DoorLevelInfo[] _doorLevelInfo;
}


[CreateAssetMenu(fileName = "doorConfig", menuName ="Configs/DoorConfig")]
public class DoorConfig : ScriptableObject
{
    public DoorLevelList _doorLevelList = new DoorLevelList();
}
