using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;

public class TetrisData :MonoBehaviour {
    public IObservable<HardDropResult> LineEraceEvent => lineEraceEvent;
    Subject<HardDropResult> lineEraceEvent;
    public IObservable<Unit> MoveEvent => moveEvent;
    Subject<Unit> moveEvent;
    public IObservable<Unit> RotationEvent => rotationEvent;
    Subject<Unit> rotationEvent;
    public IObservable<Unit> SoftDropEvent => softDropEvent;
    Subject<Unit> softDropEvent;
    public IObservable<Unit> HardDropEvent => hardDropEvent;
    Subject<Unit> hardDropEvent;
    public IObservable<Unit> PerfectEvent => perfectEvent;
    Subject<Unit> perfectEvent;
    public IObservable<Unit> TSpinEvent => tSpinEvent;
    Subject<Unit> tSpinEvent;
    public ReactiveProperty<BlockSet> NowBlock { get; private set; }
    public ReactiveProperty<Piece> NowPiece { get; private set; }

    public ReactiveCollection<PieceType> Nexts { get; private set; }

    public ReactiveProperty<PieceType> Hold { get; private set; }

    public BoolReactiveProperty Holdable { get; private set; }
    public IntReactiveProperty Score { get; private set; }

    public BoolReactiveProperty tSpin { get; private set; }

    public bool EntryNextPiece() {
        if (Nexts.Count < 7) {
            var randomType = new PieceType[7] {
                    PieceType.I, PieceType.O, PieceType.T,
                     PieceType.S, PieceType.Z, PieceType.L, PieceType.J
                    }.OrderBy(_ => Guid.NewGuid());
            foreach (var t in randomType) {
                Nexts.Add(t);
            }
        }
        if (SetPiece(DefaultPositionPiece(Nexts[0]))) {
            Nexts.RemoveAt(0);
            tSpin.Value = false;
            return true;
        } else {
            return false;
        }
    }

    public bool SetPiece(Piece piece) {
        if (NowBlock.Value.IsCrashing(piece)) {
            return false;
        }
        NowPiece.Value = piece;
        return true;
    }

    public bool RotatePiece(RotateDirection dir) {
        if (NowPiece.Value.RotatePieceOn(NowBlock.Value, dir) is Piece p) {
            if (NowPiece.Value.type == PieceType.T) {
                var x = p.x;
                var y = p.y;
                var count = 0;
                if (NowBlock.Value.BlockExists(x - 1, y - 1)) count++;
                if (NowBlock.Value.BlockExists(x + 1, y - 1)) count++;
                if (NowBlock.Value.BlockExists(x - 1, y + 1)) count++;
                if (NowBlock.Value.BlockExists(x + 1, y + 1)) count++;
                if (count >= 3) {
                    tSpinEvent.OnNext(Unit.Default);
                    tSpin.Value = true;
                } else tSpin.Value = false;
            } else {
                tSpin.Value = false;
            }
            NowPiece.Value = p;
            rotationEvent.OnNext(Unit.Default);
            return true;
        } else {
            return false;
        }

    }

    void SettlePiece() {
        NowBlock.Value = NowBlock.Value.ValidateAndPutPiece(NowPiece.Value) ?? NowBlock.Value;
    }


    public void SetTetrisField(string str) {
        NowBlock.Value = new BlockSet(str, 10, 10);
    }

    public bool SoftDrop() {
        if (NowPiece.Value.FallPieceOn(NowBlock.Value) is Piece p) {
            NowPiece.Value = p;
            softDropEvent.OnNext(default);
            tSpin.Value = false;
            return true;
        } else {
            return false;
        }
    }

    public struct HardDropResult {
        public int eraceLineCount;
        public bool IsLineEraced => eraceLineCount > 0;
        public bool IsTetris;
        public bool IsTSpin;
        public bool IsPerfect;
    }

    public async UniTask<HardDropResult> HardDrop(CancellationToken t) {
        NowPiece.Value = NowPiece.Value.HardFallOn(NowBlock.Value) ??
            NowPiece.Value;
        hardDropEvent.OnNext(default);
        SettlePiece();
        await time.Wait(0.15f).ToUniTask(cancellationToken: t);
        var eraced = EraceLine();
        var perfect = CheckPerfect();
        var result = new HardDropResult {
            eraceLineCount = eraced,
            IsTetris = eraced >= 4,
            IsTSpin = tSpin.Value,
            IsPerfect = perfect
        };
        lineEraceEvent.OnNext(result);
        if (eraced > 0) {
            await time.Wait(0.4f).ToUniTask(cancellationToken: t);
            NowBlock.Value = NowBlock.Value.OrganizeLine();
        }
        Score.Value += GetScore(result);
        NowPiece.Value = default;
        Holdable.Value = true;
        return result;
    }

    int GetScore(HardDropResult result) => result.eraceLineCount switch {
        1 => 100,
        2 => 400,
        3 => 1000,
        4 => 4000,
        _ => 0
    } * (result.IsTSpin ? 8 : 1) + 10;

    int EraceLine() {
        var (newBlocks, erased) = NowBlock.Value.EraseLine();
        NowBlock.Value = newBlocks;
        return erased;
    }

    public bool MoveRight() {
        if (NowPiece.Value.MoveRightOn(NowBlock.Value) is Piece p) {
            NowPiece.Value = p;
            moveEvent.OnNext(default);
            tSpin.Value = false;
            return true;
        } else {
            return false;
        }
    }
    public bool MoveLeft() {
        if (NowPiece.Value.MoveLeftOn(NowBlock.Value) is Piece p) {
            NowPiece.Value = p;
            moveEvent.OnNext(default);
            tSpin.Value = false;
            return true;
        } else {
            return false;
        }
    }

    bool CheckPerfect() {
        if (NowBlock.Value.IsPerfect()) {
            Debug.LogWarning("Perfect!!!");
            Score.Value += 10000;
            perfectEvent.OnNext(Unit.Default);
            return true;
        }
        return false;
    }

    public enum HoldResult {
        OK, Already, CANT
    }

    public HoldResult HoldPiece() {
        if (Holdable.Value) {
            if (Hold.Value == PieceType.None) {
                var tmp = NowPiece.Value.type;
                if (EntryNextPiece()) {
                    Hold.Value = tmp;
                    Holdable.Value = false;
                    return HoldResult.OK;
                }
                return HoldResult.CANT;
            } else {
                var tmp = NowPiece.Value.type;
                if (SetPiece(DefaultPositionPiece(Hold.Value))) {
                    Hold.Value = tmp;
                    Holdable.Value = false;
                    return HoldResult.OK;
                }
                return HoldResult.CANT;
            }
        } else {
            return HoldResult.Already;
        }
    }

    public static Piece DefaultPositionPiece(PieceType type)
    => new Piece(4, 8, type);


    void Init() {
        NowPiece = new ReactiveProperty<Piece>(default);
        NowBlock = new ReactiveProperty<BlockSet>(
            new BlockSet(10, 10)
        );
        Nexts = new ReactiveCollection<PieceType>();
        Hold = new ReactiveProperty<PieceType>(PieceType.None);
        Holdable = new BoolReactiveProperty(true);
        Score = new IntReactiveProperty(0);
        lineEraceEvent = new Subject<HardDropResult>();
        softDropEvent = new Subject<Unit>();
        hardDropEvent = new Subject<Unit>();
        moveEvent = new Subject<Unit>();
        rotationEvent = new Subject<Unit>();
        tSpin = new BoolReactiveProperty(false);
        perfectEvent = new Subject<Unit>();
        tSpinEvent = new Subject<Unit>();
    }

    public void Init(IngameTime time_) {
        time = time_;
        Init();
    }

    IngameTime time;

    [ContextMenu("OutState")]
    void OutState() {

    }
}
