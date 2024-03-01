
using System;

namespace Snowy
{
  [AttributeUsage(AttributeTargets.Class)]
  public class BehaviourNodeAttribute : Attribute
  {
    public readonly string menuPath, texturePath;

    public BehaviourNodeAttribute(string menuPath, string texturePath = null)
    {
      this.menuPath = menuPath;
      this.texturePath = texturePath;
    }
  }
}