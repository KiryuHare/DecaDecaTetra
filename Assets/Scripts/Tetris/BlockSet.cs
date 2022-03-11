using System;
using System.Collections.Generic;
using System.Linq;

public struct BlockSet {
    public Block[][] data { get; private set; }
    public int rowCount => data?.Length ?? 0;
    public int columnCount => data?[0]?.Length ?? 0;

    public Block GetBlock(int x, int y) => data[y][x];

    public bool BlockExists(int x, int y) {
        if (x >= 0 && x < columnCount) {
            if (y >= 0) {
                if (y < rowCount) {
                    return GetBlock(x, y).state == BlockState.Exist;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        } else {
            return true;
        }
    }

    public void SetBlock(int x, int y, Block b) => data[y][x] = b;

    public BlockSet(string str, int xN, int yN) {
        valid = true;
        data = BlockUtil.Generate(str, xN, yN);
    }

    public BlockSet(int xN, int yN) {
        valid = true;
        var newBlock = new Block[yN][];
        foreach (var i in Enumerable.Range(0, newBlock.Length)) {
            newBlock[i] = new Block[xN];
            for (int j = 0; j < xN; j++) {
                newBlock[i][j] = new Block();
            }
        }

        data = newBlock;
    }

    public BlockSet(BlockSet blocks) {
        valid = true;
        var newBlock = new Block[blocks.rowCount][];
        foreach (var y in Enumerable.Range(0, newBlock.Length)) {
            newBlock[y] = new Block[blocks.columnCount];
            foreach (var x in Enumerable.Range(0, blocks.columnCount)) {
                newBlock[y][x] = blocks.GetBlock(x, y);
            }
        }
        data = newBlock;
    }

    public BlockSet(Block[][] blocks) {
        data = blocks;
        valid = true;
    }


    public BlockSet(Piece piece) {
        valid = true;
        if (piece.type == PieceType.I) {
            data = piece.dir switch {
                PieceDirection.U => BlockUtil.Generate(
                @"....
                  IIII
                  ....
                  ....", 4, 4),
                PieceDirection.R => BlockUtil.Generate(
                @"..I.
                  ..I.
                  ..I.
                  ..I.", 4, 4),
                PieceDirection.D => BlockUtil.Generate(
                @"....
                  ....
                  IIII
                  ....", 4, 4),
                PieceDirection.L => BlockUtil.Generate(
                @".I..
                  .I..
                  .I..
                  .I..", 4, 4),
                _ => default
            };
        } else if (piece.type == PieceType.O) {
            data = BlockUtil.Generate(
                @"...
                  .OO
                  .OO", 3, 3);
        } else {
            var upSide = piece.type switch {
                PieceType.L => BlockUtil.Generate(
                @"..L
                  LLL
                  ...", 3, 3),
                PieceType.J => BlockUtil.Generate(
                @"J..
                  JJJ
                  ...", 3, 3),
                PieceType.Z => BlockUtil.Generate(
                @"ZZ.
                  .ZZ
                  ...", 3, 3),
                PieceType.S => BlockUtil.Generate(
                @".SS
                  SS.
                  ...", 3, 3),
                PieceType.T => BlockUtil.Generate(
                @".T.
                  TTT
                  ...", 3, 3),
                _ => default
            };
            data = piece.dir switch {
                PieceDirection.U => upSide,
                PieceDirection.R => new Block[][]{
                new Block[]{upSide[0][2],upSide[1][2],upSide[2][2]},
                new Block[]{upSide[0][1],upSide[1][1],upSide[2][1]},
                new Block[]{upSide[0][0],upSide[1][0],upSide[2][0]}
            },
                PieceDirection.D => new Block[][]{
                new Block[]{upSide[2][2],upSide[2][1],upSide[2][0] },
                new Block[]{upSide[1][2],upSide[1][1],upSide[1][0] },
                new Block[]{upSide[0][2],upSide[0][1],upSide[0][0] }
            },
                PieceDirection.L => new Block[][]{
                new Block[]{upSide[2][0],upSide[1][0],upSide[0][0] },
                new Block[]{upSide[2][1],upSide[1][1],upSide[0][1] },
                new Block[]{upSide[2][2],upSide[1][2],upSide[0][2]}
            },
                _ => default
            };
        }
    }

    public string ToStr {
        get {
            var texts = new List<string>();
            var yN = rowCount;
            var xN = columnCount;
            foreach (var y in Enumerable.Range(0, yN)) {
                foreach (var x in Enumerable.Range(0, xN)) {
                    texts.Add(
                        GetBlock(x, yN - y - 1).state switch {
                            BlockState.Empty => ".",
                            BlockState.Exist =>
                            GetBlock(x, yN - y - 1).type switch {
                                BlockType.I => "I",
                                BlockType.O => "O",
                                BlockType.L => "L",
                                BlockType.J => "J",
                                BlockType.S => "S",
                                BlockType.Z => "Z",
                                BlockType.T => "T",
                                _ => default
                            },
                            _ => "X"
                        }
                    );
                }
                texts.Add("\n");
            }
            return string.Concat(texts);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="proc">x,y,block</param>
    public void ForEachBlock(Action<int, int, Block> proc) {
        for (var y = 0; y < rowCount; y++)
            for (var x = 0; x < columnCount; x++)
                proc(x, y, data[y][x]);
    }

    public readonly bool valid;
}