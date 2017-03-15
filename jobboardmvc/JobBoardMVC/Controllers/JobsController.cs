using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoardMVC.Models;

namespace JobBoardMVC.Controllers
{
    public class JobsController : Controller
    {
        private JobBoardMvcContext context = new JobBoardMvcContext();

        // GET: Jobs
        // Include LINQ query to allow search
        public async Task<ActionResult> Index(string jobTitleString, string jobLocation, string companyString)
        {
            IQueryable<string> locationQuery = from j in context.Jobs
                                               orderby j.Location
                                               select j.Location;

            var jobs = from j in context.Jobs
                       select j;
            // Grab a count of the number of jobs in the database to display:
            int count = jobs.Count();
            ViewBag.Counts = count;

            if (!String.IsNullOrEmpty(jobTitleString))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(jobTitleString));
            }

            if (!String.IsNullOrEmpty(jobLocation))
            {
                jobs = jobs.Where(j => j.Location == jobLocation);
            }

            if (!String.IsNullOrEmpty(companyString))
            {
                jobs = jobs.Where(j => j.Company.Contains(companyString));
            }

            var jobLocationVM = new JobLocationViewModel();
            jobLocationVM.locations = new SelectList(await locationQuery.Distinct().ToListAsync());
            jobLocationVM.jobs = await jobs.ToListAsync();

            return View(jobLocationVM);
            //       return View(await context.Jobs.ToListAsync());
        }




        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ApplicationLink,Company,DatePosted,Experience,Hours,JobID,JobTitle,LanguagesUsed,Location,Salary")] Job job)
        {
            if (ModelState.IsValid)
            {
                context.Jobs.Add(job);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ApplicationLink,Company,DatePosted,Experience,Hours,JobID,JobTitle,LanguagesUsed,Location,Salary")] Job job)
        {
            if (ModelState.IsValid)
            {
                context.Entry(job).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await context.Jobs.FindAsync(id);
            context.Jobs.Remove(job);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
