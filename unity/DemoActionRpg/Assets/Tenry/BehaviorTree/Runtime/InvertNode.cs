namespace Tenry.BehaviorTree.Runtime {
  public class InvertNode : DecoratorNode {
    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      switch (this.Child.Evaluate()) {
        default:
        case NodeStatus.Running:
          return NodeStatus.Running;
        case NodeStatus.Success:
          return NodeStatus.Failure;
        case NodeStatus.Failure:
          return NodeStatus.Success;
      }
    }
  }
}
