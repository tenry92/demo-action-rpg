using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Tenry.DemoActionRpg {
  public class HeartDisplay : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private Sprite filledSprite;

    [SerializeField]
    private Sprite emptySprite;
    #endregion

    private Image image;

    public bool Filled {
      set {
        if (value) {
          image.sprite = filledSprite;
        } else {
          image.sprite = emptySprite;
        }
      }
    }

    private void Awake() {
      image = GetComponent<Image>();
    }

#if UNITY_EDITOR
    private void OnValidate() {
      image = GetComponent<Image>();

      if (image && filledSprite) {
        image.sprite = filledSprite;
      }
    }
#endif
  }
}