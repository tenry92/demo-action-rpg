namespace Tenry.BehaviorTree.Runtime {
  public class SelectorNode : CompositeNode {
    private int currentBranchIndex;

    private Node CurrentBranchNode => Children[currentBranchIndex];

    private bool HasReachedEnd => currentBranchIndex >= Children.Count;

    protected override void OnStart() {
      currentBranchIndex = 0;
    }

    protected override void OnEnd() { }

    protected override NodeStatus OnUpdate() {
      if (CheckAbort(currentBranchIndex - 1, out int branchIndex)) {
        CurrentBranchNode.Abort();
        var abortingNode = Children[branchIndex];

        switch (abortingNode.Status) {
          case NodeStatus.Running:
            currentBranchIndex = branchIndex;
            return NodeStatus.Running;
          case NodeStatus.Failure:
            currentBranchIndex = branchIndex + 1;
            break;
          case NodeStatus.Success:
            currentBranchIndex = branchIndex;
            return NodeStatus.Success;
        }
      }

      while (!HasReachedEnd) {
        switch (CurrentBranchNode.Evaluate()) {
          case NodeStatus.Running:
            return NodeStatus.Running;
          case NodeStatus.Failure:
            ++currentBranchIndex;
            continue;
          case NodeStatus.Success:
            return NodeStatus.Success;
        }
      }

      return NodeStatus.Failure;
    }
  }
}
