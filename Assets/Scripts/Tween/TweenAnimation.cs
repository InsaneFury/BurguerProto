/*Tween Animation - By Ivan Castellano v0.1
 All needs be refactorized to SOLID.

---------How To use it---------
Simple add the script to a gameobject or ui and use it.
IMPORTANT: To use with childs option you need to create "animated" tag, and put it for the childs you want to animate
this is like that to prevent errors like ui mask types etc.

---------Next Features---------
-Single and child rotation,scale,position tween capability
-Add events to OnEnter, OnPlaying & OnExit tweening states
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class TweenAnimation : MonoBehaviour
{
    [Header("In Settings")]
    public LeanTweenType inEaseType;
    public float inDuration = .3f;
    public float inDelay = .0f;
    public bool customInCurve = false;
    public AnimationCurve inCurve;
    
    [Space]
    [Header("Out Settings")]
    public LeanTweenType outEaseType;
    public float outDuration = .3f;
    public float outDelay = .0f;
    public bool customOutCurve = false;
    public AnimationCurve outCurve;

    [Space]
    [Header("Child Settings")]
    public bool animateChilds = false;
    public bool randomDuration = true;
    [Range(0.1f,10.0f)]
    public float inMinDuration;
    [Range(0.1f, 10.0f)]
    public float inMaxDuration;
    [Range(0.1f, 10.0f)]
    public float outMinDuration;
    [Range(0.1f, 10.0f)]
    public float outMaxDuration;
    [SerializeField]private List<Transform> childs;
    
    [Space]
    [Header("Events Callbacks")]
    public UnityEvent OnCompleteInCallback;
    public UnityEvent OnCompleteOutCallback;
    private void Awake()
    {
        childs = new List<Transform>();
        LoadChilds();
    }
    private void OnEnable()
    {
        if (childs.Count <= 0)
            LoadChilds();

        SetScaleToZero();
        InAnimation(gameObject,inDuration,inDelay);

        if (animateChilds)
            AnimateInChild();
    }
    private void OnDestroy() => childs.Clear();
    public void OnClose()
    {
        if (!gameObject.GetComponent<TweenAnimation>())
            return;
        
        OutAnimation(gameObject, outDuration, outDelay);
        if (animateChilds && (childs.Count > 0))
            AnimateOutChild();
    }
    private void SetScaleToZero()
    {
        transform.localScale = Vector2.zero;
        for (int i = 0; i < childs.Count; i++)
            childs[i].localScale = Vector2.zero;        
    }
    private void InAnimation(GameObject go, float duration, float delay)
    {
        if (customInCurve)
        {
            LeanTween.scale(go, Vector2.one, duration)
            .setDelay(delay)
            .setEase(inCurve)
            .setOnComplete(OnCompleteInAnimation);
        }
        else
        {
            LeanTween.scale(go, Vector2.one, duration)
            .setDelay(delay)
            .setEase(inEaseType)
            .setOnComplete(OnCompleteInAnimation);
        }    
    }
    private void OutAnimation(GameObject go,float duration,float delay)
    {
        if (customInCurve)
        {
            LeanTween.scale(go, Vector2.zero, duration)
            .setDelay(delay)
            .setEase(outCurve)
            .setOnComplete(OnCompleteOutAnimation);
        }
        else
        {
            LeanTween.scale(go, Vector2.zero, duration)
            .setDelay(delay)
            .setEase(outEaseType)
            .setOnComplete(OnCompleteOutAnimation);
        }
    }
    private void AnimateInChild()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            float randDuration = randomDuration ? 
            Random.Range(inMinDuration, inMaxDuration) : inDuration;
            InAnimation(childs[i].gameObject, randDuration, inDelay);
        }
    }
    private void AnimateOutChild()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            float randDuration = randomDuration ?
            Random.Range(outMinDuration, outMaxDuration) : inDuration;
            OutAnimation(childs[i].gameObject, randDuration, inDelay);
        }
    }
    private void LoadChilds()
    {
        Transform[] tsChilds = GetComponentsInChildren<Transform>();
        for (int i = 0; i < tsChilds.Length; i++)
        {
            if(tsChilds[i].CompareTag("animated"))
                childs.Add(tsChilds[i]);
        }     
    }
    private void OnCompleteInAnimation() => OnCompleteInCallback?.Invoke();
    private void OnCompleteOutAnimation() => OnCompleteOutCallback?.Invoke();
}
