using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSanjeevaniIcu.Portal.Controllers
{
    public class ApplicationConfigurations
    {
        public string ApplicationHostUrl { get; set; }
        public string ApplicationHostReactUrl { get; set; }
        public string attachmentUrlRoot { get; set; }
        public string videoUrlRoot { get; set; }
        public string FileAttachments { get; set; }
        public string MsgSaveSuccess { get; set; }
        public string MsgUpdateSuccess { get; set; }
        public string MsgDeleteSuccess { get; set; }
        public string MsgExist { get; set; }
        public string MsgError { get; set; }
        public string MsgActivated { get; set; }
        public string MsgDeactivated { get; set; }
        public string imageUrlRoot { get; set; }
    }
}
