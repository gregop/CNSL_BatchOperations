using Utils;
using System.IO;
namespace CNSL_BatchOperations
{
    internal class Program
    {
        static void Main(string[] args)
        {


            string file_name = "validBatchFile.txt";

            /* run this when on Debug mode.
             * Directory.GetCurrentDirectory() returns the current 
             * working directory, which by default is the bin\Debug 
             * when running the application from within an IDE.
             */

            string directory_path = Path.Combine(Directory
                .GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent
                .Parent
                .Parent
                .FullName, "BatchFiles");


            string file_path = Path.Combine(directory_path, file_name);
            Console.WriteLine(file_path);


            Dictionary<string, object> batch_content = Utilities.ReadBatchFile(file_path);

        }
    }
}