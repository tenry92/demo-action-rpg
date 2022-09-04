using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(Light))]
  public class LightCookieMover : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private Vector2 moveSpeed;
    #endregion

    private new UniversalAdditionalLightData light;

    private void Awake() {
      this.light = this.GetComponent<UniversalAdditionalLightData>();
      Debug.Assert(this.light);
    }

    private void Update() {
      var offset = this.light.lightCookieOffset;
      offset.x = Mathf.Repeat(offset.x + this.moveSpeed.x * Time.deltaTime, this.light.lightCookieSize.x);
      offset.y = Mathf.Repeat(offset.y + this.moveSpeed.y * Time.deltaTime, this.light.lightCookieSize.y);
      this.light.lightCookieOffset = offset;
    }
  }
}
