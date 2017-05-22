using Microsoft.AspNetCore.Mvc;
using TopCore.Auth.Filters;
using TopCore.Framework.Web;

// ReSharper disable once CheckNamespace
namespace TopCore.Auth.Areas.Developers.Controllers
{
    [Area("Developers")]
    [DeveloperAccessFilter]
    [HideInDocs]
    public class DevelopersMvcController : Controller
    {
    }
}