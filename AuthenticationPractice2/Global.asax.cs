using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace AuthenticationPractice2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e) //每次post验证请求的时候运行
        {
            var authCookies = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName]; //取出 Cookies里对应名字的Cookie
            if (authCookies != null) //如果Cookie正常，进行如下操作
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookies.Value); //将Cookie里的数据解码，获得明码Ticket
                if (authTicket != null && !authTicket.Expired) //如果Ticket存在，并且没有过期，进行如下操作
                {
                    //The following part is different with CompleteMVC
                    //Need to figure out why
                    var roles = authTicket.UserData.Split(','); //Ticket里面没有记错的话存的是Roles，于是用逗号split，以后得到roles
                    HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(authTicket), roles); //这部看不懂，问Abhilash
                    //HttpContext is not static, why don't we initialize it? or when was it initialized?
                    //The summary says it's Http information about "an individual request"
                    //Current 是静态方法，返回当前Http请求的HttpContext 实例， 也就是说HttpContext是在HttpContext.Current这里被实例化的
                    //User 是 Security information for the current HTTP request, 但从定义看，他似乎返回一个Iprincipal？interface？
                    //将Ticket输入构造器，实例化FormsIdentity实例，这里输入的是解码后的Ticket，注意在这里的Ticket是FormsAuthenticationTicket， 但是之前AccountController里遇到的明码只是个string
                    //接收FormsIdentity和roles array，生成GenericPrincipal实例，为什么要这样做？
                    
                }
            }

        }
    }
}
