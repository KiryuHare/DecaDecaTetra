using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class GameRestarter :MonoBehaviour {
    [SerializeField] GameObject[] gameOvers;
    [SerializeField] Button restart;
    [SerializeField] Button title;

    AsyncSubject<string> trigger;

    public void Init() {
        foreach (var go in gameOvers) go.SetActive(false);
    }

    public async UniTask<string> Run() {
        restart.onClick.RemoveAllListeners();
        restart.onClick.AddListener(() => Restart());
        title.onClick.RemoveAllListeners();
        title.onClick.AddListener(() => Title());

        foreach (var go in gameOvers) go.SetActive(true);
        trigger = new AsyncSubject<string>();
        var result = await trigger;
        foreach (var go in gameOvers) go.SetActive(false);
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
