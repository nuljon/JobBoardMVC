namespace JobBoardMVC.Migrations
{
    using Models;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;



    internal sealed class Configuration : DbMigrationsConfiguration<JobBoardMVC.Models.JobBoardMvcContext>
    {

        public Configuration()

        {

            AutomaticMigrationsEnabled = false;
            ContextKey = "JobBoardMVC.Models.JobBoardMvcContext";

         //   JobBoardMvcContext context = new JobBoardMvcContext();

           // Seed(context);

        }
        


        protected override void Seed(JobBoardMvcContext context)
        {
            // Your seed code here...
            if (context.Jobs.Any())
            {
                return;
            }
            else
            {

                var jobsJsonAll = GetEmbeddedResourceAsString("JobBoardMVC.App_Data.Amazon.json");



                JArray jsonValJobs = JArray.Parse(jobsJsonAll) as JArray;
                dynamic jobsData = jsonValJobs;
                foreach (dynamic job in jobsData)
                {

                    context.Jobs.Add(new Job
                    {
                        ApplicationLink = job.ApplicationLink,
                        Company = job.Company,
                        DatePosted = job.DatePosted,
                        Experience = job.Experience,
                        Hours = job.Hours,
                        JobID = job.JobId,
                        JobTitle = job.JobTitle,
                        LanguagesUsed = job.LanguagesUsed,
                        Location = job.Location,
                        Salary = job.Salary

                    });
                }
            }

            // Make sure to have the context save changes and to call the base seed method afterwards.
            context.SaveChanges();
            base.Seed(context);
        }

        private static string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //var names = assembly.GetManifestResourceNames();

            string result;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))

            using (StreamReader reader = new StreamReader(stream))

                result = reader.ReadToEnd();

            return result;


        }
    }
}
