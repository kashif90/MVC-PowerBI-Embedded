using TestReport.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.PowerBI.Api.V1;
using Microsoft.PowerBI.Security;
using System.Threading.Tasks;
using Microsoft.Rest;

namespace TestReport.Controllers
{
    public class HomeController : Controller
    {

        private readonly string workspaceCollection = ConfigurationManager.AppSettings["powerbi:WorkspaceCollection"];
        private readonly string workspaceId = ConfigurationManager.AppSettings["powerbi:WorkspaceId"];
        private readonly string accessKey = ConfigurationManager.AppSettings["powerbi:AccessKey"];
        private readonly string apiUrl = ConfigurationManager.AppSettings["powerbi:ApiUrl"];


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Get All Reports
        [ChildActionOnly]
        public ActionResult Reports()
        {
            using (var client = this.CreatePowerBIClient())
            {
                var reportsResponse = client.Reports.GetReports(this.workspaceCollection, this.workspaceId);

                var viewModel = new ReportsViewModel
                {
                    Reports = reportsResponse.Value.ToList()
                };
                return PartialView(viewModel);
            }
        }

        public async Task<ActionResult> Report(string reportId)
        {

            using (var client = this.CreatePowerBIClient())
            {
                var reportsResponse = await client.Reports.GetReportsAsync(this.workspaceCollection, this.workspaceId);
                var report = reportsResponse.Value.FirstOrDefault(r => r.Id == reportId);

                //Create Embedd Token for Report
                var embedToken = PowerBIToken.CreateReportEmbedToken(this.workspaceCollection, this.workspaceId, report.Id);


                // Pass report and access token to MVC view for rendering
                var viewModel = new ReportViewModel //need Report object and AccessToken
                {
                    Report = report,
                    AccessToken = embedToken.Generate(this.accessKey)
                };

                return View(viewModel);
            }
        }

        private IPowerBIClient CreatePowerBIClient()
        {
            var credentials = new TokenCredentials(accessKey, "AppKey");
            var client = new PowerBIClient(credentials)
            {
                BaseUri = new Uri(apiUrl)
            };

            return client;
        }

    }
}