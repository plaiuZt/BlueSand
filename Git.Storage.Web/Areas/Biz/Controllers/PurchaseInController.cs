using Git.Storage.Web.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Biz.Controllers
{
    public class PurchaseInController : MasterPage
    {
        //
        // GET: /Biz/PurchaseIn/

        public ActionResult List()
        {
            return View();
        }

    }
}
