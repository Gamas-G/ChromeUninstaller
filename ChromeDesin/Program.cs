using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace ChromeDesin
{
    class Program
    {
        static void Main(string[] args)
        {
            //Bandera para dejar pasar la desinstalacióin
            bool bandera = false;
            /*Rutas predeterminada en la unidad C*/
            string navegador = "Chrome"
                 , directProgram = "Program Files"
                 , directProgramx86 = "Program Files (x86)"
                 , raizPath = "C:\\{direct}\\{compania}\\GAMAS"

                 , raizdeleted = "\"C:\\{url}\\Google\"";
            Navegadores infoBrowsers;
            string json;
            List<GlobalInformation> removeBrowsers = new List<GlobalInformation>();

            string[] pathsProgram86
                   , pathsProgram;

            List<string> paths = new List<string>()
                        , deletedDirectory = new List<string>();

            //deletedDirectory.Add(@"C:\Users\elektra\AppData\Local\");
            //deletedDirectory.Add(@"C:\Users\Administrador\AppData\Local\");
            deletedDirectory.Add(@"E:\Usuarios\lgamasc\AppData\Local\Google");

            //Comando para desinstalar
            /*string command = "@echo off&" +
                             "C:&" +
                             "IF EXIST \"{raiz}\\Installer\\setup.exe\" " +
                             "(\"{raiz}\\Installer\\setup.exe\" --uninstall --multi-install --chrome --msi --system-level --force-uninstall)";*/

            string command = "@ECHO OFF&" +
                             "wmic product where name=\"Google Chrome\" call uninstall /nointeractive"; //Parece que esta mata las llaves de registros

            string deletedCommand = "@echo off$" +
                                    "C:&" +
                                    "rmdir {url} /s /q";

            Console.WriteLine(command);

            /*pathsProgram86 = Directory.GetDirectories("C:\\Program Files\\Google\\Chrome\\Application");
            foreach (string item in pathsProgram86)
            {
                Console.WriteLine(item);
            }*/

            /*Obtenemos los datos del JSON*/
            using (StreamReader r = new StreamReader(@"E:\Usuarios\lgamasc\source\repos\ChromeUninstaller\ChromeDesin\NavegadoresConfig.JSON"))
            {
                json = r.ReadToEnd();
                infoBrowsers = JsonConvert.DeserializeObject<Navegadores>(json);
            }

            /*Buscamos que navegadores tienen la bandera de 'true' para saber quienes se van a desinstalar*/
            foreach (GlobalInformation nav in infoBrowsers.navegadores)
            {
                if (!nav.activo)
                {
                    Console.WriteLine($"El siguiente navegador no entra en el proceso de desinstalación: {nav.navegador}\n");
                    removeBrowsers.Add(nav);//Los agregamos en una lista(esto lo hacemos ya que no podemos remover en tiempo del ciclo)
                }
            }

            if (removeBrowsers.Count() != 0)
            {
                //Removemos los navegadores que no procederan a la desinstalación
                foreach (GlobalInformation navRe in removeBrowsers)
                {
                    infoBrowsers.navegadores.Remove(navRe);
                }
            }




            //Procesaremos todos los navegadores(seria el ciclo donde realiza todo el trabajo)
            foreach (var item in infoBrowsers.navegadores)
            {
                raizPath = infoBrowsers.raizPath;
                bandera = false;
                Console.WriteLine($"Los navegadores a desinstalar son : {item.navegador}\n " +
                                  $"Información global : \n raizProgram : {infoBrowsers.raizProgram} \nraizProgram86: {infoBrowsers.raizProgram86}\n" +
                                  $"raizPath: {infoBrowsers.raizPath}\n raizDeleted: {infoBrowsers.raizDeleted}\n" +
                                  $"directBrowserVers: {infoBrowsers.directBrowsersVers}\n" +
                                  $"Detalle");
                foreach (DetNavegador itemDet in item.detalle)
                {
                    Console.WriteLine($"Version: {itemDet.version}\n" +
                                      $"canal: {itemDet.canal}");

                }

                //Se añade si obtiene un ruta extra(chrome genera mas carpetas y estas las obtenemos directo con el JSON)
                if (!string.IsNullOrEmpty(item.pathAppend))
                {
                    raizPath += item.pathAppend;
                    StringBuilder raiz = new StringBuilder(raizPath);
                    raiz.Replace("{direct}", directProgramx86);
                    raiz.Replace("{compania}", item.compania);
                    Console.WriteLine($"La ruta final es : \n{raiz}");
                }

                //Obtencion de las versiones(key del navegador alojada en programs normalmente)
                try
                {
                    //deletedDirectory.Add(raizdeleted.Replace("{url}", directProgramx86));
                    StringBuilder raiz = new StringBuilder(raizPath);
                    raiz.Replace("{direct}", directProgramx86);
                    raiz.Replace("{compania}", item.compania);
                    //Console.WriteLine(raiz.ToString());
                    pathsProgram86 = Directory.GetDirectories(raiz.ToString());
                }
                catch (Exception)
                {
                    pathsProgram86 = null;
                }
                try
                {
                    //deletedDirectory.Add(raizdeleted.Replace("{url}", directProgram));
                    StringBuilder raiz = new StringBuilder(raizPath);
                    raiz.Replace("{direct}", directProgram);
                    raiz.Replace("{compania}", item.compania);
                    //Console.WriteLine(raiz.ToString());
                    pathsProgram = Directory.GetDirectories(raiz.ToString());
                }
                catch (Exception)
                {

                    pathsProgram = null;
                }


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


                if (!bandera)
                {
                    Console.WriteLine("Omite toda la desinstalacion");
                    return;
                }

                Console.WriteLine("Proceso de desinstalacion");
                Console.WriteLine("Procedemos a mover el navegador estable al nuevo directorio");
                RepoAddBrowser("xcopy E:\\Usuarios\\lgamasc\\Desktop\\GIT E:\\Usuarios\\lgamasc\\Desktop\\EJEMPLO /E");

            }




            //Obtencion de las versiones
            try
            {
                deletedDirectory.Add(raizdeleted.Replace("{url}", directProgramx86));
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


            foreach (var item in deletedDirectory)
            {
                Console.WriteLine(item);
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


            if (!bandera)
            {
                Console.WriteLine("Omite toda la desinstalacion");
                return;
            }

            Console.WriteLine("Proceso de desinstalacion");
            //if (!ValidarNavegador(navegador)) return;
            //Construimos los UninstallersPaths
            foreach (var item in paths)
            {
                try
                {
                    Console.WriteLine(item);
                    Console.WriteLine(command.Replace("{raiz}", item));
                    //Procedemos a desinstalar el navegador
                    //ExecuteCommands(command.Replace("{raiz}", item));
                }
                catch (Exception)
                {
                    Console.WriteLine($"No se pudo desisntalar el navegador {item}");
                    continue;
                }
            }

            //Detener procesos de CrashHandler
            Process[] navegadores = Process.GetProcessesByName("GoogleCrashHandler");
            if (navegadores.Length != 0)
            {
                Console.WriteLine("Matando proceso GoogleCrashHandler");
                //ExecuteCommands($"taskkill /F /IM GoogleCrashHandler.exe /T");
            }
            Process[] navegadores2 = Process.GetProcessesByName("GoogleCrashHandler64");
            if (navegadores2.Length != 0)
            {
                Console.WriteLine("Matando proceso GoogleCrashHandle64");
                //ExecuteCommands($"taskkill /F /IM GoogleCrashHandler64.exe /T");
            }
            Console.WriteLine("Eliminación de archivos reciduales\n");
            //LimpiarArchivos(deletedDirectory, deletedCommand);



            Console.ReadLine();
        }

        //[ObsoleteAttrubute()]
        private static void LimpiarArchivos(List<string> paths, string deletedCommand)
        {
            Console.WriteLine("Funcion Limpiar, para administrador");

            DirectoryInfo direcx86 = new DirectoryInfo(@"C:\Program Files (x86)\");
            DirectoryInfo[] filex86 = direcx86.GetDirectories();
            foreach (var item in filex86)
            {
                if (item.Name == "Google")
                {
                    //Console.WriteLine("eliminand program x86");
                    item.Delete(true);
                    break;
                }
            }
            DirectoryInfo direc = new DirectoryInfo(@"C:\Program Files\");
            DirectoryInfo[] file = direc.GetDirectories();
            foreach (var item in file)
            {
                if (item.Name == "Google")
                {
                    //Console.WriteLine("eliminando program");
                    item.Delete(true);
                    break;
                }
            }
            Console.WriteLine(@"C:\Users\elektra\AppData\Local\");
            //DirectoryInfo direcAppData = new DirectoryInfo(@"C:\Users\elektra\AppData\Local\");
            DirectoryInfo direcAppData = new DirectoryInfo(@"E:\Usuarios\lgamasc\AppData\Local\");
            DirectoryInfo[] fileApp = direcAppData.GetDirectories();
            foreach (var item in fileApp)
            {
                if (item.Name == "Google")
                {
                    //Console.WriteLine("eliminando appdata local");
                    item.Delete(true);
                    break;
                }
            }

            /*DirectoryInfo direcAppDataAdmi = new DirectoryInfo(@"C:\Users\Administrador\AppData\Local\");
            DirectoryInfo[] fileAppAdmi = direcAppDataAdmi.GetDirectories();
            foreach (var item in fileAppAdmi)
            {
                if (item.Name == "Google")
                {
                    Console.WriteLine("eliminando appdata administradpor");
                    //item.Delete(true);
                    break;
                }
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
            try
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
            }
            catch (Exception msj)
            {
                Console.WriteLine($"Ocurrio un error al ejecutar el comando \n{msj.Message}");
                throw;
            }
        }//Ejecución de comandos, por medio de terminal
        
        private static void RepoAddBrowser(string command)
        {
            ExecuteCommands(command);
        }
    }
    public class Navegadores
    {
        //[JsonProperty(PropertyName="raizProgram")]
        public string raizProgram { get; set; }
        public string raizProgram86 { get; set; }
        public string raizPath { get; set; }
        public string raizDeleted { get; set; }
        public string directBrowsersVers { get; set; }
        public List<GlobalInformation> navegadores { get; set; }
    }
    public class GlobalInformation
    {
        public string navegador { get; set; }
        public string compania { get; set; }
        public bool activo { get; set; }
        public List<DetNavegador> detalle { get; set; }
        [JsonProperty(PropertyName = "raizPathAppend")]
        public string pathAppend { get; set; }
    }
    public class DetNavegador
    {
        public int version { get; set; }
        public string canal { get; set; }
    }
}
