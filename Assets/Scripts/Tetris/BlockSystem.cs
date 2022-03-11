
using System;
using System.Linq;

public static class BlockSystem {
    public static bool Equals(this BlockSet l, BlockSet r) {
        if (l.rowCount != r.rowCount) return false;
        if (l.columnCount != r.columnCount) return false;
        foreach (var y in Enumerable.Range(0, l.rowCount)) {
            foreach (var x in Enumerable.Range(0, l.columnCount)) {
                if (l.GetBlock(x, y).state == BlockState.Exist) {
                    if (r.GetBlock(x, y).state != BlockState.Exist) return false;
                    if (r.GetBlock(x, y).type != l.GetBlock(x, y).type) return false;
                } else if (l.GetBlock(x, y).state == BlockState.Empty) {
                    if (r.GetBlock(x, y).state != BlockState.Empty) return false;
                } else {
                    throw new Exception();
                }
            }
        }
        return true;
    }

    public static bool IsCrashing(this BlockSet blocks, Piece piece) {
        var pieceBlock = new BlockSet(piece);
        for (int y = 0; y < pieceBlock.rowCount; y++) {
            for (int x = 0; x < pieceBlock.columnCount; x++) {
                var boardY = y + piece.y - piece.type.Center().y;
                var boardX = x + piece.x - piece.type.Center().x;
                if (pieceBlock.GetBlock(x, y).state == BlockState.Exist) {
                    if (blocks.BlockExists(boardX, boardY)) return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    ///置けるかどうか関係なくピースを配置して返す 
    /// </summary>
    public static BlockSet PutPiece(this BlockSet blocks, Piece piece) {
        var newBlocks = new BlockSet(blocks);
        var pieceBlock = new BlockSet(piece);
        foreach (var y in Enumerable.Range(0, pieceBlock.rowCount)) {
            foreach (var x in Enumerable.Range(0, pieceBlock.columnCount)) {
                if (pieceBlock.GetBlock(x, y).state == BlockState.Exist) {
                    var bx = x + piece.x - piece.type.Center().x;
                    var by = y + piece.y - piece.type.Center().y;
                    if (by < newBlocks.rowCount) {
                        newBlocks.SetBlock(
                        bx, by, pieceBlock.GetBlock(x, y));
                    }
                }
            }
        }
        return newBlocks;
    }

    public static BlockSet? ValidateAndPutPiece(this BlockSet blocks, Piece piece) {
        if (blocks.IsCrashing(piece)) return null;
        return PutPiece(blocks, piece);
    }
}
