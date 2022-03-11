using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Events;
using UniRx.Diagnostics;
using UnityEngine.UI;

public class BoardBlock :MonoBehaviour {
    public void SetBlock(Block block) {
        switch (block.state) {
        case BlockState.Empty:
            image.sprite = null;
            image.color = Color.clear;
            break;
        case BlockState.Exist:
            image.sprite = blockSprite;
            image.color = block.type.GetColor();
            break;
        case BlockState.Erace:
            image.sprite = null;
            image.color = Color.white;
            break;
        }
    }
    [SerializeField] Image image;
    Sprite blockSprite;
    public void Init(Sprite blockSprite_) {
        blockSprite = blockSprite_;
    }
}
