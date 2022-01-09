using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

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
            
            var exam = db.Exams.Where(t =>t.FinishTime > DateTime.Now).ToList();

            var ExamUserId = db.Users.SingleOrDefault(t => t.UserName.Equals(HttpContext.Session.GetString("Mail"))).Id;

            var list = db.ExamUsers.Where(s => s.UserId.Equals(ExamUserId)).ToList();


            ViewBag.ds = (from s in list
                      join t in exam
                      on s.ExamId.ToString() equals t.ExamId.ToString()
                      select t
                      ).ToList();




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

           ;
            var quess = ViewBag.Question;
            var anss = ViewBag.Anwser;
            string[,] arr = new string[4, 2];

       

            List<Models.QuestionData> ListQues = new List<Models.QuestionData>();

            for (int i = 0; i <= 4; i++)
            {
                Models.QuestionData Ques = new Models.QuestionData();
                Ques.QuesionID = quess[i].QuestionId;
                Ques.QuestionTilte = quess[i].QuestionTitle;
                Ques.QuestionType = quess[i].QuestionType;

   

                for (int j = 0; j < anss.Length; j++)
                {
                    if (anss[j].QuestionId == quess[i].QuestionId)
                    {
                        Models.AnwserData Ans = new Models.AnwserData();
                        Ans.AnwserId = anss[j].AnswerId;
                        Ans.AnwserName = anss[j].AnswerContent;
                        Ans.Ischoose = false;
                        Ques.Anwser.Add(Ans);
                    }
                }
                ListQues.Add(Ques);

            }

            ViewBag.ListQues = ListQues;


            return View();

           
        }
    
        [HttpPost]
        public object CheckResult(string data)
        {
            var json = Request.Form;


            //var res = JsonConvert.DeserializeObject<
            //      List<Models.QuestionData>>(data);


            //JObject json1 = JObject.Parse(data);

            List<Models.QuestionData> ListQues = new List<Models.QuestionData>();
          
          


            for (int i=0;i<=4;i++)
            {
                Models.QuestionData Ques = new Models.QuestionData();
                Ques.QuesionID = int.Parse(json["data[" + i + "][QuesionID]"]);
                Ques.QuestionTilte = json["data[" + i + "][QuestionTilte]"];
                Ques.QuestionType = bool.Parse(json["data[" + i + "][QuestionType]"]);

                for (int j = 0; j <=3; j++)
                {
                    Models.AnwserData Ans = new Models.AnwserData();
                    Ans.AnwserId = int.Parse(json["data[" + i + "][Anwser][" + j + "][AnwserId]"]);
                    Ans.AnwserName = json["data[" + i + "][Anwser][" + j + "][AnwserName]"];
                    Ans.Ischoose = bool.Parse(json["data[" + i + "][Anwser][" + j + "][Ischoose]"]);
                    Ques.Anwser.Add(Ans);


                }
                ListQues.Add(Ques);

            }


            var anwsersystem = db.Answers.ToList();
            var tmp = true;
            int mark = 0;

            foreach (var item in ListQues)
            {
                var anwsersystem1 = anwsersystem.Where(t => t.QuestionId == item.QuesionID);

                for (int i = 0;  i <= 3; i++ )
                {

                    var question1 = anwsersystem1.SingleOrDefault(t => t.AnswerId == item.Anwser[i].AnwserId);
                    if (question1 !=null)
                    {
                        if (question1.IsCorrectAnswer == item.Anwser[i].Ischoose )
                        {

                        }
                        else
                        {
                            
                            tmp = false;
                        }
                    }
                   

                }
                if (tmp==true)
                {
                    mark += 1;
                }
                tmp = true;

            }

           

            //return json["data[0][QuesionID]"];
            return mark;



        }

    }
}
