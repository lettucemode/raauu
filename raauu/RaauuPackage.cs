global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;

namespace raauu
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.raauuString)]
    // Despite claims of being "very well documented", the only place I could find valid values for this feature
    // was this issue https://github.com/MicrosoftDocs/visualstudio-docs/issues/2929 which was closed wontfix.
    // Fortunately it seems to work preety gud.
    [ProvideUIContextRule(PackageGuids.AutoLoadString,
        name: "autoload",
        expression: "cs",
        termNames: new[] { "cs" },
        termValues: new[] { @"ActiveEditorContentType:CSharp" })]
    public sealed class RaauuPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();
        }
    }
}