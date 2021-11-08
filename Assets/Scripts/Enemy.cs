using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animation animation;
    private float size = 0f;
    private float startSize;

    private float secondsToGrow;

    public float SecondsToGrow
    {
        get => secondsToGrow;
        set => secondsToGrow = value;
    }


    public event Action OversizeEvent;


    public void OnOversize()
    {
        OversizeEvent?.Invoke();
    }

    void Start()
    {
        startSize = transform.localScale.x;
    }

    void FixedUpdate()
    {
        if (size > 1)
        {
            OnOversize();
            return;
        }

        float sizeIncr = 1f / (secondsToGrow) * Time.deltaTime;
        size += sizeIncr;
        var lScale = transform.localScale;

        float scaleIncr = (1f - startSize) / (secondsToGrow) * Time.deltaTime;
        transform.localScale = new Vector3(lScale.x + scaleIncr,
            lScale.y + scaleIncr,
            lScale.z);
    }


    public void PlayDeathAnimationAndDie()
    {
        animation.Play();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}