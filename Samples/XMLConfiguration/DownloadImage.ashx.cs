using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Samples.XMLConfiguration
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DownloadImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
			FileInfo f = new FileInfo(HttpContext.Current.Server.MapPath(context.Request.QueryString["image"]));
            context.Response.AddHeader("content-disposition", "attachment;filename=" + f.Name.Replace(" ", "_"));
			context.Response.TransmitFile(context.Request.QueryString["image"]);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
