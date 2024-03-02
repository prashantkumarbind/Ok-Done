using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace eSanjeevaniIcu.Portal.Controllers
{
    public class HtmlController : Controller
    {
        public IActionResult ServeHtmlPage()
        {
            // Specify the path to your HTML file
            var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "test.html");

            // Check if the file exists
            if (System.IO.File.Exists(htmlFilePath))
            {
                // Serve the HTML file
                return PhysicalFile(htmlFilePath, "text/html");
            }
            else
            {
                return NotFound(); // Return a 404 Not Found if the file doesn't exist
            }
        }
    }
}
