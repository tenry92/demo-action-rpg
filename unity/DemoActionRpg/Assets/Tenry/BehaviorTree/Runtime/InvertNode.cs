namespace Tenry.BehaviorTree.Runtime {
  public class InvertNode : DecoratorNode {
    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      return Child.Evaluate() switch {
        NodeStatus.Running => NodeStatus.Running,
        NodeStatus.Success => NodeStatus.Failure,
        NodeStatus.Failure => NodeStatus.Success,
        _ => NodeStatus.Running,
      };
    }
  }
}
