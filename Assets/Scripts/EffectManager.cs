using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    [SerializeField] private ParticleSystem enemyClickEffectPrefab;
    private ParticleSystem enemyClickEffect;
    [SerializeField] private ParticleSystem enemyOversizeEffectPrefab;
    private ParticleSystem enemyOversizeEffect;

    private void Awake()
    {
        enemyClickEffect = Instantiate(enemyClickEffectPrefab);
        enemyOversizeEffect = Instantiate(enemyOversizeEffectPrefab);
        
    }

    private void PlayEffect(Vector3 pos, ParticleSystem effect)
    {
        effect.transform.position = pos;
        effect.Play();
        
    }

    public void PlayEnemyClickEffect(Vector3 pos)
    {
        PlayEffect(pos,enemyClickEffect);
    }
    public void PlayEnemyOversizeEffect(Vector3 pos)
    {
        PlayEffect(pos,enemyOversizeEffect);
    }
}