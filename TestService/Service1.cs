﻿using Quartz;
using Quartz.Impl;
using System.ServiceProcess;
using TestService.Jobs;

namespace JunarMonitoring.Service
{
    public partial class Service1 : ServiceBase
    {
        private IScheduler scheduler;
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            UploadJob.Schedule(scheduler);
        }
        protected override void OnStop()
        {
            scheduler.Shutdown();
        }
    }
}
