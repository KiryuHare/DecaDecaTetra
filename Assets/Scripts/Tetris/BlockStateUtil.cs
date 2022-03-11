using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BlockStateUtil {
    public static BlockType TypeFromChar(char c) => c switch {
        'I' => BlockType.I,
        'O' => BlockType.O,
        'L' => BlockType.L,
        'J' => BlockType.J,
        'S' => BlockType.S,
        'Z' => BlockType.Z,
        'T' => BlockType.T,
        _ => default
    };
}
