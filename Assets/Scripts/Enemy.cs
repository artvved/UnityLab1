using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    private float size = 0f;
    private float startSize;

    private float secondsToGrow;

    void Start()
    {
        secondsToGrow = DifficultyManager.Instance.GrowthDifficultyCurve.Evaluate(
            PlayerMechanics.Instance.PlayerStats.PointsAmount);
        startSize = transform.localScale.x;
    }

    void FixedUpdate()
    {
        
            float sizeIncr = 1f / (secondsToGrow) * Time.deltaTime;
            size += sizeIncr;
            var lScale = transform.localScale;

            float scaleIncr = (1f - startSize) / (secondsToGrow) * Time.deltaTime;
            transform.localScale = new Vector3(lScale.x + scaleIncr,
                lScale.y + scaleIncr,
                lScale.z);
            
            if (size >= 1)
            {
                GetComponent<SpriteRenderer>().material.color=Color.red;
                GameStateManager.Instance.IsPlayerAlive = false;
            }
        
    }
}