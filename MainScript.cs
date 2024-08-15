namespace DOOM1_2WadLoader
{
    public class MainScript
    {
        bool disableTitleNaming = false;
        public MainScript(string[] args)
        {
            IEnumerable<string> wadfiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.wad");
            string bnetWadsDir = "";
            bnetWadsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games", "Nightdive Studios", "DOOM", "bnetwads");

            bool repairMode = false;

            foreach (string arg in args)
            {
                if (Directory.Exists(arg))
                    bnetWadsDir = arg;

                if (arg == "-n")
                    disableTitleNaming = true;

                if (arg == "-r")
                    repairMode = true;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nFinding bnetwads folder...");
            if (!Path.Exists(bnetWadsDir))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("bnetwads folder not found");
                Console.ReadLine();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("bnetwads folder found!");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nStarting repair mode...");
            if (repairMode)
            {
                //big code incoming
                IEnumerable<DirectoryInfo> wadFolders = new DirectoryInfo(bnetWadsDir).EnumerateDirectories();
                foreach (DirectoryInfo wadfolder in wadFolders)
                {
                    string folderPath = Path.Combine(bnetWadsDir, wadfolder.Name);
                    string wadFileFoundInDir = Directory.EnumerateFiles(folderPath, "*.wad").FirstOrDefault("");
                    string indexFileFound = Directory.GetFiles(folderPath, "index.json").FirstOrDefault("");

                    if (!string.IsNullOrEmpty(wadFileFoundInDir))
                    {
                        //if wad file is empty, delete folder
                        if (new FileInfo(wadFileFoundInDir).Length == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Folder {wadfolder.Name} is not functional. deleting folder...");
                            Directory.Delete(folderPath, true);
                        }
                        else
                        {
                            string wadName = Path.GetFileNameWithoutExtension(wadFileFoundInDir);
                            //check folder's index.json, if missing or empty create one
                            if (string.IsNullOrEmpty(indexFileFound) || (new FileInfo(indexFileFound).Length == 0))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Folder {wadfolder.Name} is missing index.json");
                                WriteJsonFileToDir(folderPath, wadName, wadName + ".wad");
                            }

                            //handle folders with unequal namings to their wads
                            if (!string.Equals(wadName, wadfolder.Name))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                string newFolderPath = Path.Combine(bnetWadsDir, wadName);
                                Directory.CreateDirectory(newFolderPath);
                                foreach (string fileInFolder in Directory.EnumerateFiles(folderPath))
                                {
                                    string newFilePath = Path.Combine(newFolderPath, Path.GetFileName(fileInFolder));
                                    if (!File.Exists(newFilePath))
                                        File.Copy(fileInFolder, newFilePath, false);
                                }
                                Directory.Delete(folderPath, true);
                                Console.WriteLine($"Renamed Folder {wadfolder.Name} to {wadName}");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Folder {wadfolder.Name} is functional.");
                            }
                        }
                    }
                    else
                    {
                        //if a folder has an index.json but no wad file, delete it
                        if (!string.IsNullOrEmpty(indexFileFound))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Folder {wadfolder.Name} is not functional. deleting folder...");
                            Directory.Delete(folderPath, true);
                        }
                    }
                }
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nGetting wads in current directory...");
            if (wadfiles.Count() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No Wad files found in current directory");
                Console.ReadLine();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{wadfiles.Count()}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" wad files found");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nStarting wad creation process..");
            foreach (string item in wadfiles)
            {
                string wadFileName = Path.GetFileName(item);
                string wadName = Path.GetFileNameWithoutExtension(item);
                string wadTitle = wadName;

                string wadFolder = Path.Combine(bnetWadsDir, wadName);
                Directory.CreateDirectory(wadFolder);

                WriteJsonFileToDir(wadFolder, wadName, wadFileName);

                File.Copy(wadFileName, Path.Combine(wadFolder, wadFileName), true);
                File.Delete(wadFileName);
            }
            Console.ResetColor();
        }

        private void WriteJsonFileToDir(string dirPath, string wadName, string wadFileName)
        {
            string wadTitle = wadName;

            if (!disableTitleNaming)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\nSpecify Title for");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($" {wadFileName} ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"(leave blank to use filename instead)");
                Console.ForegroundColor = ConsoleColor.White;
                string? typedTitle = Console.ReadLine();
                wadTitle = string.IsNullOrEmpty(typedTitle) ? wadName : typedTitle;
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(dirPath, "index.json"), false))
            {
                outputFile.WriteLine("{");
                outputFile.WriteLine($"\t\"id\" : \"{wadName.ToUpper()}\",");
                outputFile.WriteLine($"\t\"title\" : \"{wadTitle}\",");
                outputFile.WriteLine($"\t\"version\" : \"1\",");
                outputFile.WriteLine($"\t\"wad\" : \"{wadFileName}\"");
                outputFile.WriteLine("}");
            }
        }
    }
}

