using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchButton :MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler {
    //[SerializeField] UnityEvent off;
    //[SerializeField] UnityEvent on;

    [SerializeField] Image image;
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;

    public ReadOnlyReactiveProperty<bool> State => state.ToReadOnlyReactiveProperty();
    BoolReactiveProperty state;

    public void Init() {
        state = new BoolReactiveProperty(false);
        state.Subscribe(f => {
            image.sprite = f ? onSprite : offSprite;
        });
    }

    public void OnPointerDown(PointerEventData eventData) {
        GUIDebugLog.Logger.Log("DOWN");
        if (state is object) state.Value = true;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GUIDebugLog.Logger.Log("ENTER");
        if (state is object) state.Value = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GUIDebugLog.Logger.Log("EXIT");
        if (state is object) state.Value = false;
    }

    public void OnPointerUp(PointerEventData eventData) {
        GUIDebugLog.Logger.Log("UP");
        if (state is object) state.Value = false;
    }
}
