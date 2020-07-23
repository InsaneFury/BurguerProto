using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Settings")]
    public float Margin;
    public float Layer;
    public float Easing = 0.8f;

    [Space]
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] Vector2 parallaxPosition;
    [SerializeField] Vector3 pos;
    void Start() => pos = transform.position;
    
    void Update()
    {
        DoParallaxVFX();
    }

    void DoParallaxVFX()
    {
        parallaxPosition = new Vector2((Input.mousePosition.x - Screen.width / 2f) - x, (Input.mousePosition.y - Screen.height / 2f) - y);

        x += parallaxPosition.x * Easing * Time.deltaTime;
        y += parallaxPosition.y * Easing * Time.deltaTime;

        Vector3 direction = new Vector3(x, y, 0f);
        Vector3 depth = new Vector3(0f, 0f, Layer);
        transform.position = pos - direction / 500f * Margin + depth;
    }
}
