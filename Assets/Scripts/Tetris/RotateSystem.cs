
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class RotationSystem {
    /// <returns>回転不可能な場合、null</returns>
    public static Piece? RotatePieceOn(this Piece piece,in BlockSet blocks, RotateDirection rot) {

        var srsCoords = GetSrsCoordsList(piece, rot);
        var rotatedPiece = piece.GetRotated(rot);

        foreach (var (x, y) in srsCoords) {
            var srsMovedPiece = rotatedPiece.GetMoved(y, x);
            if (!blocks.IsCrashing(srsMovedPiece)) {
                return srsMovedPiece;
            }
        }
        return null;
    }

    static List<(int x, int y)> GetSrsCoordsList(Piece piece, RotateDirection rot) {
        List<(int x, int y)> srsCoords = new List<(int x, int y)>();
        if (piece.type == PieceType.I) {
            srsCoords.Add((0, 0));
            var dir = piece.dir.Rotated(rot);
            if (piece.dir == PieceDirection.U && dir == PieceDirection.R ||
            piece.dir == PieceDirection.L && dir == PieceDirection.D) {
                srsCoords.Add((-2, 0));
                srsCoords.Add((1, 0));
                srsCoords.Add((-2, -1));
                srsCoords.Add((1, 2));
            } else if (piece.dir == PieceDirection.R && dir == PieceDirection.U ||
            piece.dir == PieceDirection.D && dir == PieceDirection.L) {
                srsCoords.Add((2, 0));
                srsCoords.Add((-1, 0));
                srsCoords.Add((2, 1));
                srsCoords.Add((-1, -2));
            } else if (piece.dir == PieceDirection.R && dir == PieceDirection.D ||
            piece.dir == PieceDirection.U && dir == PieceDirection.L) {
                srsCoords.Add((-1, 0));
                srsCoords.Add((2, 0));
                srsCoords.Add((-1, 2));
                srsCoords.Add((2, -1));
            } else if (piece.dir == PieceDirection.D && dir == PieceDirection.R ||
            piece.dir == PieceDirection.L && dir == PieceDirection.U) {
                srsCoords.Add((1, 0));
                srsCoords.Add((-2, 0));
                srsCoords.Add((1, -2));
                srsCoords.Add((-2, 1));
            }

        } else if (piece.type == PieceType.O) {
            srsCoords.Add((0, 0));
        } else {
            srsCoords.Add((0, 0));
            var dir = piece.dir.Rotated(rot);
            if (piece.dir == PieceDirection.U && dir == PieceDirection.R ||
                piece.dir == PieceDirection.D && dir == PieceDirection.R) {
                srsCoords.Add((-1, 0));
                srsCoords.Add((-1, 1));
                srsCoords.Add((0, -2));
                srsCoords.Add((-1, -2));
            } else if (piece.dir == PieceDirection.R && dir == PieceDirection.U ||
             piece.dir == PieceDirection.R && dir == PieceDirection.D) {
                srsCoords.Add((1, 0));
                srsCoords.Add((1, -1));
                srsCoords.Add((0, 2));
                srsCoords.Add((1, 2));
            } else if (piece.dir == PieceDirection.D && dir == PieceDirection.L ||
            piece.dir == PieceDirection.U && dir == PieceDirection.L) {
                srsCoords.Add((1, 0));
                srsCoords.Add((1, 1));
                srsCoords.Add((0, -2));
                srsCoords.Add((1, -2));
            } else if (piece.dir == PieceDirection.L && dir == PieceDirection.U ||
            piece.dir == PieceDirection.L && dir == PieceDirection.D) {
                srsCoords.Add((-1, 0));
                srsCoords.Add((-1, -1));
                srsCoords.Add((0, 2));
                srsCoords.Add((-1, 2));
            }
        }
        return srsCoords;
    }
}
