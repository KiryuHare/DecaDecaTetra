using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using System;

public class TimeAttackDisplay :MonoBehaviour {
    public TextMeshProUGUI timeMesh;
    public TextMeshProUGUI lineMesh;

    public void Init() {
        gameObject.SetActive(false);
    }

    public async void StartDisplay(FloatReactiveProperty time, IntReactiveProperty eracedLineCount) {
        gameObject.SetActive(true);
        time.Subscribe(t => {
            timeMesh.text = TimeSpan.FromSeconds(time.Value).ToString("g");
        });
        eracedLineCount.Subscribe(n => {
            lineMesh.text = n + " / 20 Lines";
        });
        await time.DoOnCompleted(() => { });
        gameObject.SetActive(false);
    }
}
