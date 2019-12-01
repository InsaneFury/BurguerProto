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

    private void OnEnable()
    {
        Enemy.OnDieAction += AddEnemyKilled;
        Enemy.OnDieAction += AddScore;
    }
    private void OnDisable()
    {
        Enemy.OnDieAction -= AddEnemyKilled;
        Enemy.OnDieAction -= AddScore;
    }

    public void AddScore(Enemy e)
    {
        int randScore = Random.Range(10, 1000);
        score += randScore;
    }

    public void AddEnemyKilled(Enemy e)
    {
        enemiesKilled++;
    }

    public void SetMaxWave(int wave)
    {
        maxWave = wave;
    }
}
