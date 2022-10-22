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
      light = GetComponent<UniversalAdditionalLightData>();
      Debug.Assert(light);
    }

    private void Update() {
      var offset = light.lightCookieOffset;
      offset.x = Mathf.Repeat(offset.x + moveSpeed.x * Time.deltaTime, light.lightCookieSize.x);
      offset.y = Mathf.Repeat(offset.y + moveSpeed.y * Time.deltaTime, light.lightCookieSize.y);
      light.lightCookieOffset = offset;
    }
  }
}
