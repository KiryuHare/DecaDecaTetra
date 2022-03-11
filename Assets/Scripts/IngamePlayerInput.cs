using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx.Diagnostics;

public class IngamePlayerInput :MonoBehaviour {

    //Vector2 dir;
    bool upKey, downKey, rightKey, leftKey;
    bool rotRKey, rotLKey, holdKey;

    public ReactiveProperty<bool> up, down, left, right;
    public ReactiveProperty<bool> leftHold, rightHold;
    public ReactiveProperty<bool> rotR, rotL, hold;
    bool upHoldFlag, rRotHoldFlag, lRotHoldFlag, holdHoldFlag;
    float leftHoldTime, rightHoldTime;
    [SerializeField] AudioClip seButton;

    void SetUpKey(bool f) {
        upKey = f;
        if (f) aud?.PlaySE(seButton);
    }
    void SetDownKey(bool f) {
        downKey = f;
        if (f) aud?.PlaySE(seButton);
    }

    void SetLeftKey(bool f) {
        leftKey = f;
        if (f) aud?.PlaySE(seButton);
    }
    void SetRightKey(bool f) {
        rightKey = f;
        if (f) aud?.PlaySE(seButton);
    }
    void SetRotRKey(bool f) {
        rotRKey = f;
        if (f) aud?.PlaySE(seButton);
    }
    void SetRotLKey(bool f) {
        rotLKey = f;
        if (f) aud?.PlaySE(seButton);
    }
    void SetHoldKey(bool f) {
        holdKey = f;
        if (f) aud?.PlaySE(seButton);
    }

    private void Start() {
        aud = AudioManager.Get();
        this.UpdateAsObservable().Subscribe(_ => {
            if (Input.GetKeyDown(KeyCode.UpArrow)) SetUpKey(true);
            if (Input.GetKeyUp(KeyCode.UpArrow)) SetUpKey(false);
            if (Input.GetKeyDown(KeyCode.DownArrow)) SetDownKey(true);
            if (Input.GetKeyUp(KeyCode.DownArrow)) SetDownKey(false);
            if (Input.GetKeyDown(KeyCode.RightArrow)) SetRightKey(true);
            if (Input.GetKeyUp(KeyCode.RightArrow)) SetRightKey(false);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) SetLeftKey(true);
            if (Input.GetKeyUp(KeyCode.LeftArrow)) SetLeftKey(false);
            if (Input.GetKeyUp(KeyCode.LeftShift)) SetHoldKey(false);
            if (Input.GetKeyDown(KeyCode.LeftShift)) SetHoldKey(true);
            if (Input.GetKeyUp(KeyCode.X)) SetRotRKey(false);
            if (Input.GetKeyDown(KeyCode.X)) SetRotRKey(true);
            if (Input.GetKeyUp(KeyCode.Z)) SetRotLKey(false);
            if (Input.GetKeyDown(KeyCode.Z)) SetRotLKey(true);
        }).AddTo(this);


        this.UpdateAsObservable().Subscribe(_ => {
            if (!initialized) return;

            up.Value = !upHoldFlag && upKey;
            upHoldFlag = upKey;

            down.Value = downKey;

            rotR.Value = !rRotHoldFlag && rotRKey;
            rRotHoldFlag = rotRKey;
            rotL.Value = !lRotHoldFlag && rotLKey;
            lRotHoldFlag = rotLKey;
            hold.Value = !holdHoldFlag && holdKey;
            holdHoldFlag = holdKey;
            if (rightKey) {
                right.Value = rightHoldTime == 0;
                rightHold.Value = rightHoldTime > holdTime;
                rightHoldTime += ingameTime.DeltaTime;
            } else {
                right.Value = false;
                rightHold.Value = false;
                rightHoldTime = 0;
            }

            if (leftKey) {
                left.Value = leftHoldTime == 0;
                leftHold.Value = leftHoldTime > holdTime;
                leftHoldTime += ingameTime.DeltaTime;
            } else {
                left.Value = false;
                leftHold.Value = false;
                leftHoldTime = 0;
            }

        }).AddTo(this);
    }

    AudioManager aud;
    float holdTime;
    IngameTime ingameTime;
    bool initialized = false;
    public void Init(IngameTime ingameTime_, float holdTime_) {
        ingameTime = ingameTime_;
        holdTime = holdTime_;

        Up.Init();
        Up.State.Subscribe(f => SetUpKey(f));
        Down.Init();
        Down.State.Subscribe(f => SetDownKey(f));
        Left.Init();
        Left.State.Subscribe(f => SetLeftKey(f));
        Right.Init();
        Right.State.Subscribe(f => SetRightKey(f));
        RRot.Init();
        RRot.State.Subscribe(f => SetRotRKey(f));
        LRot.Init();
        LRot.State.Subscribe(f => SetRotLKey(f));
        Hold.Init();
        Hold.State.Subscribe(f => SetHoldKey(f));
        up = new ReactiveProperty<bool>();
        down = new ReactiveProperty<bool>();
        leftHold = new ReactiveProperty<bool>();
        rightHold = new ReactiveProperty<bool>();
        rotR = new ReactiveProperty<bool>();
        rotL = new ReactiveProperty<bool>();
        hold = new ReactiveProperty<bool>();

        initialized = true;
    }

    [SerializeField] TouchButton Up;
    [SerializeField] TouchButton Down;
    [SerializeField] TouchButton Left;
    [SerializeField] TouchButton Right;
    [SerializeField] TouchButton RRot;
    [SerializeField] TouchButton LRot;
    [SerializeField] TouchButton Hold;
}
