namespace provodnic
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Explorer
    {
        private static string currentPath = "";

        public static void Run()
        {
            LoadDrives();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                PrintMenu();

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        MoveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        MoveDown();
                        break;
                    case ConsoleKey.Enter:
                        if (IsDirectory())
                        {
                            MoveToDirectory();
                        }
                        else
                        {
                            OpenFile();
                        }
                        break;
                    case ConsoleKey.Escape:
                        if (currentPath != "")
                        {
                            MoveToParentDirectory();
                        }
                        else
                        {
                            exit = true;
                        }
                        break;
                }
            }
        }


        private static void PrintMenu()
        {
            Console.WriteLine("Меню:)");
            Console.WriteLine("Используйте стрелки для навигации");
            Console.WriteLine("Нажмите Enter для выбора");
            Console.WriteLine("Нажмите Escape для возвращения назад");

            if (currentPath == "")
            {
                Console.WriteLine("Диски:");
            }
            else
            {
                Console.WriteLine("Путь: " + currentPath);
                Console.WriteLine("Папки и файлы:");

                DirectoryInfo directory = new DirectoryInfo(currentPath);
                List<string> directories = new List<string>();
                List<string> files = new List<string>();

                foreach (var dir in directory.GetDirectories())
                {
                    directories.Add("<DIR> " + dir.Name);
                }

                foreach (var file in directory.GetFiles())
                {
                    files.Add(file.Name);
                }

                directories.Sort();
                files.Sort();

                foreach (var dir in directories)
                {
                    Console.WriteLine(dir);
                }

                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
            }
        }

        private static void LoadDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                Console.WriteLine(drive.Name);
            }
        }

        private static void MoveUp()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        private static void MoveDown()
        {
            Console.SetCursorPosition(0, Console.CursorTop + 1);
        }

        private static bool IsDirectory()
        {
            string toCheck = "";

            if (currentPath == "")
            {
                toCheck = new DriveInfo(Path.GetPathRoot(Environment.CurrentDirectory)).Name;
            }
            else
            {
                toCheck = Path.Combine(currentPath, Console.CursorTop >= 7 ? Console.CursorTop - 7 < currentPath.Split(Path.DirectorySeparatorChar).Length ? currentPath.Split(Path.DirectorySeparatorChar)[Console.CursorTop - 7] : "" : "");
            }

            if (Directory.Exists(toCheck))
            {
                return true;
            }

            return false;
        }

        private static void MoveToDirectory()
        {
            currentPath = Path.Combine(currentPath, Console.CursorTop >= 7 ? Console.CursorTop - 7 < currentPath.Split(Path.DirectorySeparatorChar).Length ? currentPath.Split(Path.DirectorySeparatorChar)[Console.CursorTop - 7] : "" : "");
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
        }

        private static void MoveToParentDirectory()
        {
            currentPath = Directory.GetParent(currentPath).FullName;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
        }

        private static void OpenFile()
        {
            string filePath = Path.Combine(currentPath, Console.CursorTop >= 7 ? Console.CursorTop - 7 < currentPath.Split(Path.DirectorySeparatorChar).Length ? currentPath.Split(Path.DirectorySeparatorChar)[Console.CursorTop - 7] : "" : "");

            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(filePath);
            }
        }
    }

    public static class MenuNavigation
    {
        public static void Main(string[] args)
        {
            Explorer.Run();
        }
    }

}