using UnityEngine.UIElements;

namespace Tenry.Common.Editor.UIElements {
  public class InspectorView : VisualElement {
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    private UnityEditor.Editor editor;

    public void Inspect(UnityEngine.Object target) {
      Clear();

      if (editor != null) {
        UnityEngine.Object.DestroyImmediate(editor);
      }

      editor = UnityEditor.Editor.CreateEditor(target);
      var container = new IMGUIContainer(() => {
        if (editor?.target != null) {
          editor?.OnInspectorGUI();
        }
      });
      Add(container);
    }
  }
}
