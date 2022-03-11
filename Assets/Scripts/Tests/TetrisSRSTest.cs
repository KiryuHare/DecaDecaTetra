using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TetrisSRSTest {
    void PutAndCheckAndPrint(Piece? piece, BlockSet blocks, string veryfied) {
        Assert.That(piece, Is.Not.Null);
        if (piece is Piece p) {
            var putBlocks = blocks.ValidateAndPutPiece(p);
            Assert.That(putBlocks, Is.Not.Null);
            if (putBlocks is BlockSet b) {
                CheckAndPrint(b, veryfied, blocks.columnCount, blocks.rowCount);
            }
        }
    }

    void CheckAndPrint(BlockSet blocks, string veryfied, int xN, int yN) {
        var veryfiedBlock = new BlockSet(veryfied, xN, yN);
        Debug.LogWarning(blocks.ToStr);
        Assert.That(BlockSystem.Equals(blocks, veryfiedBlock));
    }

    [Test]
    public void SRSTestS1() {
        var blocks = new BlockSet(
            @"  ..........
                ..........
                ..........
                O..OOOOOOO
                ..OOOOOOOO", 10, 5);
        Assert.That(blocks, Is.Not.Null);
        var piece = new Piece(0, 2, PieceType.S, PieceDirection.R);
        PutAndCheckAndPrint(piece, blocks,
            @"  ..........
                S.........
                SS........
                OS.OOOOOOO
                ..OOOOOOOO");
        var rot = piece.RotatePieceOn(blocks, RotateDirection.R);
        PutAndCheckAndPrint(rot, blocks,
            @"  ..........
                ..........
                ..........
                OSSOOOOOOO
                SSOOOOOOOO");
    }
    [Test]
    public void SRSTestS2() {
        var blocks = new BlockSet(
         @" ..........
            ..OO......
            .OOOO.....
            ..OOOOOOOO
            O.OOOOOOOO", 10, 5);
        var piece = new Piece(1, 3, PieceType.S, PieceDirection.U);
        PutAndCheckAndPrint(piece, blocks,
        @"  .SS.......
            SSOO......
            .OOOO.....
            ..OOOOOOOO
            O.OOOOOOOO"
        );
        var result = piece.RotatePieceOn(blocks, RotateDirection.L);
        PutAndCheckAndPrint(result, blocks,
        @"  ..........
            ..OO......
            SOOOO.....
            SSOOOOOOOO
            OSOOOOOOOO");
    }
    [Test]
    public void SRSTestT1() {
        var blocks = new BlockSet(
         @" O.........
            ..........
            .O........
            ..OOOOOOOO
            .OOOOOOOOO", 10, 5);
        var piece = new Piece(1, 3, PieceType.T, PieceDirection.U);
        PutAndCheckAndPrint(piece, blocks,
        @"  OT........
            TTT.......
            .O........
            ..OOOOOOOO
            .OOOOOOOOO"
        );
        var result = piece.RotatePieceOn(blocks, RotateDirection.R);
        PutAndCheckAndPrint(result, blocks,
        @"  O.........
            ..........
            TO........
            TTOOOOOOOO
            TOOOOOOOOO"
        );
    }
    [Test]
    public void SRSTestT2() {
        var blocks = new BlockSet(
        @"..........
            ..........
            ..........
            ..........
            ..........
            O..O......
            ...O....OO
            .OOO....OO
            ..OOOOOOOO
            .OOOOOOOOO", 10, 10);
        var piece = new Piece(2, 4, PieceType.T, PieceDirection.L);
        var result = piece.RotatePieceOn(blocks, RotateDirection.R);

        Assert.That(result, Is.Not.Null);
        if (blocks.ValidateAndPutPiece((Piece)result) is BlockSet newBlocks) {
            Assert.That(BlockSystem.Equals(newBlocks, new BlockSet(
            @"..........
            ..........
            ..........
            ..........
            ..........
            OT.O......
            TTTO....OO
            .OOO....OO
            ..OOOOOOOO
            .OOOOOOOOO", 10, 10)
            ));
        }
    }
}