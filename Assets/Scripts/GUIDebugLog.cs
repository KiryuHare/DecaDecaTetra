using System.Collections.Generic;
using UnityEngine;

public class GUIDebugLog :MonoBehaviour {

    public List<string> logs;
    int i;
    void OnGUI() {
        // プレイヤーの位置を左上に表示
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), string.Concat(logs));
    }

    public void Log(string log) {
        logs.Add(log + "\n");
        if (logs.Count > 30) logs.RemoveAt(0);
    }

    public static GUIDebugLog Logger => GameObject.FindObjectOfType<GUIDebugLog>();

}