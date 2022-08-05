using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("/Error/{statusCode}")]
        public ViewResult HttpStatusCodeHandler(int statusCode)
        {
            var httpService = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, The page you are looking for is not found, Page = " + httpService.OriginalPath + ", params= " + httpService.OriginalQueryString;

                    break;
            }
            return View("ErrorMessage");
        }

        [Route("Error")]
        [AllowAnonymous]
        public ViewResult Error(int statusCode)
        {
            var httpService = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            logger.LogError($"An Error has occured at path {httpService.Path} with Message {httpService.Error.Message}");
            return View("Error");
    }
    }
}
