using System.Collections;

using Tenry.BehaviorTree.Runtime;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class MessageNode : ActionNode {
    #region Serialized Fields
    [SerializeField]
    [Multiline]
    private string message;
    #endregion

    protected override void OnEnd() { }

    protected override void OnStart() {
      MessageBox.ShowMessage(message);
    }

    protected override NodeStatus OnUpdate() {
      return NodeStatus.Running;
    }
  }
}