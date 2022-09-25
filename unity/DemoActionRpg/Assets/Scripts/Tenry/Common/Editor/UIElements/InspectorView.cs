using UnityEngine.UIElements;

namespace Tenry.Common.Editor.UIElements {
  public class InspectorView : VisualElement {
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}

    private UnityEditor.Editor editor;

    public void Inspect(UnityEngine.Object target) {
      this.Clear();

      if (this.editor != null) {
        UnityEngine.Object.DestroyImmediate(this.editor);
      }

      this.editor = UnityEditor.Editor.CreateEditor(target);
      var container = new IMGUIContainer(() => {
        if (this.editor?.target != null) {
          this.editor?.OnInspectorGUI();
        }
      });
      this.Add(container);
    }
  }
}
