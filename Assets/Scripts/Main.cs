using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;


public class Main :MonoBehaviour {
    //[SerializeField] GUIDebugLog logger;

    [SerializeField] float holdTime;
    [SerializeField] int moveInterval;
    [SerializeField] int softDropInterval;
    [SerializeField] float timeLimit;
    //[SerializeField] IngameMode mode;

    [SerializeField] Ingame ingame;
    [SerializeField] Title title;
    [SerializeField] IngameTime ingameTime;


    private async void Awake() {
        title.Init();
        ingame.Init(ingameTime, holdTime, softDropInterval, moveInterval,
            timeLimit);
        while (true) {
            var mode = await title.Run();
            await ingame.Run(mode);
            await UniTask.WaitForEndOfFrame();
        }
    }
}
