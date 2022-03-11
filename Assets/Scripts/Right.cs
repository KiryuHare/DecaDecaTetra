using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Right :MonoBehaviour {

    public void Init() {
        gameObject.SetActive(false);
    }
    public async UniTask Run() {
        gameObject.SetActive(true);
        bool closed = false;
        var closeButton = new UnityAction(() => {
            closed = true;
        });
        close.onClick.AddListener(closeButton);
        await UniTask.WaitWhile(() => !closed);
        close.onClick.RemoveListener(closeButton);
        gameObject.SetActive(false);
        return;
    }
    [SerializeField] Button close;
}
