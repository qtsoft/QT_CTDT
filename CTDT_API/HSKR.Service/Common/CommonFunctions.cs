using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CTDT.Service
{
    public static class CommonFunctions
    {
        public static int TryParseObjectToInt(object s)
        {
            int i;
            if (!int.TryParse(s + "", out i))
            {
                return 0;
            }
            else
            {
                return i;
            }
        }

        public static bool ObjectIsInteger(object s)
        {
            int i;
            if (!int.TryParse(s + "", out i))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int TryParseId(string s)
        {
            int i;
            if (!int.TryParse(s, out i))
            {
                return 0;
            }
            else
            {
                return i;
            }
        }

        public static int? TryParseParentId(string s)
        {
            int i;
            if (!int.TryParse(s, out i))
            {
                return null;
            }
            else
            {
                return i;
            }
        }

        //Hàm gán giá trị cho thuộc tính generic của 1 object generic
        public static void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }

    }
}
