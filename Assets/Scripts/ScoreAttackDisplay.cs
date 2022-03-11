using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using System;

public class ScoreAttackDisplay :MonoBehaviour {
    public TextMeshProUGUI timeMesh;

    public void Init() {
        gameObject.SetActive(false);
    }

    public async void StartDisplay(FloatReactiveProperty time) {
        gameObject.SetActive(true);
        time.Subscribe(t => {
            timeMesh.text = TimeSpan.FromSeconds(time.Value).ToString("g");
        });
        await time.DoOnCompleted(() => { });
        gameObject.SetActive(false);
    }
}
