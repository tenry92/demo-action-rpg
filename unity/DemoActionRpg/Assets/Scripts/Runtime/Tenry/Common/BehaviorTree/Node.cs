using System.Collections.Generic;

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

    protected abstract void OnStart();

    protected abstract void OnEnd();

    protected abstract NodeStatus OnUpdate();

    public abstract void AddChild(Node child);

    public abstract void RemoveChild(Node child);

    public abstract IEnumerable<Node> GetChildren();
  }
}
