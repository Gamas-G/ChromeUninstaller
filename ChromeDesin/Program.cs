using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChromeDesin
{
    class Program
    {

        static void Main(string[] args)
        {
            int construccion = 0; //0 = Chrome por default
            /*Rutas predeterminada en la unidad C*/
            string navegador = "Brave"
                 , directProgram    = "Program Files"
                 , directProgramx86 = "Program Files (x86)"
                 , raizPath         = "C:\\{direct}\\Google\\Chrome\\Application"
                 , version          = "104.0.5112.102";

                  //, pathProgram      = @"C:\Program Files\Google\Chrome\Application"

            string[] pathsProgram86
                    ,pathsProgram;
            
            List<string> pathx86 = new List<string>()
                        ,paths = new List<string>();

            string command = "@echo off&" +
                             "C:&" +
                             "{direct}\\{version}\\Installer\\setup.exe --uninstall --multi-install --chrome --msi --system-level --force-uninstall";

            //using (StreamWriter sw = mio.StandardInput)
            //{
            //    sw.WriteLine("cd..");
            //    sw.WriteLine("cd..");
            //    sw.WriteLine("dir");
            //}

            if (!ValidarNavegador(navegador)) return;
            
            var resultado = command.Replace("{direct}", raizPath)
                                   .Replace("{version}", version);

            //var result2 = resultado.Replace("{direct}",directProgram);

            //var resultadoFormat = string.Format(archivoBatFormat,nombre,saludo);

            //Console.WriteLine(result2);

            //Console.ReadLine();

            //Console.WriteLine(resultadoFormat);
            //Console.ReadLine();


            try
            {
                pathsProgram86 = Directory.GetDirectories(raizPath.Replace("{direct}", directProgramx86));

            }
            catch (Exception)
            {
                pathsProgram86 = null;
            }
            try
            {
                pathsProgram = Directory.GetDirectories(raizPath.Replace("{direct}", directProgram));
            }
            catch (Exception)
            {

                pathsProgram = null;
            }

            if (pathsProgram != null)
            {
                foreach (var folder in pathsProgram)
                {
                    if (folder.Contains("91") || folder.Contains("104"))
                        paths.Add(folder);
                }
            }

            if (pathsProgram86 != null)
            {
                foreach (var folder in pathsProgram86) 
                {
                    if (folder.Contains("91") || folder.Contains("104"))
                        paths.Add(folder);
                }
            }

            Console.WriteLine($"Cantidad total : { paths.Count() }");
            foreach (var item in paths)
            {
                Console.WriteLine(item);
            }

            //Process mio = new Process();
            //ProcessStartInfo info = new ProcessStartInfo();

            //info.FileName = "cmd.exe";
            //info.Arguments = "/C " + result2;
            //info.UseShellExecute = false;
            //info.RedirectStandardInput = false;
            //info.RedirectStandardOutput = false;
            //mio.StartInfo = info;
            //mio.Start();

            Console.ReadLine();
        }

        private static bool ValidarNavegador(string navegador)
        {
            try
            {

                Process[] navegadores = Process.GetProcessesByName(navegador);
                if (navegadores.Length != 0)
                {
                    Console.WriteLine("El navegador Brave esta abierto..Procediendo a cerrar");
                    ExecuteCommands($"taskkill /F /IM {navegador}.exe /T");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void ExecuteCommands(string command)
        {
            Process mio = new Process();
            ProcessStartInfo info = new ProcessStartInfo();

            info.FileName = "cmd.exe";
            info.Arguments = $"/C {command}";
            info.UseShellExecute = false;
            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = false;
            mio.StartInfo = info;
            mio.Start();
        }
    }
}
