using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitGud.Controllers.Web
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //here to add everything that an admin can do
        //create categories, manage users etc.
        //first to find out how a user can take role=admin???
    }
}
