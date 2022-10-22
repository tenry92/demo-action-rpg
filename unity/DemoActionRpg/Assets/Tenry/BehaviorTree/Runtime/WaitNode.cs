using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public class WaitNode : ActionNode {
    #region Serialized Fields
    [SerializeField]
    /// Duration to wait in seconds.
    private float duration = 1f;
    #endregion

    /// Duration to wait in seconds.
    public float Duration {
      get {
        return this.duration;
      }
      set {
        this.duration = Mathf.Max(0f, value);
      }
    }

    private float startTime;

    protected override void OnStart() {
      this.startTime = Time.time;
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (Time.time - this.startTime >= this.Duration) {
        return NodeStatus.Success;
      }

      return NodeStatus.Running;
    }
  }
}
