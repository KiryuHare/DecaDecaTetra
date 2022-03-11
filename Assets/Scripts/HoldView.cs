using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HoldView :MonoBehaviour {
    [SerializeField] NextBoard holdBoard;
    void Init() {
        controller.holdBlock.Subscribe(c => {
            holdBoard.ShowBlock(c);
        }).AddTo(this);
    }

    HoldBoardController controller;
    public void Init(HoldBoardController controller_) {
        controller = controller_;
        Init();
    }
}
