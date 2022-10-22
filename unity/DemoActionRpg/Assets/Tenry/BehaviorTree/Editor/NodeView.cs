using System;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using Tenry.BehaviorTree.Runtime;

namespace Tenry.BehaviorTree.Editor {
  public class NodeView : UnityEditor.Experimental.GraphView.Node {
    public Runtime.Node Node { get; private set; }

    public Action Selected;

    private Port input;

    private Port output;

    internal Port Input => input;

    internal Port Output => output;

    public NodeView(Runtime.Node node) : base("Assets/Tenry/BehaviorTree/Editor/NodeView.uxml") {
      Node = node;
      title = Runtime.Node.GetUserFriendlyName(node.GetType());
      // todo: name = node.name;
      viewDataKey = node.Guid;

      style.left = Node.Position.x;
      style.top = Node.Position.y;

      CreatePorts();
      ApplyStyles();

      var noteLabel = this.Q<Label>("note");
      noteLabel.bindingPath = "note";
      noteLabel.Bind(new SerializedObject(node));
    }

    private void CreatePorts() {
      CreateInputPorts();
      CreateOutputPorts();
    }

    private void CreateInputPorts() {
      if (Node is ActionNode) {
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, null);
      } else if (Node is CompositeNode) {
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, null);
      } else if (Node is DecoratorNode) {
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, null);
      } else if (Node is RootNode) {

      }

      if (input != null) {
        input.portName = "";
        inputContainer.Add(input);
      }
    }

    private void CreateOutputPorts() {
      if (Node is ActionNode) {

      } else if (Node is CompositeNode) {
        output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, null);
      } else if (Node is DecoratorNode) {
        output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, null);
      } else if (Node is RootNode) {
        output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, null);
      }

      if (output != null) {
        output.portName = "";
        outputContainer.Add(output);
      }
    }

    private void ApplyStyles() {
      if (Node is ActionNode) {
        AddToClassList("action");
      } else if (Node is CompositeNode) {
        AddToClassList("composite");
      } else if (Node is DecoratorNode) {
        AddToClassList("decorator");
      } else if (Node is RootNode) {
        AddToClassList("root");
      }
    }

    public override void SetPosition(Rect position) {
      base.SetPosition(position);
      Undo.RecordObject(Node, "Behavior Tree (Set Position");
      Node.Position = new Vector2(position.xMin, position.yMin);
      EditorUtility.SetDirty(Node);
    }

    public override void OnSelected() {
      base.OnSelected();

      Selected?.Invoke();
    }

    public void UpdateState() {
      RemoveFromClassList("running");
      RemoveFromClassList("success");
      RemoveFromClassList("failure");

      if (Application.isPlaying) {
        switch (Node.Status) {
          case NodeStatus.Running:
            if (Node.Started) {
              AddToClassList("running");
            }
            break;
          case NodeStatus.Success:
            AddToClassList("success");
            break;
          case NodeStatus.Failure:
            AddToClassList("failure");
            break;
        }
      }
    }
  }
}
