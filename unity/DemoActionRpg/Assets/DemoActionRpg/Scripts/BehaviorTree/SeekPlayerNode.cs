using UnityEngine;

using Tenry.BehaviorTree.Runtime;

namespace Tenry.DemoActionRpg.BehaviorTree {
  public class SeekPlayerNode : ActionNode {
    private PlayerController playerController;

    protected override void OnStart() {
      playerController = GameObject.GetComponent<PlayerController>();
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (!Blackboard.TryGet<GameObject>("SeekTarget", out var seekTarget)) {
        return NodeStatus.Failure;
      }

      var seekFor = seekTarget.transform.position;

      var directionVector = seekFor - GameObject.transform.position;
      var direction = Vector3.SignedAngle(Vector3.forward, directionVector.normalized, Vector3.up);

      playerController.Move(direction);

      return NodeStatus.Running;
    }
  }
}
