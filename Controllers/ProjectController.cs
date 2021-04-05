using MyTasks.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyTasks.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string title)
        {
            TempData["message"] = "";
            try
            {
                using (MyTasksContext ctx = new MyTasksContext())
                {
                    Project project = new Project
                    {
                        Title = title,
                        UserId = Int32.Parse(Session["userid"].ToString()),
                        CreatedOn = DateTime.Now
                    };

                    ctx.Projects.Add(project);
                    ctx.SaveChanges();
                }
                return RedirectToAction("Home", "User");
            }
            catch (Exception ex)
            {
                TempData["message"] = "Não foi possível criar um novo projeto.";
                System.Web.HttpContext.Current.Trace.Write("ProjectController.Add() --> " + ex.Message);
                return RedirectToAction("Home", "User");
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                using (MyTasksContext ctx = new MyTasksContext())
                {
                    var project = (from p in ctx.Projects
                                   where p.Id == id
                                   select p).SingleOrDefault();
                    if (project != null)
                    {
                        ctx.Projects.Remove(project);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "Projeto não encontrado !";
                    }
                }
                return RedirectToAction("Home", "User");
            }
            catch (Exception ex)
            {
                TempData["message"] = "O projeto não pode ser excluído.";
                System.Web.HttpContext.Current.Trace.Write("ProjectController.Delete() --> " + ex.Message);
                return RedirectToAction("Home", "User");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                using (MyTasksContext ctx = new MyTasksContext())
                {
                    var project = (from p in ctx.Projects
                                   where p.Id == id
                                   select p).SingleOrDefault();
                    if (project != null)
                    {
                        return View(project);
                    }
                    else
                    {
                        TempData["message"] = "Projeto não encontrado !";
                    }
                }
                return RedirectToAction("Home", "User");
            }
            catch (Exception ex)
            {
                TempData["message"] = "O projeto não pode ser editado !";
                System.Web.HttpContext.Current.Trace.Write("ProjectController.Edit() --> " + ex.Message);
                return RedirectToAction("Home", "User");
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, string title)
        {
            try
            {
                using (MyTasksContext ctx = new MyTasksContext())
                {
                    var project = (from p in ctx.Projects
                                   where p.Id == id
                                   select p).SingleOrDefault();
                    if (project != null)
                    {
                        project.Title = title;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "Projeto não encontrado!";
                    }
                    return RedirectToAction("Home", "User");
                }
                
            }
            catch (Exception ex)
            {
                TempData["message"] = "O projeto não pode ser editado !";
                System.Web.HttpContext.Current.Trace.Write("ProjectController.Edit(project) --> " + ex.Message);
                return RedirectToAction("Home", "User");
            }
        }
    }
}