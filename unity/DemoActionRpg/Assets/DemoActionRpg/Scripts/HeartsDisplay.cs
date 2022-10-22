using System.Collections;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class HeartsDisplay : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private HeartDisplay heartPrefab;

    [SerializeField]
    private Damageable reflectHealthOf;
    #endregion

#if UNITY_EDITOR
    private void OnValidate() {
      AdjustHeartsCount();
    }
#endif

    private void Start() {
      AdjustHeartsCount();
    }

    private void OnEnable() {
      reflectHealthOf.Changed.AddListener(UpdateHealth);
    }

    private void OnDisable() {
      reflectHealthOf.Changed.RemoveListener(UpdateHealth);
    }

    private void AdjustHeartsCount() {
      var diff = reflectHealthOf.MaxHealth - transform.childCount;

      if (diff > 0) {
        AddHearts(diff);
      } else if (diff < 0) {
        RemoveHearts(-diff);
      }
    }

    private void UpdateHealth() {
      for (int i = 0; i < reflectHealthOf.MaxHealth; i++) {
        var heart = transform.GetChild(i).GetComponent<HeartDisplay>();
        heart.Filled = i < reflectHealthOf.Health;
      }
    }

    private void AddHeart() {
      var heart = Instantiate(heartPrefab, transform);

#if UNITY_EDITOR
      heart.gameObject.hideFlags = HideFlags.HideAndDontSave;
#endif
    }

    private void AddHearts(int count) {
      for (int i = 0; i < count; ++i) {
        AddHeart();
      }
    }

    private void RemoveHeart() {
      var lastHeart = transform.GetChild(transform.childCount - 1);

#if UNITY_EDITOR
      if (Application.isPlaying) {
        Destroy(lastHeart);
      } else {
        DestroyImmediate(lastHeart);
      }
#else
      Destroy(lastHeart);
#endif
    }

    private void RemoveHearts(int count) {
      for (int i = 0; i < count; ++i) {
        RemoveHeart();
      }
    }
  }
}