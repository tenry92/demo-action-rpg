using System.Collections.Generic;

namespace Tenry.BehaviorTree.Runtime {
  public class Blackboard {
    private Dictionary<string, object> data = new Dictionary<string, object>();

    public bool TryGet<T>(string key, out T result) {
      if (this.data.TryGetValue(key, out var value)) {
        if (value is T) {
          result = (T) value;
          return true;
        }
      }

      result = default(T);

      return false;
    }

    public void Set(string key, object value) {
      this.data[key] = value;
    }
  }
}
