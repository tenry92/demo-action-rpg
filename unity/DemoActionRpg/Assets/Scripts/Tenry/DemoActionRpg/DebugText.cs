using System.Collections.Generic;
using System;

using UnityEngine;
using TMPro;

using Tenry.Common;

namespace Tenry.DemoActionRpg {
  public class DebugText : MonoBehaviour {
    private List<string> linesToShow = new ();

    private static DebugText instance;

    public static DebugText Instance {
      get {
        if (instance == null) {
          instance = GameObject.FindObjectOfType<DebugText>();
        }

        return instance;
      }
    }

    private TMP_Text text;

    private FramerateMeasure framerateMeasure;

    private void Awake() {
      this.text = this.GetComponent<TMP_Text>();
      this.framerateMeasure = this.GetComponent<FramerateMeasure>();
    }

    private void Update() {
      if (this.framerateMeasure != null) {
        float framerate = this.framerateMeasure.AverageFramerate;

        if (Application.targetFrameRate > 0) {
          this.Log("FPS", $"{framerate:F1} / {Application.targetFrameRate}");
        } else {
          this.Log("FPS", $"{framerate:F1} / ∞");
        }
      }
    }

    private void LateUpdate() {
      this.text.text = String.Join("\n", this.linesToShow);
      this.linesToShow.Clear();
    }

    public void Log(string text) {
      this.linesToShow.Add(text);
    }

    public void Log(string key, string value) {
      this.linesToShow.Add($"<color=white>{key}</color> <b>{value}</b>");
    }
  }
}
