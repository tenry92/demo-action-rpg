namespace Tenry.BehaviorTree {
  public class RepeatNode : DecoratorNode {
    protected override void OnStart() {}

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      this.Child.Evaluate();

      return NodeStatus.Running;
    }
  }
}
