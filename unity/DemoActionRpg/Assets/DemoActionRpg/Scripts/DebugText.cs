using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using TMPro;

using Tenry.Common;

namespace Tenry.DemoActionRpg {
  public class DebugText : MonoBehaviour {
    private readonly List<string> linesToShow = new ();

    private List<KeyValuePair<string, float>> persistentLines = new ();

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
      text = GetComponent<TMP_Text>();
      framerateMeasure = GetComponent<FramerateMeasure>();
    }

    private void Update() {
      persistentLines = persistentLines.Where(entry => entry.Value > Time.unscaledTime).ToList();

      if (framerateMeasure != null) {
        float framerate = framerateMeasure.AverageFramerate;

        if (Application.targetFrameRate > 0) {
          Log("FPS", $"{framerate:F1} / {Application.targetFrameRate}");
        } else {
          Log("FPS", $"{framerate:F1} / ∞");
        }
      }
    }

    private void LateUpdate() {
      var lines = linesToShow.Concat(persistentLines.ConvertAll(entry => entry.Key));

      text.text = String.Join("\n", lines);
      linesToShow.Clear();
    }

    public void Log(string text) {
      linesToShow.Add(text);
    }

    public void Log(string key, string value) {
      linesToShow.Add($"<color=white>{key}</color> <b>{value}</b>");
    }

    public void LogPersistent(string text, float duration = Mathf.Infinity) {
      persistentLines.Add(new (text, Time.unscaledTime + duration));
    }

    public void LogPersistent(string key, string value, float duration = Mathf.Infinity) {
      persistentLines.Add(new ($"<color=white>{key}</color> <b>{value}</b>", Time.unscaledTime + duration));
    }
  }
}
