using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using CTDT.Model;
using CTDT.WebApi.Utilities;

namespace CTDT.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Dynamically create new timer
            System.Timers.Timer timScheduledTask = new System.Timers.Timer();

            // Timer interval is set in miliseconds,
            // Run a task every minute
            timScheduledTask.Interval = 60 * 1000;

            timScheduledTask.Enabled = true;

            // Add handler for Elapsed event
            timScheduledTask.Elapsed +=
            new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);
        }

        protected void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var dateTimeNow = DateTime.Now;
            // Execute task: Check expire review task
            if (dateTimeNow.Hour == 23 && dateTimeNow.Minute == 50)
            {
                // Check expire project
                DetechExpiredReviewTask();
            }
        }

        // Check review task
        protected void DetechExpiredReviewTask()
        {
            //using ( var db = new DBContext())
            //{
            //    var taskReviews = db.ViewTaskReviews.ToList();

            //    var now = DateTime.Now;
            //    foreach (var task in taskReviews)
            //    {
            //        if (task.EndDate != null && (now - task.EndDate.Value).Days > 30)
            //        {
            //            if (!string.IsNullOrEmpty(task.UrlImageResult) && task.UrlImageResult.Length > 1)
            //            {
            //                try
            //                {
            //                    var splitUrl = task.UrlImageResult.Split(new string[] { "/" }, StringSplitOptions.None);

            //                    var deleteImg = CommonUtils.DeleteFile(splitUrl[3], splitUrl[2]);
            //                }
            //                catch (Exception)
            //                {
            //                    // ignored
            //                }
            //            }

            //            var orgTask = db.Tasks.SingleOrDefault(x => x.Id == task.TaskId);
            //            if (orgTask != null)
            //            {
            //                orgTask.UrlImageResult = "";
            //                orgTask.IsReview = true;
            //            }
            //        }
            //    }

            //    db.SaveChanges();
            //}

        }
    }
}
