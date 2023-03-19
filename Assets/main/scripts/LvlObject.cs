using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LvlObject", menuName = "Balls/LvlObject")]
public class LvlObject : ScriptableObject
{
    public int SceneIndex;
    public bool IsComplite;
}
