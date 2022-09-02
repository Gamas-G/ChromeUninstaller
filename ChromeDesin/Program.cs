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
            bool bandera = false;
            /*Rutas predeterminada en la unidad C*/
            string navegador = "Chrome"
                 , directProgram = "Program Files"
                 , directProgramx86 = "Program Files (x86)"
                 , raizPath = "C:\\{direct}\\Google\\Chrome\\Application"
                 , raizdeleted = "\"C:\\{url}\\Google\"";

                  //, pathProgram      = @"C:\Program Files\Google\Chrome\Application"

            string[] pathsProgram86
                    ,pathsProgram;
            
            List<string> paths = new List<string>()
                        ,deletedDirectory = new List<string>();

            //Comando para desinstalar
            /*string command = "@echo off&" +
                             "C:&" +
                             "IF EXIST \"{raiz}\\Installer\\setup.exe\" " +
                             "(\"{raiz}\\Installer\\setup.exe\" --uninstall --multi-install --chrome --msi --system-level --force-uninstall)";*/

            string command = "@ECHO OFF&" +
                             "wmic product where name=\"Google Chrome\" call uninstall /nointeractive";

            string deletedCommand = "@echo off$" +
                                    "C:&" +
                                    "rmdir {url} /s /q";

            //Obtencion de las versiones
            try
            {
                //deletedDirectory.Add(raizdeleted.Replace("{url}",directProgramx86));
                pathsProgram86 = Directory.GetDirectories(raizPath.Replace("{direct}", directProgramx86));
            }
            catch (Exception)
            {
                pathsProgram86 = null;
            }
            try
            {
                //deletedDirectory.Add(raizdeleted.Replace("{url}", directProgram));
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
                    if (folder.Contains("87") || folder.Contains("105") || folder.Contains("104"))
                    {
                        paths.Add(folder);
                        bandera = true;
                    }
                }
            }

            if (pathsProgram86 != null)
            {
                foreach (var folder in pathsProgram86) 
                {
                    if (folder.Contains("87") || folder.Contains("105") || folder.Contains("104"))
                    { 
                        paths.Add(folder);
                        bandera = true;
                    }
                }
            }

            if (bandera)
            {
                Console.WriteLine("Proceso de desinstalacion");
                if (!ValidarNavegador(navegador)) return;
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

                //Detener procesos de CrashHandler
                //Process[] navegadores = Process.GetProcessesByName("GoogleCrashHandler");
                //if (navegadores.Length != 0)
                //{
                ExecuteCommands($"taskkill /F /IM GoogleCrashHandler.exe /T");
                //}
                //Process[] navegadores2 = Process.GetProcessesByName("GoogleCrashHandler64");
                //if (navegadores2.Length != 0)
                //{
                ExecuteCommands($"taskkill /F /IM GoogleCrashHandler64.exe /T");
                //}
                Console.WriteLine("Eliminación de archivos reciduales\n");
                LimpiarArchivos();
            }
            else
                Console.WriteLine("Omite toda la desinstalacion");

            Console.ReadLine();
        }

        private static void LimpiarArchivos()
        {
            Console.WriteLine("Funcion Limpiar, para administrador too");

            //Directory.Delete("\"C:\\Program Files\\Google\"",true);
            //Directory.Delete("C:\\\"Program Files (x86)\"\\Google",true);
            //Directory.Delete(@"C:\Users\lgamasc\AppData\Local\Google", true); //c:\users\elektra\AppData\Local\Google

            DirectoryInfo direcx86 = new DirectoryInfo(@"C:\Program Files (x86)\");
            DirectoryInfo[] filex86 = direcx86.GetDirectories();
            foreach (var item in filex86)
            {
                if (item.Name == "Google") item.Delete(true);
            }
            DirectoryInfo direc = new DirectoryInfo(@"C:\Program Files\");
            DirectoryInfo[] file = direc.GetDirectories();
            foreach (var item in file)
            {
                if (item.Name == "Google") item.Delete(true);
            }
            Console.WriteLine(@"C:\Users\elektra\AppData\Local\");
            //DirectoryInfo direcAppData = new DirectoryInfo(@"C:\Users\elektra\AppData\Local\");
            DirectoryInfo direcAppData = new DirectoryInfo(@"E:\Usuarios\lgamasc\AppData\Local\");
            DirectoryInfo[] fileApp = direcAppData.GetDirectories();
            foreach (var item in fileApp)
            {
                if (item.Name == "Google") item.Delete(true);
            }

            /*DirectoryInfo direcAppDataAdmi = new DirectoryInfo(@"C:\Users\Administrador\AppData\Local\");
            DirectoryInfo[] fileAppAdmi = direcAppDataAdmi.GetDirectories();
            foreach (var item in fileAppAdmi)
            {
                if (item.Name == "Google") item.Delete(true);
            }*/
        
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
            info.CreateNoWindow = false;
            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = false;
            mio.StartInfo = info;
            mio.Start();
            mio.WaitForExit();
        }//Ejecución de comandos, por medio de terminal
    }
}
