using System.Collections.Generic;

namespace Tenry.BehaviorTree.Runtime {
  public class Blackboard {
    private readonly Dictionary<string, object> data = new ();

    public bool TryGet<T>(string key, out T result) {
      if (data.TryGetValue(key, out var value)) {
        if (value is T typedValue) {
          result = typedValue;
          return true;
        }
      }

      result = default;

      return false;
    }

    public void Set(string key, object value) {
      data[key] = value;
    }
  }
}
