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
            string navegador        = "Chrome"
                 , directProgram    = "Program Files"
                 , directProgramx86 = "Program Files (x86)"
                 , raizPath         = "C:\\{direct}\\Google\\Chrome\\Application"
                 , raizdeleted      = "\"C:\\{url}\\Google\"";

                  //, pathProgram      = @"C:\Program Files\Google\Chrome\Application"

            string[] pathsProgram86
                    ,pathsProgram;
            
            List<string> paths = new List<string>()
                        ,deletedDirectory = new List<string>();

            string command = "@echo off&" +
                             "C:&" +
                             "IF EXIST \"{raiz}\\Installer\\setup.exe\" " +
                             "(\"{raiz}\\Installer\\setup.exe\" --uninstall --multi-install --chrome --msi --system-level --force-uninstall)";

            string deletedCommand = "@echo off$" +
                                    "C:&" +
                                    "rmdir {url} /s /q";

            Process.Start("C:\\Windows\\system32\\cmd.exe");
            //ExecuteCommands("dir");
            Console.ReadLine();
            
            if (!ValidarNavegador(navegador)) return;

            //Obtencion de las versiones
            try
            {
                deletedDirectory.Add(raizdeleted.Replace("{url}",directProgramx86));
                pathsProgram86 = Directory.GetDirectories(raizPath.Replace("{direct}", directProgramx86));
            }
            catch (Exception)
            {
                pathsProgram86 = null;
            }
            try
            {
                deletedDirectory.Add(raizdeleted.Replace("{url}", directProgram));
                pathsProgram = Directory.GetDirectories(raizPath.Replace("{direct}", directProgram));
            }
            catch (Exception)
            {

                pathsProgram = null;
            }


            //Validando si contiene datos y si es la version que se decea desinstalar
            if (pathsProgram != null)
            {
                foreach (var folder in pathsProgram)
                {
                    if (folder.Contains("87") || folder.Contains("105"))
                        paths.Add(folder);
                }
            }

            if (pathsProgram86 != null)
            {
                foreach (var folder in pathsProgram86) 
                {
                    if (folder.Contains("87") || folder.Contains("105"))
                        paths.Add(folder);
                }
            }


            Console.WriteLine($"Cantidad total : { paths.Count() }");
            //Construimos los UninstallersPaths
            foreach (var item in paths)
            {
                try
                {
                    Console.WriteLine(item);
                    //Console.WriteLine(command.Replace("{raiz}", item));
                    //Procedemos a desinstalar el navegador
                    ExecuteCommands(command.Replace("{raiz}", item));
                }
                catch (Exception)
                {
                    Console.WriteLine($"No se pudo desisntalar el navegador {item}");
                    continue;
                }
            }

            //Eliminando carpeta
            foreach (var folder in deletedDirectory)
            {
                try
                {
                    Console.WriteLine(deletedCommand.Replace("{url}",folder));
                    ExecuteCommands(raizdeleted.Replace("{url}", folder));
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error al eliminar el directorio {folder}");
                    continue;
                }
            }

            Console.ReadLine();
        }

        private static bool ValidarNavegador(string navegador)
        {
            try
            {

                Process[] navegadores = Process.GetProcessesByName(navegador);
                if (navegadores.Length != 0)
                {
                    Console.WriteLine($"El navegador {navegador} esta abierto..Procediendo a cerrar");
                    ExecuteCommands($"taskkill /F /IM {navegador}.exe /T");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//matamos los procesos del navegador si estan abiertos

        private static void ExecuteCommands(string command)
        {
            //Ejecución de comandos
            Process mio = new Process();
            ProcessStartInfo info = new ProcessStartInfo();

            info.FileName = "cmd.exe";
            info.Arguments = $"/C {command}";
            info.UseShellExecute = false;
            info.RedirectStandardInput = false;
            info.CreateNoWindow = false;
            info.RedirectStandardOutput = false;
            mio.StartInfo = info;
            mio.Start();
        }//Ejecución de comandos, por medio de terminal
    }
}
