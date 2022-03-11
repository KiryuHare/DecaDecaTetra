using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Threading;

public class TetrisPlayer {
    [UnityTest]
    public IEnumerator TetrisPlayerSimplePasses() => UniTask.ToCoroutine(async () => {
        await SceneManager.LoadSceneAsync("IngameScene");
        var main = GameObject.Find("Main").GetComponent<Main>();
        CancellationTokenSource ts = new CancellationTokenSource();
        var tetrisData = typeof(Main).GetField("tetrisData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(main) as TetrisData;
        tetrisData.SetTetrisField(
            @"..........
            ..........
            ..........
            ..........
            ..........
            O.........
            ........OO
            .OOOOOOOOO
            ..OOOOOOOO
            .OOOOOOOOO");
        var oPiece = new Piece(3, 7, PieceType.O, PieceDirection.L);
        tetrisData.SetPiece(oPiece);
        await UniTask.Delay(300);
        tetrisData.MoveLeft();
        await UniTask.Delay(300);
        tetrisData.MoveLeft();
        await UniTask.Delay(300);
        tetrisData.MoveLeft();
        await UniTask.Delay(300);
        tetrisData.MoveLeft();
        await UniTask.Delay(300);
        tetrisData.MoveLeft();
        await UniTask.Delay(300);
        tetrisData.MoveLeft();
        await UniTask.Delay(300);
        tetrisData.MoveRight();
        await UniTask.Delay(300);
        tetrisData.MoveRight();
        await UniTask.Delay(300);
        tetrisData.MoveRight();
        await UniTask.Delay(300);
        tetrisData.SoftDrop();
        await UniTask.Delay(300);
        await tetrisData.HardDrop(ts.Token);
        await UniTask.Delay(300);
        var piece = new Piece(2, 5, PieceType.T, PieceDirection.L);
        tetrisData.SetPiece(piece);
        await UniTask.Delay(300);
        tetrisData.SoftDrop();
        await UniTask.Delay(300);
        tetrisData.RotatePiece(RotateDirection.R);
        await UniTask.Delay(300);
        tetrisData.RotatePiece(RotateDirection.R);
        await UniTask.Delay(300);
        tetrisData.RotatePiece(RotateDirection.L);
        await UniTask.Delay(300);
        tetrisData.RotatePiece(RotateDirection.R);
        await UniTask.Delay(300);
        await tetrisData.HardDrop(ts.Token);
        await UniTask.Delay(300);
    });

    [UnityTest]
    public IEnumerator TetrisPlayerSimplePasses2() => UniTask.ToCoroutine(async () => {
        await SceneManager.LoadSceneAsync("IngameScene");
        var main = GameObject.Find("Main").GetComponent<Main>();
        CancellationTokenSource ts = new CancellationTokenSource();
        await UniTask.WaitForEndOfFrame();
        var tetrisData = typeof(Main).GetField("tetrisData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(main) as TetrisData;
        tetrisData.SetTetrisField(
            @"..........
            ..........
            ..........
            ..........
            ..........
            ..........
            ..........
            ..........
            OOOOO..OOO
            OOOO..OOOO");
        var piece = new Piece(5, 3, PieceType.S, PieceDirection.R);
        tetrisData.SetPiece(piece);
        await UniTask.Delay(1000);
        tetrisData.MoveLeft();
        await UniTask.Delay(1000);
        tetrisData.SoftDrop();
        await UniTask.Delay(1000);
        tetrisData.RotatePiece(RotateDirection.R);
        await UniTask.Delay(1000);
        await tetrisData.HardDrop(ts.Token);
        await UniTask.Delay(1000);
    });
}
