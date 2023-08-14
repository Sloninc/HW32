using System.Text;
namespace HW32
{
    public class Program
    {
        static async Task Main()
        {
            string path1 = @"c:\Otus\TestDir1";
            string path2 = @"c:\Otus\TestDir2";
            DirectoryInfo directoryInfo1 = new DirectoryInfo(path1);
            directoryInfo1.Create();
            DirectoryInfo directoryInfo2 = new DirectoryInfo(path2);
            directoryInfo2.Create();
            await CreateFileAndWrite(path1);
            await CreateFileAndWrite(path2);
            await WriteFilesAddition(directoryInfo1);
            await WriteFilesAddition(directoryInfo2);
            await WriteFileInfo(directoryInfo1);
            await WriteFileInfo(directoryInfo2);
            Console.ReadLine();
        }
        static async Task CreateFileAndWrite(string directoryPath)
        {
            for (int i = 0; i < 10; i++)
            {
                string name = "";
                try
                {
                    // запись в файл
                    using (FileStream fstream = File.Create(directoryPath + $"\\File_{(i + 1):D2}.txt"))
                    {
                        name = fstream.Name;
                        // преобразуем строку в байты
                        byte[] buffer = Encoding.UTF8.GetBytes(name);
                        // запись массива байтов в файл
                        await fstream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine($"Файл {name} доступен только для чтения или является каталогом.");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Указан недопустимый путь");
                }
                catch (IOException)
                {
                    Console.WriteLine($"Файл {name} уже открыт.");
                }
            }
        }

        static async Task WriteFilesAddition(DirectoryInfo info)
        {
            try
            {
                FileInfo[] files = info.GetFiles();
                foreach (var file in files)
                {
                    try
                    {
                        // запись в файл
                        using (FileStream fstream = file.Open(FileMode.Append))
                        {
                            if (fstream.CanWrite)
                            {
                                // преобразуем строку в байты
                                byte[] buffer = Encoding.UTF8.GetBytes(" " + DateTime.Now.ToString());
                                // запись массива байтов в файл
                                await fstream.WriteAsync(buffer, 0, buffer.Length);
                            }
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"Файл {file.Name} не найден.");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine($"Файл {file.Name} доступен только для чтения или является каталогом.");
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("Файл уже открыт.");
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Указан недопустимый путь");
            }

        }

        static async Task WriteFileInfo(DirectoryInfo info)
        {
            Console.WriteLine($"Имя файла:\t\tСодержимое файла");
            try
            {
                var files = info.GetFiles();
                //for (int i = 0; i < files.Length; i++)
                foreach(var file in files)
                {
                    string name = "";
                    try
                    {
                        // запись в файл
                        name = file.Name;
                        using (FileStream fstream = File.OpenRead(file.FullName))
                        {
                            // выделяем массив для считывания данных из файла
                            byte[] buffer = new byte[fstream.Length];
                            // считываем данные
                            await fstream.ReadAsync(buffer, 0, buffer.Length);
                            // декодируем байты в строку
                            string textFromFile = Encoding.UTF8.GetString(buffer);
                            Console.WriteLine($"{name}:\t\t{textFromFile}");
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine($"Файл {name} доступен только для чтения или является каталогом.");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"Файл {name} не найден.");
                    }
                    catch (IOException ex)
                    {
                        //foreach(Exception exception in ex.InnerException)
                        Console.WriteLine($"Файл {name} уже открыт.");
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Указан недопустимый путь");
            }
        }
    }
}