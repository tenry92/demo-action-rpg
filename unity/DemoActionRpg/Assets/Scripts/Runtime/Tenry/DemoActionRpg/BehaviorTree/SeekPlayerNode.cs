using UnityEngine;

using Tenry.BehaviorTree;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class SeekPlayerNode : ActionNode {
    private PlayerDetector detector;

    private PlayerController controller;

    protected override void OnStart() {
      this.detector = this.GameObject.GetComponent<PlayerDetector>();
      this.controller = this.GameObject.GetComponent<PlayerController>();
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (this.detector.DetectedPlayer == null) {
        return NodeStatus.Failure;
      }

      var seekFor = this.detector.DetectedPlayer.transform.position;

      var directionVector = seekFor - this.GameObject.transform.position;
      var direction = Vector3.SignedAngle(Vector3.forward, directionVector.normalized, Vector3.up);

      this.controller.Move(direction);

      return NodeStatus.Running;
    }
  }
}
