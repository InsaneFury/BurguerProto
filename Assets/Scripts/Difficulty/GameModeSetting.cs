using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MenuGameSettings",menuName ="GameSettings",order = 0)]
public class GameModeSetting : ScriptableObject
{
    [Header("Difficulty")]
    public bool easy;
    public bool nightmare;

    [Space]
    [Header("Map")]
    public bool map1;
    public bool map2;
}
