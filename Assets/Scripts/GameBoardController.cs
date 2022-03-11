using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using UniRx.Diagnostics;

public class GameBoardController :MonoBehaviour {

    public ReactiveProperty<BlockSet> shownBlock;
    public ReactiveProperty<BlockSet> shownDrop;
    public ReactiveProperty<BlockSet> shownNext;
    public IntReactiveProperty score;

    void Init() {
        shownBlock = new ReactiveProperty<BlockSet>();
        data.NowPiece.Subscribe(p => {
            shownBlock.Value =
                BlockSystem.ValidateAndPutPiece(data.NowBlock.Value, p) ?? shownBlock.Value;
        }).AddTo(this);
        data.NowBlock.Subscribe(b => {
            shownBlock.Value = data.NowBlock.Value;
        }).AddTo(this);
        data.NowPiece.Subscribe(p => {
            var dropShadow = p.HardFallOn(data.NowBlock.Value);
            shownDrop.Value = empty.PutPiece(dropShadow ?? p);
        });
        data.Nexts.ObserveCountChanged().Subscribe(i => {
            var next = data.Nexts[0];
            shownNext.Value = empty.PutPiece(TetrisData.DefaultPositionPiece(next));
        });
        data.Score.Subscribe(s => {
            score.Value = s;
        });
    }

    TetrisData data;
    public void Init(TetrisData data_) {
        data = data_;
        Init();
    }

    static readonly BlockSet empty = new BlockSet(10, 10);
}
