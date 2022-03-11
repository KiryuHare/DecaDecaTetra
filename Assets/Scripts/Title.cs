using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Title :MonoBehaviour {
    AsyncSubject<string> selection;
    public void Init() {
        gameObject.SetActive(false);
        Endless.onClick.AddListener(() => StartGame(IngameMode.endless));
        TimeAttack.onClick.AddListener(() => StartGame(IngameMode.timeAttack));
        ScoreAttack.onClick.AddListener(() => StartGame(IngameMode.scoreAttack));
        Rights.onClick.AddListener(() => Right());
        right.Init();
    }

    public async UniTask<IngameMode> Run() {
        gameObject.SetActive(true);

        string result = null;
        while (true) {
            selection = new AsyncSubject<string>();
            await UniTask.WaitForEndOfFrame();
            result = await selection;
            if (result != "Right") break;
            await right.Run();
        }

        gameObject.SetActive(false);
        return result switch {
            "endless" => IngameMode.endless,
            "scoreAttack" => IngameMode.scoreAttack,
            "timeAttack" => IngameMode.timeAttack,
        };
    }

    void StartGame(IngameMode mode) {
        selection?.OnNext(mode switch {
            IngameMode.endless => "endless",
            IngameMode.scoreAttack => "scoreAttack",
            IngameMode.timeAttack => "timeAttack"
        });
        selection?.OnCompleted();
        selection = null;
    }

    void Right() {
        selection?.OnNext("Right");
        selection?.OnCompleted();
        selection = null;
    }

    [SerializeField] Button Endless;
    [SerializeField] Button TimeAttack;
    [SerializeField] Button ScoreAttack;
    [SerializeField] Button Rights;
    [SerializeField] Right right;
}
