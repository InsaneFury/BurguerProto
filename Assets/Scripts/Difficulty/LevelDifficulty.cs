using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty",menuName ="Difficulty",order =0)]
public class LevelDifficulty : ScriptableObject
{
    [Header("Aji Settings")]
    public float ajiSpeed;
    public int ajiDmg;
    public int ajiLife;

    [Header("Tomato Settings")]
    public float tomatoSpeed;
    public int tomatoDmg;
    public int tomatoLife;
}
