using UnityEngine;

using Cinemachine;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(CinemachineVirtualCamera))]
  public class CameraController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float orthoMin = 2f;

    [SerializeField]
    private float orthoMax = 10f;

    [SerializeField]
    private float zoomSpeed = 30f;
    #endregion

    private CinemachineVirtualCamera virtualCamera;

    private void Awake() {
      virtualCamera = GetComponent<CinemachineVirtualCamera>();
      Debug.Assert(virtualCamera != null);
    }

    private void OnValidate() {
      if (orthoMin < 0.1f) {
        orthoMin = 0.1f;
      }

      if (orthoMax < orthoMin) {
        orthoMax = orthoMin;
      }
    }

    /// <summary>
    /// A positive value zooms in, a negative value zooms out.
    /// </summary>
    public void Zoom(float delta) {
      var ortho = virtualCamera.m_Lens.OrthographicSize;

      ortho = Mathf.Clamp(ortho - delta * zoomSpeed, orthoMin, orthoMax);

      virtualCamera.m_Lens.OrthographicSize = ortho;
    }
  }
}
