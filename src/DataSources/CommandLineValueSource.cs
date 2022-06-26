namespace LostTech.Stack.Widgets.DataSources;

using System.Collections.Generic;
using System.Diagnostics;

using LostTech.Stack.Widgets.DataBinding;

public class CommandLineValueSource: DependencyObjectNotifyBase {
    readonly ProcessStartInfo startInfo = new() {
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
    };

    /// <summary>
    /// Program to execute. See <see cref="ProcessStartInfo.FileName"/>
    /// </summary>
    public string Program {
        get => this.startInfo.FileName;
        set {
            if (value == this.Program)
                return;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Command line arguments for the <see cref="Program"/>.
    /// See <see cref="ProcessStartInfo.ArgumentList"/>
    /// </summary>
    public IList<string> Arguments => this.startInfo.ArgumentList;

    /// <summary>
    /// Working directory for the command line process.
    /// </summary>
    public string WorkingDirectory {
        get => this.startInfo.WorkingDirectory;
        set {
            if (value == this.WorkingDirectory)
                return;
            this.startInfo.WorkingDirectory = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// If set to <c>true</c>, the <see cref="Value"/> is read from stderr instead of stdout.
    /// </summary>
    public bool ValueFromStdErr {
        get => this.startInfo.RedirectStandardError;
        set {
            if (value == this.ValueFromStdErr)
                return;

            this.startInfo.RedirectStandardError = value;
            this.startInfo.RedirectStandardOutput = !value;
            this.OnPropertyChanged();
        }
    }

    public Process Start() => Process.Start(this.startInfo)!;
}
