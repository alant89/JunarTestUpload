using Junar;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var title = "Food Service Inspections";
            var category = "health-and-human-services";
            var description = "Data on inspections of food establishments licensed in Arlington.Each row of data represents an inspection and any specific violation that may have been cited.";
            var guid = "FOOD-SERVI-INSPE";
            var extension = ".xlsx";

            var path = "c:\\Users\\atran\\Desktop\\Quarterly Food Data FY19 Q1.xls";
            var memoryStream = new MemoryStream(File.ReadAllBytes(path));
            var data = memoryStream.ToArray();

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
