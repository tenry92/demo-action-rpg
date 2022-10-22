using System;

using UnityEngine;

namespace Tenry.Common {
  public class FramerateMeasure : MonoBehaviour {
    private int[] pastRates = new int[60];

    private int nextIndex = 0;

    public float AverageFramerate { get; private set; }

    private void OnEnable() {
      Array.Clear(pastRates, 0, pastRates.Length);
      AverageFramerate = 0f;
    }

    private void Start() {
      InvokeRepeating("CalculateAverage", 1f, 0.5f);
    }

    private void Update() {
      int framerate = Mathf.RoundToInt(1f / Time.deltaTime);
      pastRates[nextIndex] = framerate;

      nextIndex = (nextIndex + 1) % pastRates.Length;
    }

    private void CalculateAverage() {
      int sum = 0;

      for (int i = 0; i < pastRates.Length; ++i) {
        sum += pastRates[i];
      }

      AverageFramerate = ((float) sum) / pastRates.Length;
    }
  }
}
