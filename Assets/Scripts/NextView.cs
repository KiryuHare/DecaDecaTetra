using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NextView :MonoBehaviour {
    [SerializeField] NextBoard[] nextBoards;
    void Init() {
        controller.shownNextBlocks.ObserveCountChanged().Subscribe(c => {
            for (var i = 0; i < nextBoards.Length; i++) {
                if (controller.shownNextBlocks.Count > i + 1) {
                    nextBoards[i].ShowBlock(controller.shownNextBlocks[i]);
                } else {
                    nextBoards[i].ShowBlock(new BlockSet(4, 4));
                }
            }
        }).AddTo(this);
    }

    NextBoardController controller;
    public void Init(NextBoardController controller_) {
        controller = controller_;
        Init();
    }
}
