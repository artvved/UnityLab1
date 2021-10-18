using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    private DifficultyManager()
    {
    }

    public static DifficultyManager Instance;
    [SerializeField]private AnimationCurve spawnDifficultyCurve;
    [SerializeField]private AnimationCurve growthDifficultyCurve;

    public AnimationCurve SpawnDifficultyCurve
    {
        get => spawnDifficultyCurve;
        set => spawnDifficultyCurve = value;
    }

    public AnimationCurve GrowthDifficultyCurve
    {
        get => growthDifficultyCurve;
        set => growthDifficultyCurve = value;
    }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
