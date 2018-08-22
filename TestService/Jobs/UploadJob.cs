using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DataStore.Junar;
using Quartz;

namespace TestService.Jobs
{
    class UploadJob : IJob
    {
        public UploadJob()
        {
        }
        public static void Schedule(IScheduler scheduler)
        {
            const string jobName = "UploadJob";
            const string groupName = "test";

            var jobDetail = JobBuilder.Create<UploadJob>()
                .WithIdentity(jobName, groupName)
                .Build();

            var trigger = TriggerBuilder.Create()
                .ForJob(jobName, groupName)
                .StartNow()
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var title = "Food Service Inspections";
                var category = "health-and-human-services";
                var description = "Data on inspections of food establishments licensed in Arlington.Each row of data represents an inspection and any specific violation that may have been cited.";
                var guid = "FOOD-SERVI-INSPE";
                var extension = ".xls";

                var path = "c:\\Users\\atran\\Desktop\\testdawd.xlsx";
                var memoryStream = new MemoryStream(File.ReadAllBytes(path));
                var data = memoryStream.GetBuffer();

                JunarUploader uploader = new JunarUploader();
                uploader.UploadFile(data
                    , title
                    , category
                    , description
                    , extension
                    , guid
                    );
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
