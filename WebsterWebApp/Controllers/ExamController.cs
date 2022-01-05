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


    
        public IActionResult StartExam()
        {
            string type = HttpContext.Request.Query["type"];

            var id = HttpContext.Session.GetString("examid");
            var pass = HttpContext.Session.GetString("exampass");

            var res = db.Exams.SingleOrDefault(t => t.ExamId == int.Parse(id) && t.ExamPass.Equals(pass));

            if (res == null)
            {
                return RedirectToAction("ExamList", "Exam", new { notification = "The exam password is incorrect, please try again!" });

            }
            else
            {
                DateTime CurrentTime = DateTime.Now;
                if ((CurrentTime >= res.StartDate) && (CurrentTime < res.FinishTime))
                {
                   

                }
                else
                {
                    if (CurrentTime < res.StartDate)
                    {
                        return RedirectToAction("ExamList", "Exam", new { notification = "The contest hasn't started yet, please come back later!" });
                    }
                    else

                    if (CurrentTime > res.FinishTime)
                    {
                        return RedirectToAction("ExamList", "Exam", new { notification = "Contest is over !" });
                    }
                }
            }


            if (type =="1")
            {

                var questions = (from x in db.ExamTypes
                                 where x.ExamId == int.Parse(id)
                                 join question in db.Questions
                                 on x.QuestionId.ToString() equals question.QuestionId.ToString()
                                 select question
                           ).Where(t => t.Subject.Equals("General Knowledge")).ToArray();

                ViewBag.Time = db.Exams.SingleOrDefault(t => t.ExamId == int.Parse(id)).FirstCountdown.TotalSeconds;
                ViewBag.Question = questions;

                ViewBag.Anwser = (from x in questions
                                  join anwser in db.Answers
                                  on x.QuestionId.ToString() equals anwser.QuestionId.ToString()
                                  select new Areas.Admin.Models.Answer()
                                  {
                                      AnswerId = anwser.AnswerId,
                                      QuestionId = anwser.QuestionId,
                                      AnswerContent = anwser.AnswerContent,
                                      Photo = anwser.Photo
                                  }).ToArray();
            }else if (type == "2")
            {
                var questions = (from x in db.ExamTypes
                                 where x.ExamId == int.Parse(id)
                                 join question in db.Questions
                                 on x.QuestionId.ToString() equals question.QuestionId.ToString()
                                 select question
                          ).Where(t => t.Subject.Equals("Math")).ToArray();

                ViewBag.Time = db.Exams.SingleOrDefault(t => t.ExamId == int.Parse(id)).SecondCountdown.TotalSeconds;
                ViewBag.Question = questions;

                ViewBag.Anwser = (from x in questions
                                  join anwser in db.Answers
                                  on x.QuestionId.ToString() equals anwser.QuestionId.ToString()
                                  select new Areas.Admin.Models.Answer()
                                  {
                                      AnswerId = anwser.AnswerId,
                                      QuestionId = anwser.QuestionId,
                                      AnswerContent = anwser.AnswerContent,
                                      Photo = anwser.Photo
                                  }).ToArray();
            }
            else if (type == "3")
            {
                var questions = (from x in db.ExamTypes
                                 where x.ExamId == int.Parse(id)
                                 join question in db.Questions
                                 on x.QuestionId.ToString() equals question.QuestionId.ToString()
                                 select question
                          ).Where(t => t.Subject.Equals("Tech")).ToArray();

                ViewBag.Time = db.Exams.SingleOrDefault(t => t.ExamId == int.Parse(id)).FirstCountdown.TotalSeconds;
                ViewBag.Question = questions;

                ViewBag.Anwser = (from x in questions
                                  join anwser in db.Answers
                                  on x.QuestionId.ToString() equals anwser.QuestionId.ToString()
                                  select new Areas.Admin.Models.Answer()
                                  {
                                      AnswerId = anwser.AnswerId,
                                      QuestionId = anwser.QuestionId,
                                      AnswerContent = anwser.AnswerContent,
                                      Photo = anwser.Photo
                                  }).ToArray();
            }
            else
            {
                RedirectToAction("ExamRoom");
            }

            //return BadRequest(ViewBag.Anwser[1]);
            return View();

           
        }
    }
}
