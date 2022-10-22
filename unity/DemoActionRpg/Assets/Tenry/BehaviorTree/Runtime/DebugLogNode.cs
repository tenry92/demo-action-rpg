using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
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
        return message;
      }
      set {
        message = value;
      }
    }

    private string GetMessagePrefix(string phase) {
      var objectName = GameObject?.name ?? "?";
      var treeName = BehaviorTree?.name ?? "?";

      return $"<color=#007fff>{objectName}</color><color=#c9a96d>@{treeName}</color>(<color=#c96da6>{phase}</color>): ";
    }

    protected override void OnStart() {
      if (logStart) {
        Debug.Log($"{GetMessagePrefix("OnStart")}{Message}", this);
      }
    }

    protected override void OnEnd() {
      if (logEnd) {
        Debug.Log($"{GetMessagePrefix("OnEnd")}{Message}", this);
      }
    }

    protected override NodeStatus OnUpdate() {
      Debug.Log($"{GetMessagePrefix("OnUpdate")}{Message}", this);

      return NodeStatus.Success;
    }
  }
}
