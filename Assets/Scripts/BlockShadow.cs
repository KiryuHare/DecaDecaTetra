using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BlockShadow :MonoBehaviour {
    public void SetBlock(Block block) {
        switch (block.state) {
        case BlockState.Empty:
            colorEvent.Invoke(Color.clear);
            break;
        case BlockState.Exist:
            colorEvent.Invoke(block.type.GetColor() * 0.5f);
            break;
        case BlockState.Erace:
            colorEvent.Invoke(Color.white * 0.5f);
            break;
        }
    }
    [SerializeField] UnityEvent<Color> colorEvent;
}
