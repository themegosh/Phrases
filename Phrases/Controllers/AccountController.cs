

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Phrases.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Phrases.Controllers
{
    public class AccountController : ApiController
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController() {  }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetLoginStatus ()
        {
            return jsonResponse(new { Message = "Logged In.", UserName = User.Identity.Name }, HttpStatusCode.OK);
        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[LayoutInjector("_LoginRegisterLayout")]
        public async Task<HttpResponseMessage> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return jsonResponse(new { Message = "Invalid login request." }, HttpStatusCode.BadRequest);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            //return jsonResponse(result, HttpStatusCode.OK);

            switch (result)
            {
                case SignInStatus.Success:
                    return jsonResponse(new { Message = "Login success!", UserName = User.Identity.Name }, HttpStatusCode.OK);
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    return jsonResponse(new { Message = "Could not log in with supplied credentials." }, HttpStatusCode.BadRequest);
            }
        }

        // POST: /Account/LogOff
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public HttpResponseMessage LogOff()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return jsonResponse(new { Message = "Logout success!" }, HttpStatusCode.OK);
        }
        
        private HttpResponseMessage jsonResponse(object obj, HttpStatusCode status)
        {
            HttpResponseMessage response = Request.CreateResponse(status);
            response.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
