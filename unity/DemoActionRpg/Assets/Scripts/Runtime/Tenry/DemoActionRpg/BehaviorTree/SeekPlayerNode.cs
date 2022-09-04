using UnityEngine;

using Tenry.BehaviorTree;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class SeekPlayerNode : ActionNode {
    private PlayerController playerController;

    protected override void OnStart() {
      this.playerController = this.GameObject.GetComponent<PlayerController>();
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (!this.Blackboard.TryGet<GameObject>("SeekTarget", out var seekTarget)) {
        return NodeStatus.Failure;
      }

      var seekFor = seekTarget.transform.position;

      var directionVector = seekFor - this.GameObject.transform.position;
      var direction = Vector3.SignedAngle(Vector3.forward, directionVector.normalized, Vector3.up);

      this.playerController.Move(direction);

      return NodeStatus.Running;
    }
  }
}
