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

            var files = directoryInfo1.GetFiles();
   
            Console.WriteLine("Имя файла\t\tТекст из файла");
            for(int i = 0; i < files.Length; i++)
            {
                string name = Path.GetFileNameWithoutExtension(path1 + $"\\File{i + 1}.txt");
                using (FileStream fstream = File.OpenRead(path1 + $"\\File{i + 1}.txt"))
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
            Console.ReadLine();
        }
        async Task CreateFileAndWrite(string directoryPath)
        {
            for (int i = 0; i < 10; i++)
            {
                string name = "";
                try
                {
                    // запись в файл
                    using (FileStream fstream = File.Create(directoryPath + $"\\File{i + 1}.txt"))
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

        async Task WriteFilesAddition(DirectoryInfo info)
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
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Указан недопустимый путь");
                }
                catch (IOException)
                {
                    Console.WriteLine("Файл уже открыт.");
                }
            }
        }
    }
}