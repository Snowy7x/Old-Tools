
namespace Snowy.Designer
{
  public interface IUndoableAction
  {
    void Undo();
    void Redo();
  }
}
