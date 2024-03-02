using eSanjeevaniIcu.Data.eSanjeevaniIcuDBEntities;
using eSanjeevaniIcu.Portal.Filters;
using eSanjeevaniIcu.Shared.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    [SessionTimeoutAttribute]
    public class CommonController : Controller
    {
        private readonly eSanjeevaniIcuDbContext _context;
        private EmailConfiguration EmailConfigurations { get; set; }
        private ApplicationConfigurations ApplicationConfigurations { get; set; }
        private readonly CommonFunctions _commonFunctions;
        public CommonController(eSanjeevaniIcuDbContext appcontext, IOptions<ApplicationConfigurations> settings, IOptions<EmailConfiguration> emailSettings)
        {
            _context = appcontext;
            ApplicationConfigurations = settings.Value;
            EmailConfigurations = emailSettings.Value;
            _commonFunctions = new CommonFunctions(_context);
        }

        public string GetSignalRCon(int UserId)
        {
            try
            {
                TbSignalRcon sr = new TbSignalRcon();
                sr = (from t1 in _context.TbSignalRcons
                      where t1.Status == true && t1.UserId == UserId
                      select new TbSignalRcon
                      {
                          Srcid = t1.Srcid,
                          UserId = t1.UserId,
                          ConnectionId = t1.ConnectionId,
                          Status = t1.Status,
                          CreatedOn = t1.CreatedOn,
                      }).OrderByDescending(o => o.Srcid).FirstOrDefault();

                return sr.ConnectionId;
            }
            catch (Exception ex)
            {
                return "NotFound";
            }
        }

        public List<SelectListItem> GetUserGroupList(Type enumType)
        {
            var userGroupList = new List<SelectListItem>();
            if (enumType.IsEnum)
            {
                userGroupList = System.Enum.GetValues(typeof(UserGroupEnum)).Cast<UserGroupEnum>()
                    .Select(e => new SelectListItem()
                    {
                        Value = Convert.ToString(e.GetHashCode()),
                        Text = e.ToString(),
                    }).ToList();
            }

            return userGroupList;
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(ApplicationConfigurations.attachmentUrlRoot, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        public string AppendTimeStamp(string fileName)
        {
            string modFileName = Path.GetFileNameWithoutExtension(fileName);
            modFileName = Regex.Replace(modFileName, @"[^0-9a-zA-Z]+", "");
            return string.Concat(
                modFileName,
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }
    }
}