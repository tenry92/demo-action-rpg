using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

using Tenry.BehaviorTree.Runtime;

namespace Tenry.BehaviorTree.Editor {
  public class BehaviorTreeView : GraphView {
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> {}

    private Runtime.BehaviorTree tree;

    public Action<Runtime.Node> NodeSelected;

    public BehaviorTreeView() {
      this.Insert(0, new GridBackground());

      this.AddManipulator(new ContentZoomer());
      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new RectangleSelector());

      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Tenry/BehaviorTree/Editor/BehaviorTreeEditor.uss");
      this.styleSheets.Add(styleSheet);

      Undo.undoRedoPerformed += this.OnUndoRedo;
    }

    private void OnUndoRedo() {
      this.PopulateView(this.tree);
      AssetDatabase.SaveAssets();
    }

    private NodeView FindNodeViewByNode(Runtime.Node node) {
      return this.GetNodeByGuid(node.Guid) as NodeView;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent ev) {
      var viewTransform = this.contentViewContainer.transform;
      var position = ev.localMousePosition;
      position.x = (position.x - viewTransform.position.x) / viewTransform.scale.x;
      position.y = (position.y - viewTransform.position.y) / viewTransform.scale.y;
      
      {
        ev.menu.AppendAction("Actions", null, DropdownMenuAction.Status.Disabled);

        var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
        foreach (var type in types) {
          ev.menu.AppendAction(Runtime.Node.GetUserFriendlyName(type), action => this.CreateNode(type, position));
        }
      }

      ev.menu.AppendSeparator();

      {
        ev.menu.AppendAction("Composites", null, DropdownMenuAction.Status.Disabled);

        var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
        foreach (var type in types) {
          ev.menu.AppendAction(Runtime.Node.GetUserFriendlyName(type), action => this.CreateNode(type, position));
        }
      }

      ev.menu.AppendSeparator();

      {
        ev.menu.AppendAction("Decorators", null, DropdownMenuAction.Status.Disabled);

        var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        foreach (var type in types) {
          ev.menu.AppendAction(Runtime.Node.GetUserFriendlyName(type), action => this.CreateNode(type, position));
        }
      }
    }

    private void CreateNode(System.Type type) {
      var node = this.tree.CreateNode(type);
      this.CreateNodeView(node);
    }

    private void CreateNode(System.Type type, Vector2 position) {
      var node = this.tree.CreateNode(type);
      node.Position = position;
      this.CreateNodeView(node);
    }

    internal void PopulateView(Runtime.BehaviorTree tree) {
      this.tree = tree;

      this.ClearView();

      // todo: do this in BehaviorTree instead?
      if (this.tree.Root == null) {
        this.tree.Root = this.tree.CreateNode(typeof(RootNode));
        EditorUtility.SetDirty(this.tree);
        AssetDatabase.SaveAssets();
      }

      this.CreateNodeViews();
      this.ConnectEdges();
    }

    private void ClearView() {
      this.graphViewChanged -= this.OnGraphViewChanged;
      this.DeleteElements(this.graphElements);
      this.graphViewChanged += this.OnGraphViewChanged;
    }

    private void CreateNodeViews() {
      foreach (var node in this.tree.Nodes) {
        this.CreateNodeView(node);
      }
    }

    private void ConnectEdges() {
      foreach (var node in this.tree.Nodes) {
        var children = this.tree.GetChildren(node);

        foreach (var child in children) {
          var parentView = this.FindNodeViewByNode(node);
          var childView = this.FindNodeViewByNode(child);

          var edge = parentView.Output.ConnectTo(childView.Input);
          this.AddElement(edge);
        }
      }
    }

    private static bool PortsCompatible(Port startPort, Port endPort) {
      // todo: disallow children to be connected back to parents (creating loops)

      return startPort != endPort && startPort.direction != endPort.direction;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
      return this.ports.ToList().Where(endPort => PortsCompatible(startPort, endPort)).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange change) {
      if (change.elementsToRemove != null) {
        foreach (var element in change.elementsToRemove) {
          var nodeView = element as NodeView;

          if (nodeView != null) {
            this.tree.DeleteNode(nodeView.Node);
          }

          var edge = element as Edge;

          if (edge != null) {
            var parentView = edge.output.node as NodeView;
            var childView = edge.input.node as NodeView;
            this.tree.RemoveChild(parentView.Node, childView.Node);
          }
        }
      }

      if (change.edgesToCreate != null) {
        foreach (var edge in change.edgesToCreate) {
          var parentView = edge.output.node as NodeView;
          var childView = edge.input.node as NodeView;
          this.tree.AddChild(parentView.Node, childView.Node);
        }
      }

      if (change.edgesToCreate != null || change.movedElements != null) {
        foreach (var node in this.nodes) {
          var view = node as NodeView;
          view.Node.SortChildren();
        }
      }

      return change;
    }

    private void CreateNodeView(Runtime.Node node) {
      NodeView nodeView = new NodeView(node);

      nodeView.Selected = () => {
        this.NodeSelected?.Invoke(nodeView.Node);
      };

      this.AddElement(nodeView);
    }

    public void UpdateNodeStates() {
      foreach (var graphNode in this.nodes) {
        var nodeView = graphNode as NodeView;
        nodeView?.UpdateState();
      }
    }
  }
}
