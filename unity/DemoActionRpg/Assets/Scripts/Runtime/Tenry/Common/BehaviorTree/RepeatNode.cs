using System.Threading;
using System.Threading.Tasks;

namespace Tenry.Common.BehaviorTree {
  public class RepeatNode : Node {
    public override string Name => "Repeat";

    public override async Task<bool> Execute(CancellationToken token) {
      while (!token.IsCancellationRequested) {
        if (this.children.Count > 0) {
          await this.children[0].Execute(token);
        }

        await Task.Yield();
      }

      return false;
    }
  }
}
