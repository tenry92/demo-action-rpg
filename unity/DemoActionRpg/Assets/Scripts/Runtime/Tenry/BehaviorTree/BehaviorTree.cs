using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tenry.BehaviorTree {
  [CreateAssetMenu()]
  public class BehaviorTree : ScriptableObject {
    #region Serialized Fields
    [SerializeField]
    private List<Node> nodes = new List<Node>();

    [SerializeField]
    private Node root;
    #endregion

    public Node Root {
      get => this.root;
      set => this.root = value;
    }

    public NodeStatus Status = NodeStatus.Running;

    public List<Node> Nodes => this.nodes;

    private BehaviorTreeController controller;

    public BehaviorTreeController Controller {
      get => this.controller;
      internal set {
        this.controller = value;

        if (this.Root != null) {
          this.Root.Traverse(node => {
            node.behaviorTree = this;
            node.controller = this.controller;
            node.gameObject = this.controller.gameObject;
          });
        }
      }
    }

    public Blackboard Blackboard { get; private set; }

    public NodeStatus Update() {
      if (this.Root.Status == NodeStatus.Running) {
        this.Status = this.Root.Evaluate();
      }

      return this.Status;
    }

    public void AddChild(Node parent, Node child) {
      Undo.RecordObject(parent, "Behavior Tree (Add Child)");
      parent.AddChild(child);
      EditorUtility.SetDirty(parent);
    }

    public void RemoveChild(Node parent, Node child) {
      Undo.RecordObject(parent, "Behavior Tree (Remove Child)");
      parent.RemoveChild(child);
      EditorUtility.SetDirty(parent);
    }

    public List<Node> GetChildren(Node parent) {
      return parent.GetChildren().ToList();
    }

    public BehaviorTree Clone() {
      var copy = Instantiate(this);
      copy.Blackboard = new Blackboard();
      copy.Root = copy.Root.Clone();
      copy.nodes = new List<Node>();

      // todo(?): we are missing detached nodes
      copy.Root.Traverse(node => {
        copy.nodes.Add(node);
      });

      return copy;
    }

    #if UNITY_EDITOR
    public Node CreateNode(System.Type type) {
      var node = ScriptableObject.CreateInstance(type) as Node;
      node.name = type.Name;
      node.Guid = GUID.Generate().ToString();

      Undo.RecordObject(this, "Behavior Tree (Create Node)");

      this.nodes.Add(node);

      if (!Application.isPlaying) {
        AssetDatabase.AddObjectToAsset(node, this);
      }

      Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (Create Node)");

      AssetDatabase.SaveAssets();

      return node;
    }

    public void DeleteNode(Node node) {
      Undo.RecordObject(this, "Behavior Tree (Delete Node)");
      this.nodes.Remove(node);

      Undo.DestroyObjectImmediate(node);

      AssetDatabase.SaveAssets();
    }
    #endif
  }
}
