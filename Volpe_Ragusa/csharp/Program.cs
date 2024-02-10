using System.Diagnostics;
namespace Volpe_Ragusa.csharp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {   //TO_DO dobbiamo avviare i server qui, non ci sono riuscita
            /*
            // Avvio del server Flask come processo separato
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "python"; // Assicurati che "python" sia nel PATH o specifica il percorso completo
            startInfo.Arguments = Path.Combine("..", "python", "server.py"); // Assicura che questo sia il percorso del tuo file server.py
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            using (Process process = Process.Start(startInfo))
            {
                // Leggi l'output se necessario
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);

                // Attendere che il processo termini, se necessario
                process.WaitForExit();
            }*/
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}