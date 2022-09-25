using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public class RepeatNode : DecoratorNode {
    #region Serialized Fields
    [SerializeField]
    private bool endOnFailure = false;
    #endregion

    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      var status = this.Child.Evaluate();

      if (this.endOnFailure && status == NodeStatus.Failure) {
        return NodeStatus.Failure;
      }

      return NodeStatus.Running;
    }
  }
}
