using UnityEngine;
using UnityEngine.UIElements;

namespace Tenry.DemoActionRpg {
  public class HUD : MonoBehaviour {
    #region Serialized Properties
    [SerializeField]
    private Damageable reflectHealthOf;
    #endregion

    private VisualElement healthBar;

    private void Awake() {
      var root = GetComponent<UIDocument>().rootVisualElement;

      healthBar = root.Q<VisualElement>("HealthBar");
    }

    private void Update() {
      UpdateHealthBar();
    }

    private void UpdateHealthBar() {
      healthBar.style.width = new Length(((float) reflectHealthOf.Health) / reflectHealthOf.MaxHealth * 100, LengthUnit.Percent);
    }
  }
}
