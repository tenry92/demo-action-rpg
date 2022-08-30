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
      Debug.Assert(this.virtualCamera = this.GetComponent<CinemachineVirtualCamera>());
    }

    private void OnValidate() {
      if (this.orthoMin < 0.1f) {
        this.orthoMin = 0.1f;
      }

      if (this.orthoMax < this.orthoMin) {
        this.orthoMax = this.orthoMin;
      }
    }

    /// <summary>
    /// A positive value zooms in, a negative value zooms out.
    /// </summary>
    public void Zoom(float delta) {
      var ortho = this.virtualCamera.m_Lens.OrthographicSize;

      ortho = Mathf.Clamp(ortho - delta * this.zoomSpeed, this.orthoMin, this.orthoMax);

      this.virtualCamera.m_Lens.OrthographicSize = ortho;
    }
  }
}
