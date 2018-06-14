﻿namespace LostTech.Stack.Widgets
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Navigation;
    using Prism.Commands;

    public static class Commands
    {
        public static ICommand LaunchUrl { get; } = new DelegateCommand<object>(o => {
            if (!(o is Uri uri) &&
                !(o is string str && Uri.TryCreate(str, UriKind.Absolute, out uri)))
                return;

            if (uri.Scheme == null)
                return;

            if (uri.Scheme.ToLowerInvariant().Any(c => c < 'a' || c > 'z'))
                return;

            Process.Start(uri.ToString());
        });
    }
}
