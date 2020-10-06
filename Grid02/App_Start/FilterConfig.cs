using System.Web;
using System.Web.Mvc;

/**
* The School Navigator
*
* @author  Vaishnavi Sinha
* @version 1.0
* @Date   05-01-2019 
*/
namespace Grid02
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
