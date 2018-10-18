using Junar;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var title = "Test Upload 123";
            var category = "government";
            var description = "Testing upload";
            var guid = "TEST-UPLOA-123";
            var extension = ".csv";

            var path = "c:\\Users\\atran\\Desktop\\testuploadinspections.csv";
            var data = File.ReadAllBytes(path);

            Console.WriteLine("press return to run");
            Console.ReadKey();

            Console.WriteLine("");
            Console.WriteLine("Loading..");
            Console.WriteLine("");

            JunarUploader uploader = new JunarUploader();
            var hold = uploader.UploadFile(data
                , title
                , category
                , description
                , extension
                , guid
                );

            Console.WriteLine(hold);

            Console.WriteLine("");
            Console.WriteLine("Press return to exit");
            Console.WriteLine("");
            Console.ReadKey();
        }
    }
}
