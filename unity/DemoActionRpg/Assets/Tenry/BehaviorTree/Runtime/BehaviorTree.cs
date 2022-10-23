using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tenry.BehaviorTree.Runtime {
  [CreateAssetMenu(menuName = "Tenry/Behavior Tree", fileName = "BehaviorTree")]
  public class BehaviorTree : ScriptableObject {
    #region Serialized Fields
    [SerializeField]
    private List<Node> nodes = new List<Node>();

    [SerializeField]
    private Node root;
    #endregion

    public Node Root {
      get => root;
      set => root = value;
    }

    public NodeStatus Status = NodeStatus.Running;

    public List<Node> Nodes => nodes;

    private BehaviorTreeController controller;

    public BehaviorTreeController Controller {
      get => controller;
      internal set {
        controller = value;

        if (Root != null) {
          Root.Traverse(node => {
            node.behaviorTree = this;
            node.controller = controller;
            node.gameObject = controller.gameObject;
          });
        }
      }
    }

    public Blackboard Blackboard { get; private set; }

    public NodeStatus Update() {
      if (Root.Status == NodeStatus.Running) {
        Status = Root.Evaluate();
      }

      return Status;
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

    public void Restart() {
      root.Restart();
    }

#if UNITY_EDITOR
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

    public Node CreateNode(System.Type type) {
      var node = ScriptableObject.CreateInstance(type) as Node;
      node.name = type.Name;
      node.Guid = GUID.Generate().ToString();

      Undo.RecordObject(this, "Behavior Tree (Create Node)");

      nodes.Add(node);

      if (!Application.isPlaying) {
        AssetDatabase.AddObjectToAsset(node, this);
      }

      Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (Create Node)");

      AssetDatabase.SaveAssets();

      return node;
    }

    public void DeleteNode(Node node) {
      Undo.RecordObject(this, "Behavior Tree (Delete Node)");
      nodes.Remove(node);

      Undo.DestroyObjectImmediate(node);

      AssetDatabase.SaveAssets();
    }
#endif
  }
}
