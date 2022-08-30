using System.Threading;
using System.Threading.Tasks;

namespace Tenry.Common.BehaviorTree {
  public class SequenceNode : Node {
    public override string Name => "Sequence";

    public override async Task<bool> Execute(CancellationToken token) {
      foreach (var child in this) {
        if (token.IsCancellationRequested) {
          return false;
        }

        if (await child.Execute(token) == false) {
          return false;
        }
      }

      return true;
    }
  }
}
