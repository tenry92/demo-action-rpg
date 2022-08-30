using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tenry.Common.BehaviorTree {
  public class CallbackNode : Node {
    public Func<CancellationToken, Task<bool>> Callback;

    public override string Name => "Callback";

    public CallbackNode(Func<CancellationToken, Task<bool>> callback) {
      this.Callback = callback;
    }

    public override async Task<bool> Execute(CancellationToken token) {
      if (this.Callback != null) {
        return await this.Callback(token);
      }

      return false;
    }
  }
}
