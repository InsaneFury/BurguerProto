using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float timePeriod = 2;
    public float height = 30f;
    private float timeSinceStart;
    private Vector3 pivot;
    private bool spawned = false;

    Player player;

    private void Start()
    {
        player = Player.Get();
        pivot = transform.position;
        height /= 2;
        timeSinceStart = (3 * timePeriod) / 4;
    }
    void Update()
    {
        if (spawned)
        {
            GoUp();
        }
        transform.LookAt(player.transform);
    }

    public void GoUp()
    {
        Vector3 nextPos = transform.position;

        nextPos.y = pivot.y + height + height * Mathf.Sin(((Mathf.PI * 2) / timePeriod) * timeSinceStart);
        timeSinceStart += Time.deltaTime;
        transform.position = nextPos;

    }

    public void StartFloating()
    {
        spawned = true;
    }
}
