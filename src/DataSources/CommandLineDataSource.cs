namespace LostTech.Stack.Widgets.DataSources;

using System;
using System.Threading.Tasks;

/// <summary>
/// Executes specified command and returns the generated text
/// </summary>
public class CommandLineDataSource : AsyncDataSourceBase<string> {
    public CommandLineValueSource CommandLineValueSource { get; set; } = new();

    public CommandLineDataSource() {
        this.CommandLineValueSource.PropertyChanged += delegate { this.DoAutoRefresh(); };
    }

    protected override async Task<string> RefreshValueAsync() {
        if (string.IsNullOrEmpty(this.CommandLineValueSource?.Program))
            throw new InvalidOperationException("Value source has not been configured");

        using var process = this.CommandLineValueSource.Start();
        var stream = this.CommandLineValueSource.ValueFromStdErr
            ? process.StandardError
            : process.StandardOutput;
        return await stream.ReadToEndAsync().ConfigureAwait(false);
    }
}
