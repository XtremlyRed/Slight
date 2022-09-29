using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
[assembly: XmlnsDefinition("http://slight-library.org/", "Slight.Core")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

namespace Slight.Core
{


    /// <summary>
    /// pack://Application:,,,/Slight.Core;component/Themes/DefaultStyle.xaml
    /// </summary>
    [DisplayName("pack://Application:,,,/Slight.Core.Wpf;component/Themes/DefaultStyle.xaml")]
    public static class Initialize
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static bool _initialized = false;

        public static void Init(Application application = null)
        {
            application ??= Application.Current;

            if (application is null)
            {
                return;
            }

            lock (application)
            {
                if (_initialized)
                {
                    return;
                }
                _initialized = true;
            }

            string path = $"pack://Application:,,,/Slight.Core.Wpf;component/Themes/DefaultStyle.xaml";

            ResourceDictionary resource = new()
            {
                Source = new Uri(path)
            };

            application.Resources?.MergedDictionaries?.Add(resource);
        }
    }
}
