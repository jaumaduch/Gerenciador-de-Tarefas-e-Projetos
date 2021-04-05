using MyTasks.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyTasks.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        [HttpGet]
        public ActionResult Add(int id)
        {
            TaskModel model = new TaskModel();
            model.ProjectId = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(TaskModel model)
        {
            TempData["message"] = "";

            if (ModelState.IsValid)
            {
                try
                {

                    Task task = new Task
                    {
                        Title = model.Title,
                        AddedOn = DateTime.Now,
                        Importance = model.Importance,
                        Status = model.Status,
                        ProjectId = model.ProjectId
                    };

                    if (model.DueDate != null &&  model.DueDate.Length > 0)
                        task.DueDate = DateTime.Parse(model.DueDate);
                    else
                        task.DueDate = null;

                    using (MyTasksContext ctx = new MyTasksContext())
                    {
                        ctx.Tasks.Add(task);
                        ctx.SaveChanges();
                        TempData["message"] = "A tarefa foi adicionada com sucesso !";
                    }
                    return RedirectToAction("Add");

                }
                catch (Exception ex)
                {
                    TempData["message"] = "A tarefa não pode ser adicionada.";
                    System.Web.HttpContext.Current.Trace.Write("TaskController.Add() --> " + ex.Message);
                    return View(model);
                }
            }
            else
            {
                TempData["message"] = "Informe um valor válido !";
                return View(model);
            }

        } // Add

        [HttpGet]
        public ActionResult List(int id)
        {

            // get list of tasks in a project
            using (MyTasksContext ctx = new MyTasksContext())
            {
                var project =(from p in ctx.Projects
                              where p.Id == id
                              select p).SingleOrDefault();

                ViewBag.ProjectTitle = project.Title;

                var tasks = from t in ctx.Tasks.ToList<Task>()
                            where t.ProjectId == id
                            select t;

                return View(tasks);
            }
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MyTasksContext ctx = new MyTasksContext())
            {
                var task = (from t in ctx.Tasks
                            where t.Id == id
                            select t).SingleOrDefault();

                var projectid = task.ProjectId;
                ctx.Tasks.Remove(task);
                ctx.SaveChanges();

                return RedirectToAction("List", new { Id = projectid });
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            using (MyTasksContext ctx = new MyTasksContext())
            {
                var task = (from t in ctx.Tasks.Include("Project")
                            where t.Id == id
                            select t).SingleOrDefault();

                return View(task);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (MyTasksContext ctx = new MyTasksContext())
            {
                var task = (from t in ctx.Tasks
                            where t.Id == id
                            select t).SingleOrDefault();

                return View(task);
            }
        }

        [HttpPost]
        public ActionResult Edit(Task newTask)
        {
            using (MyTasksContext ctx = new MyTasksContext())
            {
                var task = (from t in ctx.Tasks
                            where t.Id == newTask.Id
                            select t).SingleOrDefault();

                try
                {
                    task.Title = newTask.Title;
                    task.DueDate = newTask.DueDate;
                    task.Importance = newTask.Importance;
                    task.Status = newTask.Status;

                    ctx.SaveChanges();

                    TempData["message"] = "A tarefa foi atualizada com sucesso !";
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Trace.Write("TaskController.Edit() --> " + ex.Message);
                    TempData["message"] = "A tarefa não foi atualizada !";
                }

                return View(task);
            }// using 
        } // Edit 

        [HttpGet]
        public ActionResult SearchForm()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Search(string title)
        {
            using (MyTasksContext ctx = new MyTasksContext())
            {
                var tasks = from t in ctx.Tasks.Include ("Project").ToList<Task>()
                            where t.Project.UserId ==  Int32.Parse( Session["userid"].ToString ())
                                        &&  t.Title.ToUpper().Contains(title.ToUpper())
                            orderby t.DueDate
                            select t;

                return PartialView("SearchResults", tasks);
            }
        }

    }
}