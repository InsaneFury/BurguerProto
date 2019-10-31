using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonobehaviourSingleton<ScoreManager>
{
    public int score = 0;
    public int enemiesKilled = 0;
    public int maxWave = 0;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void AddEnemyKilled()
    {
        enemiesKilled++;
    }

    public void SetMaxWave(int wave)
    {
        maxWave = wave;
    }
}
