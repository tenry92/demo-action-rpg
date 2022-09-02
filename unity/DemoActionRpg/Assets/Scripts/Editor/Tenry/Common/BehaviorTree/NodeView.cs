using System;

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Tenry.Common.BehaviorTree {
  public class NodeView : UnityEditor.Experimental.GraphView.Node {
    public Node Node { get; private set; }

    public Action Selected;

    private Port input;

    private Port output;

    internal Port Input => input;

    internal Port Output => output;

    public NodeView(Node node) {
      this.Node = node;
      this.title = node.name;
      this.viewDataKey = node.Guid;

      this.style.left = this.Node.Position.x;
      this.style.top = this.Node.Position.y;

      this.CreatePorts();
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

    public override void SetPosition(Rect position) {
      base.SetPosition(position);
      this.Node.Position = new Vector2(position.xMin, position.yMin);
    }

    public override void OnSelected() {
      base.OnSelected();

      this.Selected?.Invoke();
    }
  }
}
