using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

using Tenry.Common.UIElements;

namespace Tenry.Common.BehaviorTree {
  public class BehaviorTreeEditor : EditorWindow {
    private BehaviorTreeView treeView;

    private InspectorView inspectorView;

    public static BehaviorTree ActiveTree => Selection.activeObject as BehaviorTree;

    public static BehaviorTreeController ActiveController => Selection.activeGameObject?.GetComponent<BehaviorTreeController>();

    [MenuItem("Window/Tenry/Behavior Tree Editor")]
    public static void ShowWindow() {
      BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
      wnd.titleContent = new GUIContent("Behavior Tree Editor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instandeId, int line) {
      if (Selection.activeObject is BehaviorTree) {
        ShowWindow();
        return true;
      }

      return false;
    }

    private void OnEnable() {
      EditorApplication.playModeStateChanged -= this.OnPlayModeStateChanged;
      EditorApplication.playModeStateChanged += this.OnPlayModeStateChanged;
    }

    private void OnDisable() {
      EditorApplication.playModeStateChanged -= this.OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange change) {
      switch (change) {
        case PlayModeStateChange.EnteredEditMode:
        case PlayModeStateChange.EnteredPlayMode:
          this.LoadActiveTree();
          break;
      }
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
      var tree = ActiveTree;

      if (tree == null) {
        var controller = ActiveController;
        if (controller != null) {
          tree = controller.Tree;
        }
      }

      if (tree != null) {
        if (Application.isPlaying || AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID())) {
          this.treeView?.PopulateView(tree);
        }
      }
    }

    private void OnSelectionChange() {
      this.LoadActiveTree();
    }

    private void OnNodeSelectionChanged(Node node) {
      this.inspectorView.Inspect(node);
    }

    private void OnInspectorUpdate() {
      this.treeView?.UpdateNodeStates();
    }
  }
}