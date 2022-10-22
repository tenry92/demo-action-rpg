using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

namespace Tenry.BehaviorTree.Runtime {
  public abstract class Node : ScriptableObject {
    #region Serialized Fields
    [SerializeField]
    [HideInInspector]
    private Vector2 position;

    [SerializeField]
    [HideInInspector]
    private string guid;

    [SerializeField]
    private string note;
    #endregion

    internal GameObject gameObject;

    public GameObject GameObject => gameObject;

    internal BehaviorTree behaviorTree;

    public BehaviorTree BehaviorTree => behaviorTree;

    internal BehaviorTreeController controller;

    public BehaviorTreeController Controller => controller;

    protected Blackboard Blackboard => BehaviorTree.Blackboard;

    public NodeStatus Status { get; protected set; } = NodeStatus.Running;

    public bool Started { get; protected set; } = false;

    public bool IsRunning => Started == true && Status == NodeStatus.Running;

    public string Guid {
      get {
        return guid;
      }
      set {
        guid = value;
      }
    }

    public string Note {
      get => note;
      set => note = value;
    }

    public Vector2 Position {
      get {
        return position;
      }
      set {
        position = value;
      }
    }

    public static string GetUserFriendlyName(System.Type type) {
      var niceName = Regex.Replace(type.Name, "(\\B[A-Z])", " $1");

      if (niceName.EndsWith(" Node")) {
        niceName = niceName.Remove(niceName.Length - " Node".Length);
      }

      return niceName;
    }

    public NodeStatus Evaluate() {
      if (!Started) {
        OnStart();
        Started = true;
      }

      Status = OnUpdate();

      if (Status != NodeStatus.Running) {
        OnEnd();
        Started = false;
      }

      return Status;
    }

    public virtual Node Clone() {
      return Instantiate(this);
    }

    public void Traverse(System.Action<Node> visitor) {
      visitor?.Invoke(this);

      foreach (var child in GetChildren()) {
        child.Traverse(visitor);
      }
    }

    public virtual void Abort() {
      if (IsRunning) {
        OnEnd();
        ResetStatus();
      }
    }

    private void ResetStatus() {
      Started = false;
      Status = NodeStatus.Running;
    }

    protected abstract void OnStart();

    protected abstract void OnEnd();

    protected abstract NodeStatus OnUpdate();

    public abstract void AddChild(Node child);

    public abstract void RemoveChild(Node child);

    public abstract IEnumerable<Node> GetChildren();

    public abstract void SortChildren();
  }
}
