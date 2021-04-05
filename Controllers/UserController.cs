using MyTasks.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

namespace MyTasks.Controllers
{
    public class UserController : Controller
    {
       
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (MyTasksContext ctx = new MyTasksContext())
                    {
                        var user = (from u in ctx.Users 
                                   where u.Email == model.Email && u.Password == model.Password
                                   select u).SingleOrDefault();

                        if (user == null)
                        {
                            ViewBag.Message = "Login inválido, tente novamente !";
                            return View(model);
                        }
                        else
                        {
                            Session.Add("userid", user.Id);
                            Session.Add("email", user.Email);
                            FormsAuthentication.SetAuthCookie(user.Email, false);
                            return RedirectToAction("Home");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Não foi possível processar o login.";
                    System.Web.HttpContext.Current.Trace.Write("UserController.Login() --> " + ex.Message);
                    return View(model);
                }
            }
            else
            {
                ViewBag.Message = "Informe o email e a senha !";
                return View(model);
            }
        }

        public ActionResult Register()
        {
            RegistrationModel model = new RegistrationModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User u = new User { Email = model.Email, Password = model.Password, JoinedOn = DateTime.Now };
                    using (MyTasksContext ctx = new MyTasksContext())
                    {
                        ctx.Users.Add(u);
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Login");

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "O endereço de email ja esta sendo utilizado!";
                    System.Web.HttpContext.Current.Trace.Write("UserController.Register() --> " + ex.Message);
                    return View(model);
                }
            }
            else
            {   ViewBag.Message = "Informe um valor válido !";
                return View(model);
             }

        }

        public ActionResult Recovery()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }

        /// <summary>
        /// Neste método você precisa informar os valores corretos do seu servidor
        /// SMTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Recovery(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (MyTasksContext ctx = new MyTasksContext())
                    {
                        var user = (from u in ctx.Users
                                    where u.Email == model.Email
                                    select u).SingleOrDefault();

                        if (user == null)
                        {
                            ViewBag.Message = "O usuário deste email não foi encontrado!";
                            return View(model);
                        }
                        else
                        {
                            // envia email com senha
                            MailMessage m = new MailMessage();
                            m.To.Add(new MailAddress(user.Email));
                            m.From = new MailAddress("admin@mytasks.com");
                            m.Subject = "Recuperar Senha ";
                            m.IsBodyHtml = true;
                            m.Body = "Prezado Usuário, <p/>" +
                               "Use a seguinte senha para realizar o login.<p/>Senha : " + user.Password +
                               "<p/>Admin,<br/>MyTarefas.Net";


                            SmtpClient smtp = new SmtpClient("localhost");
                            smtp.Send(m);
                            ViewBag.Message = "Um email com sua senha foi enviado!";
                            return View(model);
                        } // else
                    } // using
                } 
                catch (Exception ex)
                {
                    ViewBag.Message = "Não foi possível recuperar a senha";
                    System.Web.HttpContext.Current.Trace.Write("UserController.Recovery() --> " + ex.Message);
                    return View(model);
                }
            }
            else
            {
                ViewBag.Message = "Forneça um email válido !";
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Home()
        {
            // get list of projects
            using (MyTasksContext ctx = new MyTasksContext())
            {
                var userid = Int32.Parse(Session["userid"].ToString ());
                var projects = from p in ctx.Projects.Include("Tasks").ToList<Project>()
                               where p.UserId == userid
                               select p;

                return View(projects);
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }



        public ActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (MyTasksContext ctx = new MyTasksContext())
                    {
                        string email = Session["email"].ToString();
                        var user = (from u in ctx.Users
                                    where u.Email == email 
                                    select u).SingleOrDefault();

                        // verifica se a senha naterior é a mesma que a do database
                        if ( user.Password == model.Password)
                        {
                            user.Password = model.NewPassword;
                            ctx.SaveChanges();
                            ViewBag.Message = "A senha foi alterada com sucesso !";
                        }
                        else
                        {
                            ViewBag.Message = "A senha anterior esta incorreta !";
                        }

                    }
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Não foi possível processar sua requisição.";
                    System.Web.HttpContext.Current.Trace.Write("UserController.ChangePasssword() --> " + ex.Message);
                    return View(model);
                }
            }
            else
            {
                ViewBag.Message = "Informação inválida !";
                return View(model);
            }

        }
	}
}