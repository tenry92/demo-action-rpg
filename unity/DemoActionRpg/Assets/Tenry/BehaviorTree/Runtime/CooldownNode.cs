using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public class CooldownNode : DecoratorNode {
    #region Serialized Fields
    [SerializeField]
    [Min(0.01f)]
    private float cooldown = 1f;
    #endregion

    private float readyAt = 0f;

    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (Time.time < readyAt) {
        return NodeStatus.Failure;
      }

      var status = Child.Evaluate();

      switch (status) {
        case NodeStatus.Running:
          break;
        case NodeStatus.Success:
        case NodeStatus.Failure:
          readyAt = Time.time + cooldown;
          break;
      }

      return status;
    }
  }
}
