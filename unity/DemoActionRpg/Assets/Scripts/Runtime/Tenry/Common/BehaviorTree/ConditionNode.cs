using System.Threading;
using System.Threading.Tasks;

namespace Tenry.Common.BehaviorTree {
  public class ConditionNode : Node {
    public override string Name => "Condition";

    public Node Condition {
      get {
        return this.children[0];
      }
      set {
        this.children[0] = value;
      }
    }

    public Node Pass {
      get {
        return this.children[1];
      }
      set {
        this.children[1] = value;
      }
    }

    public Node Fail {
      get {
        return this.children[2];
      }
      set {
        this.children[2] = value;
      }
    }

    public ConditionNode() {
      this.Add(null); // condition
      this.Add(null); // pass
      this.Add(null); // fail
    }

    public override async Task<bool> Execute(CancellationToken token) {
      var passed = await this.Condition.Execute(token);

      if (token.IsCancellationRequested) {
        return false;
      }

      if (passed && this.Pass != null) {
        return await this.Pass.Execute(token);
      }

      if (!passed && this.Fail != null) {
        return await this.Fail.Execute(token);
      }

      return passed;
    }
  }
}
