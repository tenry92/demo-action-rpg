using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tenry.Common.BehaviorTree {
  [CreateAssetMenu()]
  public class BehaviorTree : ScriptableObject {
    #region Serialized Fields
    [SerializeField]
    private List<Node> nodes = new List<Node>();

    [SerializeField]
    private Node root;
    #endregion

    public Node Root {
      get {
        return this.root;
      }
      set {
        this.root = value;
      }
    }

    public NodeStatus Status = NodeStatus.Running;

    public List<Node> Nodes => this.nodes;

    public NodeStatus Update() {
      if (this.Root.Status == NodeStatus.Running) {
        this.Status = this.Root.Evaluate();
      }

      return this.Status;
    }

    public void AddChild(Node parent, Node child) {
      parent.AddChild(child);
    }

    public void RemoveChild(Node parent, Node child) {
      parent.RemoveChild(child);
    }

    public List<Node> GetChildren(Node parent) {
      return parent.GetChildren().ToList();
    }

    public BehaviorTree Clone() {
      var copy = Instantiate(this);
      copy.Root = copy.Root.Clone();

      return copy;
    }

    #if UNITY_EDITOR
    public Node CreateNode(System.Type type) {
      var node = ScriptableObject.CreateInstance(type) as Node;
      node.name = type.Name;
      node.Guid = GUID.Generate().ToString();
      this.nodes.Add(node);

      AssetDatabase.AddObjectToAsset(node, this);
      AssetDatabase.SaveAssets();

      return node;
    }

    public void DeleteNode(Node node) {
      this.nodes.Remove(node);
      AssetDatabase.RemoveObjectFromAsset(node);
      AssetDatabase.SaveAssets();
    }
    #endif
  }
}
