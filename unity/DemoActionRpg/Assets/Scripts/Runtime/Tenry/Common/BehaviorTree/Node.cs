using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tenry.Common.BehaviorTree {
  public abstract class Node : ICollection<Node> {
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    protected List<Node> children = new List<Node>();

    public int Count => this.children.Count;

    public bool IsReadOnly => false;

    public virtual string Name { get; }

    public void Add(Node child) {
      this.children.Add(child);
    }

    public bool Remove(Node child) {
      return this.children.Remove(child);
    }

    public void Clear() {
      this.children.Clear();
    }

    public bool Contains(Node node) {
      return this.children.Contains(node);
    }

    public void CopyTo(Node[] array, int arrayIndex) {
      this.children.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Node> GetEnumerator() {
      return this.children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.children.GetEnumerator();
    }

    public void Cancel() {
      this.cancellationTokenSource.Cancel();
    }

    public async Task<bool> Execute() {
      return await this.Execute(this.cancellationTokenSource.Token);
    }

    public abstract Task<bool> Execute(CancellationToken token);
  }
}
