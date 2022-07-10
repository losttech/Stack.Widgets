namespace LostTech.Stack.Widgets.DataSources;

using System;
using System.Collections.Generic;
using System.Windows.Markup;

[ContentProperty(nameof(CommandLineValueSource))]
public class CommandLineOutputSource {
    public CommandLineValueSource CommandLineValueSource { get; set; } = new();

    public IAsyncEnumerable<string> ValueStream => this.GetValueStream();

    async IAsyncEnumerable<string> GetValueStream() {
        if (this.CommandLineValueSource is not { } commandLine)
            throw new InvalidOperationException("Value source has not been configured");

        var process = commandLine.Start();
        var stream = commandLine.ValueFromStdErr
            ? process.StandardError
            : process.StandardOutput;

        while (true) {
            string? line = await stream.ReadLineAsync();
            if (line is null) yield break;
            yield return line;
        }
    }
}
