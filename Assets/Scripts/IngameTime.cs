using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameTime :MonoBehaviour {
    public float Time { get; private set; }
    public float DeltaTime { get; private set; }
    public float Speed { get; set; }
    public void Init() {
        Time = 0.0f;
        Speed = 1;
        StopAllCoroutines();
        StartCoroutine(TimeLoop());
    }

    IEnumerator TimeLoop() {
        while (true) {
            var preFrameTime = Time;
            //Time = UnityEngine.Time.time;
            DeltaTime = UnityEngine.Time.deltaTime;
            Time += DeltaTime * Speed;
            yield return null;
        }
    }

    public IEnumerator Wait(float seconds) {
        var startTime = Time;
        while (true) {
            var passedTime = Time - startTime;
            if (passedTime >= seconds) break;
            yield return null;
        }
    }
}
