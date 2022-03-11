using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenu :MonoBehaviour {
    public void Init() {
        gameObject.SetActive(false);
    }

    public async UniTask<string> Run() {
        gameObject.SetActive(true);
        restart.onClick.RemoveAllListeners();
        title.onClick.RemoveAllListeners();
        close.onClick.RemoveAllListeners();
        trigger = new AsyncSubject<string>();
        restart.onClick.AddListener(() => {
            trigger.OnNext("restart");
            trigger.OnCompleted();
        });
        title.onClick.AddListener(() => {
            trigger.OnNext("title");
            trigger.OnCompleted();
        });
        close.onClick.AddListener(() => {
            trigger.OnNext("close");
            trigger.OnCompleted();
        });

        var result = await trigger;
        gameObject.SetActive(false);
        return result;
    }

    AsyncSubject<string> trigger;

    [SerializeField] Button restart;
    [SerializeField] Button title;
    [SerializeField] Button close;

}
