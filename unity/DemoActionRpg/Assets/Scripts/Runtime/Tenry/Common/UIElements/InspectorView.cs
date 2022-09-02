using UnityEditor;
using UnityEngine.UIElements;

namespace Tenry.Common.UIElements {
  public class InspectorView : VisualElement {
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}

    private Editor editor;

    public void Inspect(UnityEngine.Object target) {
      this.Clear();

      if (this.editor != null) {
        UnityEngine.Object.DestroyImmediate(this.editor);
      }

      this.editor = Editor.CreateEditor(target);
      var container = new IMGUIContainer(() => { this.editor?.OnInspectorGUI(); });
      this.Add(container);
    }
  }
}
