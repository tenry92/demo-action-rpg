using System;

namespace Tenry.Common.BehaviorTree {
  [Flags]
  public enum NodeStatus {
    Running,
    Success,
    Failure,
  }
}
