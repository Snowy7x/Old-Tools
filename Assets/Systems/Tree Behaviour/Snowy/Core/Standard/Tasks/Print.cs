﻿using Snowy.Core.Base;
using System.Text;
using UnityEngine;

namespace Snowy.Core.Standard
{
  /// <summary>
  /// Displays a message.
  /// </summary>
  [BehaviourNode("Tasks/", "Log")]
  public class Print : Task
  {
    public enum LogType { Normal, Warning, Error };

    [Multiline]
    public string message = "Print Node";

    [Tooltip("The type of message to display.")]
    public LogType logType = LogType.Normal;

    public override Status Run()
    {
      switch (logType)
      {
        case LogType.Normal:
          Debug.Log(message);
          break;

        case LogType.Warning:
          Debug.LogWarning(message);
          break;

        case LogType.Error:
          Debug.LogError(message);
          break;
      }

      return Status.Success;
    }

    public override void Description(StringBuilder builder)
    {
      // Nothing to display.
      if (message.Length == 0)
      {
        return;
      }

      string displayed = message;

      // Only consider display the message up to the newline.
      int newLineIndex = message.IndexOf('\n');
      if (newLineIndex >= 0)
      {
        displayed = message.Substring(0, newLineIndex);
      }

      // Nothing to display.
      if (displayed.Length == 0)
      {
        return;
      }

      if (logType != LogType.Normal)
      {
        builder.AppendLine(logType.ToString());
      }

      // Cap the message length to display to keep things compact.
      int maxCharacters = 20;
      if (displayed.Length > maxCharacters)
      {
        builder.Append(displayed.Substring(0, maxCharacters));
        builder.Append("...");
      }
      else
      {
        builder.Append(displayed);
      }
    }

  }
}