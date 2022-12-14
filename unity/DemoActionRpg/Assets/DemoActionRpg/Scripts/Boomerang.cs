using Tenry.DemoActionRpg.Pool;
using Tenry.Utils;
using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Boomerang : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float distance = 5f;

    [SerializeField]
    private float speed = 5f;
    #endregion

    private Transform home;

    private Vector3 startPosition;

    private Vector3 farTarget;

    private float time;

    private void OnEnable() {
      time = 0f;
    }

    private void Update() {
      Vector3 pos;
      var duration = distance / speed;

      time += Time.deltaTime;

      if (time < duration) {
        pos = Vector3.Lerp(startPosition, farTarget, time / duration);
      } else {
        pos = Vector3.Lerp(farTarget, home.position, (time % duration) / duration);

        if (time >= 2 * duration) {
          gameObject.ReturnToPoolOrDestroy();
        }
      }

      transform.position = pos;
    }

    public void SetHome(Transform transform) {
      home = transform;
      startPosition = home.position;
      farTarget = startPosition + transform.forward * distance;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
      if (Application.isPlaying) {
        Gizmos.color = MoreColors.RoyalBlue.WithAlpha(0.5f);
        Gizmos.DrawSphere(startPosition, 1.2f);

        Gizmos.color = MoreColors.Brown.WithAlpha(0.5f);
        Gizmos.DrawSphere(farTarget, 1f);
      }

      Gizmos.color = MoreColors.PowderBlue;
      Gizmos.matrix = transform.localToWorldMatrix;

      Gizmos.DrawLine(Vector3.zero, Vector3.forward * distance);
    }
#endif
  }
}
