namespace LostTech.Stack.Widgets.ProcessManagement;

using System;
using System.IO;

interface IProcess : IDisposable {
    StreamReader StandardError { get; }
    StreamReader StandardOutput { get; }
}
