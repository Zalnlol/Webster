using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebsterWebApp.Controllers
{
    public class ExamController : Controller
    {
        private readonly Data.ApplicationDbContext db;

        public ExamController(Data.ApplicationDbContext db)
        {
            this.db = db;
        }

    
        public IActionResult ExamList(string ?notification)
        {
            ViewBag.Notification = notification;
            ViewBag.ds = db.Exams.Where(t =>t.FinishTime > DateTime.Now).ToList();
            return View();
        }


        [HttpPost]
        public IActionResult ExamCheck()
        {

            var ds = Request.Form;
            var res = db.Exams.SingleOrDefault(t => t.ExamId== int.Parse(ds["id"]) && t.ExamPass.Equals(ds["passexam"]));

            if (res==null)
            {
                return RedirectToAction("ExamList", "Exam", new { notification= "The exam password is incorrect, please try again!" });

            }
            else
            {
                DateTime CurrentTime = DateTime.Now;
                if ((CurrentTime >= res.StartDate) &&(CurrentTime<res.FinishTime) )
                {
                    HttpContext.Session.SetString("examid", res.ExamId.ToString());
                    HttpContext.Session.SetString("exampass", res.ExamPass);

                    return RedirectToAction("ExamRoom");

                }
                else
                {
                    if (CurrentTime < res.StartDate)
                    {
                        return RedirectToAction("ExamList", "Exam", new { notification = "The contest hasn't started yet, please come back later!" });
                    }else

                    if (CurrentTime >  res.FinishTime)
                    {
                        return RedirectToAction("ExamList", "Exam", new { notification = "Contest is over !" });
                    }
                }
            }

            return  BadRequest(res);
        }


        public IActionResult ExamRoom()
        {
            return View();
        }

    }
}
