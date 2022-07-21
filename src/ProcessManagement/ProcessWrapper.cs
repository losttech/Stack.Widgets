namespace LostTech.Stack.Widgets.ProcessManagement;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using static PInvoke.Kernel32;

class ProcessWrapper : IProcess {
    readonly Job? job;
    public Process Process { get; }

    public StreamReader StandardError => this.Process.StandardError;
    public StreamReader StandardOutput => this.Process.StandardOutput;

    public ProcessWrapper(Process process, bool slave) {
        this.Process = process ?? throw new ArgumentNullException(nameof(process));
        if (slave) {
            this.job = new Job();
            var processHandle = new SafeObjectHandle(process.Handle, ownsHandle: false);
            if (!this.job.AddProcess(processHandle))
                throw new Win32Exception();
        }
    }

    public void Dispose() {
        this.Process.Dispose();
        this.job?.Dispose();
    }
}
