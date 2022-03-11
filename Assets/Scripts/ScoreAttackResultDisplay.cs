using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreAttackResultDisplay :MonoBehaviour {
    public UnityEvent<string> resultScore;
    [SerializeField] Button restart;
    [SerializeField] Button title;

    AsyncSubject<string> trigger;

    public void Init() {
        gameObject.SetActive(false);
    }

    public async UniTask<string> Run(int score) {
        restart.onClick.RemoveAllListeners();
        restart.onClick.AddListener(() => Restart());
        title.onClick.RemoveAllListeners();
        title.onClick.AddListener(() => Title());

        gameObject.SetActive(true);
        resultScore.Invoke(score.ToString());
        trigger = new AsyncSubject<string>();
        var result = await trigger;
        gameObject.SetActive(false);
        return result;
    }
    public void Restart() {
        trigger?.OnNext("restart");
        trigger?.OnCompleted();
        trigger = null;
    }
    public void Title() {
        trigger?.OnNext("title");
        trigger?.OnCompleted();
        trigger = null;
    }
}
