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
      var root = this.GetComponent<UIDocument>().rootVisualElement;

      this.healthBar = root.Q<VisualElement>("HealthBar");
    }

    private void Update() {
      this.UpdateHealthBar();
    }

    private void UpdateHealthBar() {
      this.healthBar.style.flexGrow = ((float) this.reflectHealthOf.Health) / this.reflectHealthOf.MaxHealth;
    }
  }
}
