using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;


public class GameBoard :MonoBehaviour {
    [SerializeField] BoardLine[] lines;
    void Init() {
        controller.shownBlock.Subscribe(blockSet => {
            blockSet.ForEachBlock((x, y, block) => {
                lines[y].BoardBlocks[x].SetBlock(block);
            });
        }).AddTo(this);
        controller.shownDrop.Subscribe(drop => {
            drop.ForEachBlock((x, y, block) => {
                lines[y].DropShadows[x].SetBlock(block);
            });
        });
        controller.shownNext.Subscribe(next => {
            next.ForEachBlock((x, y, block) => {
                lines[y].NextShadows[x].SetBlock(block);
            });
        });
    }

    GameBoardController controller;
    public void Init(GameBoardController controller_, Sprite blockSprite) {
        controller = controller_;
        foreach (var line in lines) {
            foreach (var block in line.BoardBlocks) {
                block.Init(blockSprite);
            }
        }
        Init();
    }
}
