using EnvDTE;
using Microsoft;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace raauu
{
    [Command(PackageIds.raauuCommand)]
    internal sealed class RaauuCommand : BaseCommand<RaauuCommand>
    {
        private Random random;

        protected override Task InitializeCompletedAsync()
        {
            this.random = new Random();
            return base.InitializeCompletedAsync();
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // The old strat was to cache all assemblies in the InitializeCompletedAsync()
            // override above, since that's what the original version of this extension did,
            // but in my present-day testing doing ass.DefinedTypes in a for loop takes
            // over 8 seconds to complete the first run on my dev machine.
            // Maybe there's a way to warm the cache when the extension starts up on a non-UI thread.
            // For now, just grab 3 random ones per-run.
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] selected =
            { 
               assemblies[this.random.Next(assemblies.Length)], 
               assemblies[this.random.Next(assemblies.Length)],
               assemblies[this.random.Next(assemblies.Length)],
            };
            var types = new List<TypeInfo>();
            foreach (var type in selected)
            {
                try
                {
                    types.AddRange(selected.SelectMany(ass => ass.DefinedTypes));
                }
                catch (ReflectionTypeLoadException)
                {
                    // This occasionally happens. Just swallow it, doesn't matter
                }
            }
            var namespaces = types.Select(t => t.Namespace)
                                  .Distinct()
                                  .Where(name => name != null && !name.StartsWith("_"))
                                  .ToList();

            // add some random namespaces from the above to a list
            List<string> usingList = new(5);
            int num = this.random.Next(4) + 2;
            for (int index = 0; index < num; ++index)
            {
                usingList.Add(namespaces[this.random.Next(namespaces.Count)]);
            }

            // init dte obj
            var dte = await Package.GetServiceAsync(typeof(DTE)) as DTE;
            Assumes.Present(dte);

            // find the first using statement in the file
            TextSelection selection = (dte.ActiveDocument.Object("") as TextDocument).Selection;
            selection.StartOfDocument(false);
            var theStartingLine = selection.CurrentLine; // say it like you mean it
            selection.SelectLine();
            while (!selection.Text.Contains("using"))
            {
                theStartingLine++;
                selection.GotoLine(theStartingLine);
                selection.SelectLine();
            }

            // assume we're at the start of a block of using statements
            // remove all using statements and add them to the list
            dte.UndoContext.Open("raauu");
            while (selection.Text.Contains("using"))
            {
                usingList.Add(selection.Text);
                selection.Delete(1);
                selection.SelectLine();
            }

            // scramble list
            for (int index1 = usingList.Count - 1; index1 > 0; --index1)
            {
                int index2 = this.random.Next(index1 + 1);
                (usingList[index2], usingList[index1]) = (usingList[index1], usingList[index2]);
            }

            // write list contents back into file
            selection.GotoLine(theStartingLine);
            foreach (string str in usingList)
            {
                if (!str.Contains("using")) selection.Insert("using " + str + ";\r\n", 1);
                else                        selection.Insert(str, 1);
            }
            dte.UndoContext.Close();
        }
    }
}
