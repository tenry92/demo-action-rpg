using System.Collections.Generic;

namespace Tenry.Common.BehaviorTree {
  public abstract class ActionNode : Node {
    public override void AddChild(Node child) {
      // nothing
    }

    public override void RemoveChild(Node child) {
      // nothing
    }

    public override IEnumerable<Node> GetChildren() {
      return new List<Node>();
    }

    public override void SortChildren() {}
  }
}
