using UnityEngine;

namespace Tenry.Common.BehaviorTree {
  public class DebugLogNode : ActionNode {
    #region Serialized Fields
    [SerializeField]
    private string message;

    [SerializeField]
    private bool logStart = false;

    [SerializeField]
    private bool logEnd = false;
    #endregion

    public string Message {
      get {
        return this.message;
      }
      set {
        this.message = value;
      }
    }

    protected override void OnStart() {
      if (this.logStart) {
        Debug.Log($"DebugLogNode({this.Message})@OnStart");
      }
    }

    protected override void OnEnd() {
      if (this.logEnd) {
        Debug.Log($"DebugLogNode({this.Message})@OnEnd");
      }
    }

    protected override NodeStatus OnUpdate() {
      Debug.Log(this.Message);

      return NodeStatus.Success;
    }
  }
}
