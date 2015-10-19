using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSecurityOverview
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // HttpOnly flag in the browser does not have that check
            // client script can access cookie if HttpOnly flag is not checked
            var cookie = new HttpCookie("MyCookie", "Bob's cookie");
            cookie.Secure = false;
            Response.Cookies.Add(cookie);
        }
    }
}