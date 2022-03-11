using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class NextBoardController :MonoBehaviour {
    public ReactiveCollection<BlockSet> shownNextBlocks;

    void Init() {
        shownNextBlocks = new ReactiveCollection<BlockSet>();
        data.Nexts.ObserveCountChanged(true).Subscribe(count => {
            shownNextBlocks.Clear();
            for (var i = 0; i < count; i++) {
                shownNextBlocks.Add(new BlockSet(
                   data.Nexts[i] switch {
                       PieceType.I =>
                        @"....
                          IIII
                          ....
                          ....",
                       PieceType.O =>
                       @"....
                          .OO.
                          .OO.
                          ....",
                       PieceType.L =>
                         @"..L.
                            LLL.
                            ....
                            ....",
                       PieceType.J =>
                         @"J...
                            JJJ.
                            ....
                            ....",
                       PieceType.S =>
                         @".SS.
                            SS..
                            ....
                            ....",
                       PieceType.Z =>
                         @"ZZ..
                            .ZZ.
                            ....
                            ....",
                       PieceType.T =>
                         @".T..
                            TTT.
                            ....
                            ...."
                   }
                , 4, 4));
            }
        }).AddTo(this);
    }

    TetrisData data;
    public void Init(TetrisData data_) {
        data = data_;
        Init();
    }
}
