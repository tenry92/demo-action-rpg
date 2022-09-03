using System;

namespace Tenry.BehaviorTree {
  [Flags]
  public enum AbortType {
    None = 0x0,

    /// <summary>
    /// This node may abort any running node on the right of this (lower priority).
    /// (Not implemented)
    /// </summary>
    LowerPriority = 0x1,

    /// <summary>
    /// A previously ended child node may abort the currently running branch.
    /// </summary>
    Self = 0x2,

    Both = LowerPriority | Self,
  }
}
