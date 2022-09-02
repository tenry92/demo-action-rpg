using UnityEngine;

namespace Tenry.Common.BehaviorTree {
  public class DebugNode : ActionNode {
    #region Serialized Fields
    [SerializeField]
    private NodeStatus emitStatus;
    #endregion

    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      return this.emitStatus;
    }
  }
}
