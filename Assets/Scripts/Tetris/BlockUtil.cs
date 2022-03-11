
using System;
using UnityEngine;

public static class BlockUtil {
    public static Color GetColor(this BlockType block) => block switch {
        BlockType.I => new Color(0.3f, 0.7f, 1.0f),
        BlockType.O => new Color(1.0f, 1.0f, 0.0f),
        BlockType.L => new Color(1.0f, 0.5f, 0.0f),
        BlockType.J => new Color(0.0f, 0.0f, 1.0f),
        BlockType.S => new Color(0.0f, 1.0f, 0.0f),
        BlockType.Z => new Color(1.0f, 0.0f, 0.0f),
        BlockType.T => new Color(0.8f, 0.0f, 0.7f),
        _ => default
    };

    public static Block[][] Generate(string str, int xN, int yN) {
        var newBlock = new Block[yN][];
        var textLines = str.Trim().Split('\n');
        if (textLines.Length != yN) throw new Exception();
        for (int y = 0; y < yN; y++) {
            var line = textLines[y].Trim();
            newBlock[yN - y - 1] = new Block[xN];
            if (line.Length != xN) throw new Exception();
            for (int x = 0; x < xN; x++) {
                var chara = line[x];
                newBlock[yN - y - 1][x] = new Block {
                    type = BlockStateUtil.TypeFromChar(chara),
                    state = chara switch {
                        '.' => BlockState.Empty,
                        _ => BlockState.Exist
                    }
                };
            }
        }
        return newBlock;
    }



}

