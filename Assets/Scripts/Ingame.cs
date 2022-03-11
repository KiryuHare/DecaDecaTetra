using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum IngameMode {
    endless = default, timeAttack, scoreAttack
}

public class Ingame :MonoBehaviour {
    [SerializeField] GameBoard gameBoard;
    [SerializeField] GameBoardController gameBoardController;
    [SerializeField] NextBoardController nextBoardController;
    [SerializeField] NextView nextView;
    [SerializeField] HoldBoardController holdBoardController;
    [SerializeField] HoldView holdView;
    [SerializeField] TetrisData tetrisData;
    [SerializeField] IngamePlayerInput input;
    [SerializeField] ScoreDisplay scoreDisplay;
    [SerializeField] GameRestarter restarter;
    [SerializeField] TimeAttackDisplay timeAttack;
    [SerializeField] TimeAttackResultDisplay timeAttackResult;
    [SerializeField] ScoreAttackDisplay scoreAttack;
    [SerializeField] ScoreAttackResultDisplay scoreAttackResult;
    [SerializeField] IngameMenu menu;
    [SerializeField] Sprite blockSprite;
    [SerializeField] AudioClip seMove;
    [SerializeField] AudioClip seSoftDrop;
    [SerializeField] AudioClip seHardDrop;
    [SerializeField] AudioClip seLineErace;
    [SerializeField] AudioClip seRotate;
    [SerializeField] AudioClip seTetris;
    [SerializeField] AudioClip seTSpin;
    [SerializeField] AudioClip seGameStart;
    [SerializeField] AudioClip seGameOver;
    [SerializeField] AudioClip seGameClear;
    [SerializeField] AudioClip sePerfect;
    [SerializeField] AudioClip seHold;
    [SerializeField] AudioClip seTSpinning;
    [SerializeField] Button menuButton;

    IngameTime time;
    float holdTime;
    int softDropInterval;
    int moveInterval;
    float timeLimit;

    //int lineCount;
    IngameMode mode;

    public void Init(IngameTime time_, float holdTime_, int softDropInterval_, int moveInterval_, float timeLimit_) {
        time = time_;
        holdTime = holdTime_;
        softDropInterval = softDropInterval_;
        moveInterval = moveInterval_;
        gameObject.SetActive(false);
        timeLimit = timeLimit_;

    }

    public async UniTask Run(IngameMode mode_) {
        mode = mode_;
        gameObject.SetActive(true);
        while (true) {
            menu.Init();
            menuButton.onClick.RemoveAllListeners();
            using CancellationTokenSource ts = new CancellationTokenSource();
            var cancelResult = "";
            // capture cancelResult
            menuButton.onClick.AddListener(async () => {
                time.Speed = 0;
                var result = await menu.Run();
                if (result == "title") {
                    cancelResult = "title";
                    ts.Cancel();
                } else if (result == "restart") {
                    cancelResult = "restart";
                    ts.Cancel();
                }
                time.Speed = 1;
            });

            time.Init();
            restarter.Init();
            input.Init(time, holdTime);
            tetrisData.Init(time);
            scoreDisplay.Init(tetrisData);
            gameBoardController.Init(tetrisData);
            gameBoard.Init(gameBoardController, blockSprite);
            nextBoardController.Init(tetrisData);
            nextView.Init(nextBoardController);
            holdBoardController.Init(tetrisData);
            holdView.Init(holdBoardController);
            restarter.Init();
            timeAttack.Init();
            timeAttackResult.Init();
            scoreAttack.Init();
            scoreAttackResult.Init();
            var audio = AudioManager.Get();
            audio.PlaySE(seGameStart);

            var startTime = time.Time;
            try {
                var result = await Loop(softDropInterval, moveInterval, ts.Token);

                if (result == "gameOver") {
                    audio.PlaySE(seGameOver);
                    var restart = await restarter.Run();
                    if (restart == "title") {
                        break;
                    }
                } else if (result == "clear") {
                    audio.PlaySE(seGameClear);
                    var clearTime = TimeSpan.FromSeconds(time.Time - startTime);
                    var restart = await timeAttackResult.Run(clearTime);
                    if (restart == "title") {
                        break;
                    }
                } else if (result == "finish") {
                    audio.PlaySE(seGameClear);
                    await scoreAttackResult.Run(tetrisData.Score.Value);
                    if (result == "title") {
                        break;
                    }
                }
            } catch (OperationCanceledException c) {
                if (cancelResult == "title") {
                    break;
                }
            }
            await UniTask.WaitForEndOfFrame();
            await GoogleAdsController.Get().GameOver();
        }
        await GoogleAdsController.Get().GameOver();
        gameObject.SetActive(false);
    }

    public async UniTask<string> Loop(int softDropInterval, int moveInterval, CancellationToken t) {
        var audio = AudioManager.Get();

        var softDropSpan = TimeSpan.FromMilliseconds(softDropInterval);
        var moveSpan = TimeSpan.FromMilliseconds(moveInterval);

        var eracedLineCount = new IntReactiveProperty();
        var elapsedTime = new FloatReactiveProperty();
        var remainTime = new FloatReactiveProperty();
        var startTime = time.Time;
        //var holdDead = false;
        var timeUp = false;

        if (mode == IngameMode.timeAttack) {
            timeAttack.StartDisplay(elapsedTime, eracedLineCount);
        } else if (mode == IngameMode.scoreAttack) {
            scoreAttack.StartDisplay(remainTime);
        }

        //capture timeUp
        using var x =
            Observable.EveryUpdate().Subscribe((_ => {
                elapsedTime.Value = time.Time - startTime;
                remainTime.Value = timeLimit - elapsedTime.Value;
                if (remainTime.Value < 0) remainTime.Value = 0;
                if (mode == IngameMode.scoreAttack && elapsedTime.Value >= timeLimit) {
                    timeUp = true;
                }
            }));

        using var s0 = tetrisData.LineEraceEvent.Subscribe(result => {
            if (result.IsLineEraced) {
                if (result.IsTetris) {
                    audio.PlaySE(seTetris);
                } else if (result.IsTSpin) {
                    audio.PlaySE(seTSpin);
                } else {
                    audio.PlaySE(seLineErace);
                }
            }
        });

        using var movese = tetrisData.MoveEvent.Subscribe(_ => {
            audio.PlaySE(seMove);
        });

        using var rotatese = tetrisData.RotationEvent.Subscribe(_ => {
            audio.PlaySE(seRotate);
        });
        using var sdse = tetrisData.SoftDropEvent.Subscribe(_ => {
            audio.PlaySE(seSoftDrop);
        });
        using var hdse = tetrisData.HardDropEvent.Subscribe(_ => {
            audio.PlaySE(seHardDrop);
        });
        using var holdse = tetrisData.Hold.Subscribe(_ => {
            audio.PlaySE(seHold);
        });
        using var perfse = tetrisData.PerfectEvent.Subscribe(_ => {
            audio.PlaySE(sePerfect);
        });
        using var spinse = tetrisData.TSpinEvent.Subscribe(_ => {
            audio.PlaySE(seTSpinning);
        });

        while (true) {
            var nextOK = tetrisData.EntryNextPiece();
            if (!nextOK) {
                return "gameOver";
            }
            {
                var settle = false;
                using var a0 = input.right.Where(f => f).Subscribe(_ => {
                    tetrisData.MoveRight();
                });
                using var a1 = input.left.Where(f => f).Subscribe(_ => {
                    tetrisData.MoveLeft();
                });
                using var a2 = Observable.EveryUpdate().Where(_ => input.down.Value).ThrottleFirst(softDropSpan).Subscribe(_ => {
                    tetrisData.SoftDrop();
                });
                using var a3 = input.rotL.Where(f => f).Subscribe(_ => {
                    tetrisData.RotatePiece(RotateDirection.L);
                });
                using var a4 = input.rotR.Where(f => f).Subscribe(_ => {
                    tetrisData.RotatePiece(RotateDirection.R);
                });
                using var a5 = Observable.EveryUpdate().Where(_ => input.rightHold.Value).ThrottleFirst(moveSpan).Subscribe(_ => {
                    tetrisData.MoveRight();
                });
                using var a6 = Observable.EveryUpdate().Where(_ => input.leftHold.Value).ThrottleFirst(moveSpan).Subscribe(_ => {
                    tetrisData.MoveLeft();
                });

                using var a7 = input.hold.Where(f => f).Subscribe(_ => {
                    tetrisData.HoldPiece();
                });

                //capture settle
                input.up.Where(f => f).Subscribe(_ => {
                    settle = true;
                });

                await UniTask.WaitWhile(() => !settle && !timeUp, cancellationToken: t);
            }

            if (timeUp) return "finish";
            var eraced = await tetrisData.HardDrop(t);
            if (mode == IngameMode.timeAttack) {
                eracedLineCount.Value += eraced.eraceLineCount;
                if (eracedLineCount.Value >= 20) {
                    return "clear";
                }
            }
        }
    }
}
