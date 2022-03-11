using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBoard :MonoBehaviour {
    [SerializeField] BoardLine[] lines;

    public void ShowBlock(BlockSet nextPiece){
        nextPiece.ForEachBlock((x, y, block) => {
            lines[y].BoardBlocks[x].SetBlock(block);
        });
    }

}
