using System.Collections.Generic;

namespace Tenry.BehaviorTree.Runtime {
  public class SequenceNode : CompositeNode {
    private int currentBranchIndex;

    private Node CurrentBranchNode => Children[currentBranchIndex];

    private bool HasReachedEnd => currentBranchIndex >= Children.Count;

    protected override void OnStart() {
      currentBranchIndex = 0;
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      if (CheckAbort(currentBranchIndex - 1, out int branchIndex)) {
        CurrentBranchNode.Abort();
        var abortingNode = Children[branchIndex];

        switch (abortingNode.Status) {
          case NodeStatus.Running:
            currentBranchIndex = branchIndex;
            return NodeStatus.Running;
          case NodeStatus.Failure:
            currentBranchIndex = branchIndex;
            return NodeStatus.Failure;
          case NodeStatus.Success:
            currentBranchIndex = branchIndex + 1;
            break;
        }
      }

      while (!HasReachedEnd) {
        switch (CurrentBranchNode.Evaluate()) {
          case NodeStatus.Running:
            return NodeStatus.Running;
          case NodeStatus.Failure:
            return NodeStatus.Failure;
          case NodeStatus.Success:
            ++currentBranchIndex;
            continue;
        }
      }

      return NodeStatus.Success;
    }
  }
}
