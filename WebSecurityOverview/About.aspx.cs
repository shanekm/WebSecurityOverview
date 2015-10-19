using System;
using System.Web.UI;

namespace WebSecurityOverview
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Force exception 
            // this is not good because you're leaking information on the page to general public
            var i = Convert.ToInt16("x");
        }
    }
}