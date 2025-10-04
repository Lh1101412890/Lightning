using System;
using System.Collections.Generic;

namespace Lightning.Extension
{
    public static class IDisposableExtension
    {
        public static void DisposeAll(this IEnumerable<IDisposable> disposables)
        {
            foreach (IDisposable disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}