using UnityEngine.UIElements;

namespace Tenry.Common.Editor.UIElements {
  public class SplitView : TwoPaneSplitView {
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> {}
  }
}
