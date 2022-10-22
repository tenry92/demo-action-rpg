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
      this.Node = node;
      this.title = Runtime.Node.GetUserFriendlyName(node.GetType());
      // todo: this.name = node.name;
      this.viewDataKey = node.Guid;

      this.style.left = this.Node.Position.x;
      this.style.top = this.Node.Position.y;

      this.CreatePorts();
      this.ApplyStyles();

      var noteLabel = this.Q<Label>("note");
      noteLabel.bindingPath = "note";
      noteLabel.Bind(new SerializedObject(node));
    }

    private void CreatePorts() {
      this.CreateInputPorts();
      this.CreateOutputPorts();
    }

    private void CreateInputPorts() {
      if (this.Node is ActionNode) {
        this.input = this.InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, null);
      } else if (this.Node is CompositeNode) {
        this.input = this.InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, null);
      } else if (this.Node is DecoratorNode) {
        this.input = this.InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, null);
      } else if (this.Node is RootNode) {

      }

      if (this.input != null) {
        this.input.portName = "";
        this.inputContainer.Add(this.input);
      }
    }

    private void CreateOutputPorts() {
      if (this.Node is ActionNode) {

      } else if (this.Node is CompositeNode) {
        this.output = this.InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, null);
      } else if (this.Node is DecoratorNode) {
        this.output = this.InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, null);
      } else if (this.Node is RootNode) {
        this.output = this.InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, null);
      }

      if (this.output != null) {
        this.output.portName = "";
        this.outputContainer.Add(this.output);
      }
    }

    private void ApplyStyles() {
      if (this.Node is ActionNode) {
        this.AddToClassList("action");
      } else if (this.Node is CompositeNode) {
        this.AddToClassList("composite");
      } else if (this.Node is DecoratorNode) {
        this.AddToClassList("decorator");
      } else if (this.Node is RootNode) {
        this.AddToClassList("root");
      }
    }

    public override void SetPosition(Rect position) {
      base.SetPosition(position);
      Undo.RecordObject(this.Node, "Behavior Tree (Set Position");
      this.Node.Position = new Vector2(position.xMin, position.yMin);
      EditorUtility.SetDirty(this.Node);
    }

    public override void OnSelected() {
      base.OnSelected();

      this.Selected?.Invoke();
    }

    public void UpdateState() {
      this.RemoveFromClassList("running");
      this.RemoveFromClassList("success");
      this.RemoveFromClassList("failure");

      if (Application.isPlaying) {
        switch (this.Node.Status) {
          case NodeStatus.Running:
            if (this.Node.Started) {
              this.AddToClassList("running");
            }
            break;
          case NodeStatus.Success:
            this.AddToClassList("success");
            break;
          case NodeStatus.Failure:
            this.AddToClassList("failure");
            break;
        }
      }
    }
  }
}
