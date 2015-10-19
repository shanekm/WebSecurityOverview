namespace KatanaIntro
{
    using System.Web.Http;

    public class GreetingController : ApiController
    {
        // http://localhost:8010/api/greeting
        // This route is matched 
        public Greeting Get()
        {
            return new Greeting{ Text = "Hello from api controller" };
        }
    }
}