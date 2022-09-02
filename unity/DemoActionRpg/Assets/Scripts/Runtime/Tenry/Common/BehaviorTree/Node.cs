using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

namespace Tenry.Common.BehaviorTree {
  public abstract class Node : ScriptableObject {
    #region Serialized Fields
    [SerializeField]
    [HideInInspector]
    private Vector2 position;

    [SerializeField]
    [HideInInspector]
    private string guid;
    #endregion

    internal GameObject gameObject;

    public GameObject GameObject => this.gameObject;

    internal BehaviorTree behaviorTree;

    public BehaviorTree BehaviorTree => this.behaviorTree;

    internal BehaviorTreeController controller;

    public BehaviorTreeController Controller => this.controller;

    public NodeStatus Status { get; protected set; } = NodeStatus.Running;

    public bool Started { get; protected set; } = false;

    public string Guid {
      get {
        return this.guid;
      }
      set {
        this.guid = value;
      }
    }

    public Vector2 Position {
      get {
        return this.position;
      }
      set {
        this.position = value;
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
      if (!this.Started) {
        this.OnStart();
        this.Started = true;
      }

      this.Status = this.OnUpdate();

      if (this.Status != NodeStatus.Running) {
        this.OnEnd();
        this.Started = false;
      }

      return this.Status;
    }

    public virtual Node Clone() {
      return Instantiate(this);
    }

    public void Traverse(System.Action<Node> visitor) {
      visitor?.Invoke(this);

      foreach (var child in this.GetChildren()) {
        child.Traverse(visitor);
      }
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
