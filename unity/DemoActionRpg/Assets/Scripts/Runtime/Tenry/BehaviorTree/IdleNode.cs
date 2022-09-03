using UnityEngine;

namespace Tenry.BehaviorTree {
  public class IdleNode : ActionNode {
    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      return NodeStatus.Running;
    }
  }
}
