using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CTDT.WebApi.Utilities
{
	public static class CommonUtils
	{
        public static bool DeleteFile(string fileName, string employeeFolder)
        {
            try
            {
                if (File.Exists(Path.Combine(HostingEnvironment.MapPath("~/Uploads/" + employeeFolder), fileName)))
                {
                    File.Delete(Path.Combine(HostingEnvironment.MapPath("~/Uploads/" + employeeFolder), fileName));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}