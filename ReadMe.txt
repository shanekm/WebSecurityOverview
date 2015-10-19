WEB.CONFIG Settings

1. Adding custom erros to web page
	- protects from public version number, net info etc
	- add <custom errors> tag in web.config => <system.web> without redirect/url change
		<customErrors mode ="RemoteOnly" defaultRedirect="~/Error.aspx" redirectMode="ResponseRewrite"></customErrors>

2. Session state
	- ASP.NET_SessionId => cookie identifier that stores session data
	- changing asp.net session cookie name
		<sessionState mode="InProc" customProvider="DefaultSessionProvider" cookieName="MySessionCookie">

3. Tracing - used for debugging - writing out info stuff
	- <system.web> => <trace enabled="true" localOnly="true" /> => url/trace.axd
	Trace.Write("this is trace");

4. Securing Content using the Location Element
	- in web.config you can specify access to a single/specific page by using <location> attribute
	<configuration>
	...
		<location path="Contact">
			<system.web>
			  <authorization>
				<allow roles="Admin" /><!-- allow only Admin roles -->
				<deny users="*" /><!-- deny all other roles -->
			  </authorization>
			</system.web>
		  </location>
	</configuration> 

5. Hiding ASP.NET version number
	- Response headers display IIS ver, asp.net version and other specific data. You can turn off this info
	<httpRuntime targetFramework="4.5" enableVersionHeader="false" /> - enableVersionHeader does not display version anymore

6. HTTP Cookies
	- HttpOnly - client script can NOT access cookie info
	- Secure - cookie can only be sent over HTTPS connection

	// HttpOnly flag in the browser does not have that check
    // client script can access cookie if HttpOnly flag is not checked
    var cookie = new HttpCookie("MyCookie", "Bob's cookie");
    Response.Cookies.Add(cookie);

	- in order to set HttpOnly for cookies use web.config
	<system.web>...<httpCookies httpOnlyCookies="true" requireSSL="true" /> - cookie can NO longer be access by client code/script AND only over HTTPS
	- this behaviour can be overwritten in the code: 
		cookie.Secure = false;

7. Deployment Retail
	<system.web>...<deployment retail="true" /> - disables debugging, trace.axd. It acts as if on the live envirenment

8. maxRequestLength setting
	<httpRuntime targetFramework="4.5" enableVersionHeader="false" maxRequestLengh ="4096" /><!-- maxRequestLength in Kilobytes (4 Megabytes) -->
	- will accept requests only no more than 4 Megabytes



SECURITY (Membership/Identity and Roles)
	<authentication>
	.ASPXAUTH - cookie that holds authentication data
    <authentication>
      <!-- timeout in minutes: 2880 => 2 days until will be automatically logged out/ 120 min => 2 hours etc--> 
      <!-- slidingExpiration => you have to stop doing stuff for 2 hours before you get logged out, true by default, if false you will be logged out after 2 hours even you're doing stuff -->
      <!-- protection => how the cookie is protected. All by default -->
      <forms loginUrl="~/Account/Login" timeout="2880" defaultUrl="~/" requireSSL="true" slidingExpiration="true" protection="All" /> 
    </authentication>

	<membership> settings - control provider and password settings etc
	<roleManager> settings - control access to features based on roles

MVC
1. Output Encoding
	<script> tags are enocoded  &lt;script&gt;alert etc.. instead of <script> tag - feature anti SQL injection
	Html.Raw Helper does not encode 
	@Html.Raw(Model.MyAttribute) => alerts 'Hello World!' - will generate javascript
	- if someone passed script in URL and this was emitted you would have a risk
	When to use Html.Raw()?
		- Wysiwyg
	[AllowHtml] => able to post Html to controller or javascript code (a potentially dangerous request error message) without this tag

2. Cross Site Request Forgery (CSRF)
	- user clicking on (hacked) button to the page that user is already authenticated to, to submit some hack code
	[ValidateAntiForgeryToken]
	AntiForgeryToken is generated on the <form> => _RequestVerificationToken (pair of values) 
		- hidden form paired with cookie value (one pair on the form tag, one in the browser) they need to match

3. [Authorization] / [AllowAnonymous]
	- [Authorize] on the controller, [AllowAnonymous] allows single Action to be executed without being logged in

4. [RequreHttps] 
	- only able to execute action via HTTPs only

5. Http Verb
	


OWIN and Katana
	- How are you going to interact with the host?, how does my software interact with that host (usually IIS)
	- Katana - build on a specification called OWIN. Used to provide light weight piece software to build web apps. Microsoft implementation
	- System.Web assembly 
		used by MVC at its core to process HTTP request/responses etc (it includes everything from caching, configuration to web sockets)
		instead of one giant assembly Katana features are HIGHLY MODULAR and componatized so that you can add to your project features you only need
		tied to ASP.Net and IIS
	OWIN (Open Web Server Interface for .Net) - specification (Api) for Web Framework and Web Server to remain decoupled (self hosting etc)
		You write a web compoment for OWIN, you can run your app anywhere where OWIN is supported and not only IIS (Linux etc)

1. KatanaIntro Project
	IAppBuilder - configuration class allows you specify how this app will behave and respond to HTTP requests

	AppFunc - application function is how components process and interact: Func<IDictionary<string, object> Task> 
		=> Dictionary represents request envirenment (request/response/cookies/headers etc) similar to HTTP context - carries all info about specific request
		=> Task - returned by AppFunc - all Async
			public async Task Invoke<IDictionary<string, object> envirenment)
			{
				// processing
				await _nextComponent(envirenment); // call next component
				// processing
			}

	app.Use<HelloWorldComponent>(); // Register new component
	AppFunc => Func<IDictionary<string, object>, Task> _next; // required constructor taking in AppFunc

	Building Extension method for new HelloWorldComponent
	Owin Component (above) - also called Middleware. It's called Middleware because it sits between the Owin processing pipeline and there might be before/after other components

	What was build?
	Architecture: Host => Server => Katana => Application ()

2. Adding Web Api Controller
	- installing package self hosting (which added system.net.http, system.web.http etc) - ConfigureWebApi(IAppBuilder app)
	Startup.cs [assembly: OwinStartupAttribute(typeof(WebSecurityOverview.Startup))] => when on IIS this is class that IIS looks for OWIN configuration class


MVC Authentication
	 - No authentication
	 - Individual User Accounts - Forms based Authentication. Used for internet users (twitter/google etc)
	 - Organizational Accounts - Cloud based, active directory, Office 365 accounts etc
	 - Windows Authentication - for intranet applications. Using Active directory

1. Identity (Microsoft.AspNet.Identity.Core) 
	- defines core abstraction for Identity 
	Web.Config - hold info about DB users accounts - DefaultConnection (by default user data is looked up in DefaultConnection)
	Core Identity
		a. IUser
		b. IRole
		c. IUserStore<TUser> => implement this interface to have control over how users are managed/stored
		d. UserManager<TUser>
		e. IUserLoginStore<TUser>
		f. IUserPasswordStore<TUser>
            
	Core Identity - Entity Framework
		a. IdentityUser - class that describes user (Id, logins, PasswordHash, Roles etc)
		b. IdentityRole - (id, name)
		c. UserStore<TUser> - implements IUserLoginStore/IUserPassword store etc
		d. IdentityDbContext<TUser> - Roles/Users

	External Logins
