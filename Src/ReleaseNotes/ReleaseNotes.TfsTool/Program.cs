using System;
using System.Linq;
using System.Windows.Forms;
using ReleaseNotes.Core;

namespace ReleaseNotes.TfsTool
{

    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CommandLineOptions options = ParseCommandLine(args);

            if (options.Action == AppAction.Export)
            {
                var reportRunner = new ReportRunner();
                reportRunner.Generate(options);
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var generator = new frmReleaseNotesGenerator();
                generator.Options = options;
                generator.Show();

                Application.Run(generator);
            }
        }

        private static CommandLineOptions ParseCommandLine(string[] args)
        {
            Console.WriteLine("ReleaseNotes: Parsing Command Line...");

            var options = new CommandLineOptions();

            return options;
        }

    }
}
