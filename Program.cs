using System.Drawing;
using Console = Colorful.Console; // Make sure to have the library installed.

namespace FileManager {
    class Program {
        private static string currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private static int currentFileOrFolder;
        private static string currentSelectedPath = "";

        static void ClearConsole() {
            if (!Console.IsOutputRedirected) {
                Console.Clear();
            }
            else {
                Console.Write("\u001b[2J");
            }
        }

        static List<string> GetPathsFromDirectory() {
            List<string> folderPaths = new (Directory.GetDirectories(currentDirectory));
            List<string> filePaths = new (Directory.GetFiles(currentDirectory));
            List<string> directoryPaths = new (folderPaths.Concat(filePaths));

            return directoryPaths;
        }

        static void WriteFoldersAndFiles() {
            ClearConsole();

            if (currentDirectory != null) {
                Console.WriteLine($"{currentDirectory[3..].Replace("\\", " -> ")}\n");
            }

            List<string> folderPaths = new (Directory.GetDirectories(currentDirectory));
            List<string> filePaths = new (Directory.GetFiles(currentDirectory));
            List<string> directoryPaths = GetPathsFromDirectory();

            if (currentFileOrFolder > directoryPaths.ToArray().Length - 1) {
                currentFileOrFolder = directoryPaths.ToArray().Length - 1;
            }

            string longestPath = directoryPaths.OrderByDescending(s => s.Length).First();

            foreach (string folderPath in folderPaths) {
                string folderName = Path.GetFileName(folderPath);
                if (currentFileOrFolder == directoryPaths.IndexOf(folderPath)) {
                    Console.WriteLine($"{folderName}{new string(' ', longestPath.Length - folderName.Length)}Folder", Color.Yellow);
                    currentSelectedPath = folderName;
                }
                else {
                    Console.WriteLine($"{folderName}{new string(' ', longestPath.Length - folderName.Length)}Folder");
                }
            }

            Console.WriteLine();

            foreach (string filePath in filePaths) {
                string fileName = Path.GetFileName(filePath);
                if (currentFileOrFolder == directoryPaths.IndexOf(filePath)) {
                    Console.WriteLine($"{fileName}{new string(' ', longestPath.Length - fileName.Length)}File", Color.Yellow);
                    currentSelectedPath = fileName;
                }
                else {
                    Console.WriteLine($"{fileName}{new string(' ', longestPath.Length - fileName.Length)}File");
                }
            }
        }
        
        static void Main(string[] args) {
            while (true) {
                WriteFoldersAndFiles();

                while (true) {
                    ConsoleKey keyPressed = Console.ReadKey().Key;
                    if (keyPressed == ConsoleKey.UpArrow) {
                        if (currentFileOrFolder == 0) {
                            currentFileOrFolder = GetPathsFromDirectory().ToArray().Length - 1;
                        }
                        else {
                            currentFileOrFolder -= 1;
                        }
                        break;
                    }
                    else if (keyPressed == ConsoleKey.DownArrow) {
                        if (currentFileOrFolder == GetPathsFromDirectory().ToArray().Length - 1) {
                            currentFileOrFolder = 0;
                        }
                        else {
                            currentFileOrFolder += 1;
                        }
                        break;
                    }
                    else if (keyPressed == ConsoleKey.Enter) {
                        string fullPath = Path.Combine(currentDirectory, currentSelectedPath);

                        if (Directory.Exists(fullPath)) {
                            currentDirectory += $"\\{currentSelectedPath}";
                        }
                        break;
                    }
                    else if (keyPressed == ConsoleKey.Backspace) {
                        currentDirectory = Path.GetDirectoryName(currentDirectory) ?? currentDirectory;
                        break;
                    }
                }
            }
        }
    }
}