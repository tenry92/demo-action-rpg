using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using Tenry.Common.UIElements;

namespace Tenry.Common.BehaviorTree {
  public class BehaviorTreeEditor : EditorWindow {
    private BehaviorTreeView treeView;

    private InspectorView inspectorView;

    public BehaviorTree ActiveTree => Selection.activeObject as BehaviorTree;

    [MenuItem("Window/Tenry/Behavior Tree Editor")]
    public static void ShowWindow() {
      BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
      wnd.titleContent = new GUIContent("Behavior Tree Editor");
    }

    public void CreateGUI() {
      // Each editor window contains a root VisualElement object
      VisualElement root = rootVisualElement;

      var basePath = "Assets/Scripts/Editor/Tenry/Common/BehaviorTree";

      // Import UXML
      var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{basePath}/BehaviorTreeEditor.uxml");
      visualTree.CloneTree(root);

      // A stylesheet can be added to a VisualElement.
      // The style will be applied to the VisualElement and all of its children.
      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{basePath}/BehaviorTreeEditor.uss");
      root.styleSheets.Add(styleSheet);

      this.treeView = root.Q<BehaviorTreeView>();
      this.inspectorView = root.Q<InspectorView>();

      this.treeView.NodeSelected = this.OnNodeSelectionChanged;

      this.LoadActiveTree();
    }

    private void LoadActiveTree() {
      var tree = this.ActiveTree;

      if (tree != null && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID())) {
        this.treeView.PopulateView(tree);
      }
    }

    private void OnSelectionChange() {
      this.LoadActiveTree();
    }

    private void OnNodeSelectionChanged(Node node) {
      this.inspectorView.Inspect(node);
    }
  }
}