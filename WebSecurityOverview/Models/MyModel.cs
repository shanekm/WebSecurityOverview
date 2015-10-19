namespace WebSecurityOverview.Models
{
    public class MyModel
    {
        public string MyAttribute
        {
            get
            {
                return "<script>alert('hello world!');</script>";
            }
        }
    }
}