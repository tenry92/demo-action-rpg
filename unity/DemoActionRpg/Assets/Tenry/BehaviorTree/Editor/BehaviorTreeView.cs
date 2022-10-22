using System;
using System.Collections.Generic;
using System.Linq;

using Tenry.BehaviorTree.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tenry.BehaviorTree.Editor {
  public class BehaviorTreeView : GraphView {
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }

    private Runtime.BehaviorTree tree;

    public Action<Runtime.Node> NodeSelected;

    public BehaviorTreeView() {
      Insert(0, new GridBackground());

      this.AddManipulator(new ContentZoomer());
      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new RectangleSelector());

      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Tenry/BehaviorTree/Editor/BehaviorTreeView.uss");
      styleSheets.Add(styleSheet);

      Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo() {
      PopulateView(tree);
      AssetDatabase.SaveAssets();
    }

    private NodeView FindNodeViewByNode(Runtime.Node node) {
      return GetNodeByGuid(node.Guid) as NodeView;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent ev) {
      var viewTransform = contentViewContainer.transform;
      var position = ev.localMousePosition;
      position.x = (position.x - viewTransform.position.x) / viewTransform.scale.x;
      position.y = (position.y - viewTransform.position.y) / viewTransform.scale.y;

      {
        ev.menu.AppendAction("Actions", null, DropdownMenuAction.Status.Disabled);

        var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
        foreach (var type in types) {
          ev.menu.AppendAction(Runtime.Node.GetUserFriendlyName(type), action => CreateNode(type, position));
        }
      }

      ev.menu.AppendSeparator();

      {
        ev.menu.AppendAction("Composites", null, DropdownMenuAction.Status.Disabled);

        var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
        foreach (var type in types) {
          ev.menu.AppendAction(Runtime.Node.GetUserFriendlyName(type), action => CreateNode(type, position));
        }
      }

      ev.menu.AppendSeparator();

      {
        ev.menu.AppendAction("Decorators", null, DropdownMenuAction.Status.Disabled);

        var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        foreach (var type in types) {
          ev.menu.AppendAction(Runtime.Node.GetUserFriendlyName(type), action => CreateNode(type, position));
        }
      }
    }

    private void CreateNode(System.Type type) {
      var node = tree.CreateNode(type);
      CreateNodeView(node);
    }

    private void CreateNode(System.Type type, Vector2 position) {
      var node = tree.CreateNode(type);
      node.Position = position;
      CreateNodeView(node);
    }

    internal void PopulateView(Runtime.BehaviorTree tree) {
      this.tree = tree;

      ClearView();

      // todo: do this in BehaviorTree instead?
      if (this.tree.Root == null) {
        this.tree.Root = this.tree.CreateNode(typeof(RootNode));
        EditorUtility.SetDirty(this.tree);
        AssetDatabase.SaveAssets();
      }

      CreateNodeViews();
      ConnectEdges();
    }

    private void ClearView() {
      graphViewChanged -= OnGraphViewChanged;
      DeleteElements(graphElements);
      graphViewChanged += OnGraphViewChanged;
    }

    private void CreateNodeViews() {
      foreach (var node in tree.Nodes) {
        CreateNodeView(node);
      }
    }

    private void ConnectEdges() {
      foreach (var node in tree.Nodes) {
        var children = tree.GetChildren(node);

        foreach (var child in children) {
          var parentView = FindNodeViewByNode(node);
          var childView = FindNodeViewByNode(child);

          var edge = parentView.Output.ConnectTo(childView.Input);
          AddElement(edge);
        }
      }
    }

    private static bool PortsCompatible(Port startPort, Port endPort) {
      // todo: disallow children to be connected back to parents (creating loops)

      return startPort != endPort && startPort.direction != endPort.direction;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
      return ports.ToList().Where(endPort => PortsCompatible(startPort, endPort)).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange change) {
      if (change.elementsToRemove != null) {
        foreach (var element in change.elementsToRemove) {
          if (element is NodeView nodeView) {
            tree.DeleteNode(nodeView.Node);
          }

          if (element is Edge edge) {
            var parentView = edge.output.node as NodeView;
            var childView = edge.input.node as NodeView;
            tree.RemoveChild(parentView.Node, childView.Node);
          }
        }
      }

      if (change.edgesToCreate != null) {
        foreach (var edge in change.edgesToCreate) {
          var parentView = edge.output.node as NodeView;
          var childView = edge.input.node as NodeView;
          tree.AddChild(parentView.Node, childView.Node);
        }
      }

      if (change.edgesToCreate != null || change.movedElements != null) {
        foreach (var node in nodes) {
          var view = node as NodeView;
          view.Node.SortChildren();
        }
      }

      return change;
    }

    private void CreateNodeView(Runtime.Node node) {
      var nodeView = new NodeView(node);

      nodeView.Selected = () => {
        NodeSelected?.Invoke(nodeView.Node);
      };

      AddElement(nodeView);
    }

    public void UpdateNodeStates() {
      foreach (var graphNode in nodes) {
        var nodeView = graphNode as NodeView;
        nodeView?.UpdateState();
      }
    }
  }
}
