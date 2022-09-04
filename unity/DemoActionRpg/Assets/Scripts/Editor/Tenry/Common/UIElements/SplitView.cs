using UnityEngine.UIElements;

namespace Tenry.Common.UIElements {
  public class SplitView : TwoPaneSplitView {
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> {}
  }
}
