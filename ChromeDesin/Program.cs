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
            /*Rutas predeterminada en la unidad C*/
            string pathProgramx86 = @"C:\Program Files (x86)\Google\Chrome\Application"
                  ,pathProgram    = @"C:\Program Files\Google\Chrome\Application"
                  ,nombre = "Saday"
                  ,saludo = "No se";
            
            string[] pathsProgram86
                    ,pathsProgram;
            
            List<string> pathx86 = new List<string>()
                        ,paths = new List<string>();

            string archivoBat = "@echo off\n" +
                                "echo hola {nombre} espero que estes {saludo}";

            string archivoBatFormat = "@echo off\n" +
                                "echo hola {0} espero que estes {1}\n" +
                                "dir";
            //Console.WriteLine("Esto es un comando con c#");
            //DriveInfo dirve = new DriveInfo(@"C:\");
            ////var unidades = DriveInfo.GetDrives();
            //DirectoryInfo dirInfo = dirve.RootDirectory;

            //ProcessStartInfo info = new ProcessStartInfo("cmd", "/c cd.. dir")
            //{
            //    RedirectStandardOutput = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = false
            //};

            //Process proceso = new Process()
            //{
            //    StartInfo = info,

            //};
            //proceso.Start();
            //string resultado = proceso.StandardOutput.ReadToEnd();
            //Console.WriteLine(resultado);

            var resultado = archivoBat.Replace("{nombre}", nombre)
                                      .Replace("{saludo}", saludo);

            var resultadoFormat = string.Format(archivoBatFormat,nombre,saludo);

            Console.WriteLine(resultado);
            Console.WriteLine(resultadoFormat);


            try
            {
                pathsProgram86 = Directory.GetDirectories(pathProgramx86);

            }
            catch (Exception)
            {
                pathsProgram86 = null;
            }
            try
            {

                pathsProgram = Directory.GetDirectories(pathProgram);
            }
            catch (Exception)
            {

                pathsProgram = null;
            }

            if (pathsProgram != null)
            {
                foreach (var folder in pathsProgram)
                {
                    if (folder.Contains("104"))
                    {
                        paths.Add(folder);
                        Console.WriteLine("Esto esta en Programe Files " + folder);
                        Console.WriteLine($"Esto esta {folder} en el archivo Programe Files");
                    }
                }
            }

            if (pathsProgram86 != null)
            {
                foreach (var folder in pathsProgram86) 
                {
                    if (folder.Contains("104"))
                    {
                        paths.Add(folder);
                        Console.WriteLine(" Esto esta en Programe Files (x86)" + folder);
                    }
                }
            }

            Console.WriteLine($"Cantidad total : { paths.Count() }");
            foreach (var item in paths)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }

    class BatParametres
    {
        string nombre
              ,saludo;
    }
}
