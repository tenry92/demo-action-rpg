using UnityEngine;

using TMPro;

namespace Tenry.DemoActionRpg {
  public class FloatingText : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private AnimationCurve positionOverTime;

    [SerializeField]
    private AnimationCurve alphaOverTime;
    #endregion

    private float age = 0f;

    private TMP_Text tmpText;

    private float MaxPositionTime {
      get {
        var lastKey = positionOverTime.keys[positionOverTime.length - 1];
        return lastKey.time;
      }
    }

    private float MaxAlphaTime {
      get {
        var lastKey = alphaOverTime.keys[alphaOverTime.length - 1];
        return lastKey.time;
      }
    }

    private float MaxAge => Mathf.Max(MaxPositionTime, MaxAlphaTime);

    private float originalPosition;

    private float Position {
      set {
        var transformPos = transform.position;
        transformPos.y = originalPosition;
        transform.position = transformPos + Vector3.up * value;
      }
    }

    private float Alpha {
      set {
        if (tmpText == null) {
          return;
        }

        var color = tmpText.color;
        color.a = value;
        tmpText.color = color;
      }
    }

    public string Text {
      set {
        if (tmpText == null) {
          return;
        }

        tmpText.text = value;
      }
    }

    private void Awake() {
      tmpText = GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
      originalPosition = transform.position.y;
    }

    private void Update() {
      Position = positionOverTime.Evaluate(age);
      Alpha = alphaOverTime.Evaluate(age);
    }

    private void LateUpdate() {
      age += Time.deltaTime;

      if (age >= MaxAge) {
        Destroy(gameObject);
      }
    }
  }
}
