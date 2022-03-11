using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class TetrisFallTest {
    [Test]
    public void Test0() {
        foreach (var f in typeof(Main).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
            Debug.LogWarning(f.Name);
        }
    }
}
