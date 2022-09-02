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

    private string GetMessagePrefix(string phase) {
      var objectName = this.GameObject?.name ?? "?";
      var treeName = this.BehaviorTree?.name ?? "?";

      return $"<color=#007fff>{objectName}</color><color=#c9a96d>@{treeName}</color>(<color=#c96da6>{phase}</color>): ";
    }

    protected override void OnStart() {
      if (this.logStart) {
        Debug.Log($"{this.GetMessagePrefix("OnStart")}{this.Message}", this);
      }
    }

    protected override void OnEnd() {
      if (this.logEnd) {
        Debug.Log($"{this.GetMessagePrefix("OnEnd")}{this.Message}", this);
      }
    }

    protected override NodeStatus OnUpdate() {
      Debug.Log($"{this.GetMessagePrefix("OnUpdate")}{this.Message}", this);

      return NodeStatus.Success;
    }
  }
}
