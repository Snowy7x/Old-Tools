
using System.Collections.Generic;
using Snowy.Core.Base;

namespace Snowy.Designer
{
  /// <summary>
  /// View of the selection.
  /// </summary>
  public interface IReadOnlySelection
  {
    IReadOnlyList<BonsaiNode> SelectedNodes { get; }
    BonsaiNode SingleSelectedNode { get; }
    IReadOnlyList<BehaviourNode> Referenced { get; }

    bool IsNodeSelected(BonsaiNode node);
    bool IsReferenced(BonsaiNode node);

    int SelectedCount { get; }
    bool IsEmpty { get; }
    bool IsSingleSelection { get; }
    bool IsMultiSelection { get; }
  }
}
