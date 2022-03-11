using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ScoreDisplay :MonoBehaviour {
    [SerializeField] TextMeshProUGUI textMesh;

    void Init() {
        tetrisData.Score.Subscribe(s => {
            textMesh.text = string.Format("{0:00000000}", s);
        });
    }

    TetrisData tetrisData;

    public void Init(TetrisData tetrisData_) {
        tetrisData = tetrisData_;
        Init();
    }
}
