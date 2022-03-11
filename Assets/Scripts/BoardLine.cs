using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using UniRx.Diagnostics;

public class BoardLine :MonoBehaviour {
    [SerializeField] BoardBlock[] boardBlocks;
    [SerializeField] BlockShadow[] dropShadows;
    [SerializeField] BlockShadow[] nextShadows;
    public BoardBlock[] BoardBlocks => boardBlocks;
    public BlockShadow[] DropShadows => dropShadows;
    public BlockShadow[] NextShadows => nextShadows;
}
