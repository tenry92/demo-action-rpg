using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

using Tenry.Common.Editor.UIElements;

namespace Tenry.BehaviorTree.Editor {
  public class BehaviorTreeEditor : EditorWindow {
    #region Serialized Fields
    [SerializeField]
    private VisualTreeAsset document;
    #endregion

    private BehaviorTreeView treeView;

    private InspectorView inspectorView;

    public static Runtime.BehaviorTree ActiveTree => Selection.activeObject as Runtime.BehaviorTree;

    public static Runtime.BehaviorTreeController ActiveController => Selection.activeGameObject?.GetComponent<Runtime.BehaviorTreeController>();

    [MenuItem("Window/Tenry/Behavior Tree Editor")]
    public static void ShowWindow() {
      BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
      wnd.titleContent = new GUIContent("Behavior Tree Editor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instandeId, int line) {
      if (Selection.activeObject is Runtime.BehaviorTree) {
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

      // Import UXML
      var visualTree = this.document;
      visualTree.CloneTree(root);

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

    private void OnNodeSelectionChanged(Runtime.Node node) {
      this.inspectorView.Inspect(node);
    }

    private void OnInspectorUpdate() {
      this.treeView?.UpdateNodeStates();
    }
  }
}