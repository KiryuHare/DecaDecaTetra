using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class HoldBoardController :MonoBehaviour {
    public ReactiveProperty<BlockSet> holdBlock;

    void Init() {
        holdBlock = new ReactiveProperty<BlockSet>();
        data.Hold.Subscribe(hold => {
            holdBlock.Value = new BlockSet(
                data.Hold.Value switch {
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
                            ....",
                    _ =>
                      @"....
                            ....
                            ....
                            ...."

                }
                , 4, 4);
        }).AddTo(this);
    }

    TetrisData data;
    public void Init(TetrisData data_) {
        data = data_;
        Init();
    }
}
