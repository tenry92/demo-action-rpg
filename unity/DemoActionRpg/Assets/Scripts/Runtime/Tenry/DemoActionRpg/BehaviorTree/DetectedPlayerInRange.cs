using UnityEngine;

using Tenry.BehaviorTree;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class DetectedPlayerInRange : ActionNode {
    #region Serialized Fields
    [SerializeField]
    private float range = 1f;
    #endregion

    private PlayerDetector detector;

    protected override void OnStart() {
      this.detector = this.GameObject.GetComponent<PlayerDetector>();
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (this.detector.DetectedPlayer == null) {
        return NodeStatus.Failure;
      }

      var distance = Vector3.Distance(this.GameObject.transform.position, this.detector.DetectedPlayer.transform.position);

      if (distance <= range) {
        return NodeStatus.Success;
      }

      return NodeStatus.Failure;
    }
  }
}
