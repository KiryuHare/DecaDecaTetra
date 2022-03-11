
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public enum BlockType {
    I, O, J, L, S, Z, T
}

[Serializable]
public struct Block {
    public BlockType type;
    public BlockState state;
}
