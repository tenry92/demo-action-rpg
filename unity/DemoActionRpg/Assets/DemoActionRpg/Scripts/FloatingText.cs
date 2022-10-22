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
        var lastKey = this.positionOverTime.keys[this.positionOverTime.length - 1];
        return lastKey.time;
      }
    }

    private float MaxAlphaTime {
      get {
        var lastKey = this.alphaOverTime.keys[this.alphaOverTime.length - 1];
        return lastKey.time;
      }
    }

    private float MaxAge => Mathf.Max(this.MaxPositionTime, this.MaxAlphaTime);

    private float originalPosition;

    private float Position {
      set {
        var transformPos = this.transform.position;
        transformPos.y = originalPosition;
        this.transform.position = transformPos + Vector3.up * value;
      }
    }

    private float Alpha {
      set {
        if (this.tmpText == null) {
          return;
        }

        var color = this.tmpText.color;
        color.a = value;
        this.tmpText.color = color;
      }
    }

    public string Text {
      set {
        if (this.tmpText == null) {
          return;
        }

        this.tmpText.text = value;
      }
    }

    private void Awake() {
      this.tmpText = this.GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
      this.originalPosition = this.transform.position.y;
    }

    private void Update() {
      this.Position = this.positionOverTime.Evaluate(this.age);
      this.Alpha = this.alphaOverTime.Evaluate(this.age);
    }

    private void LateUpdate() {
      this.age += Time.deltaTime;

      if (this.age >= this.MaxAge) {
        Destroy(this.gameObject);
      }
    }
  }
}
