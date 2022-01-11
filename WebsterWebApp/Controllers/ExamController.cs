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


            var ds1 = (from s in list
                      join t in exam
                      on s.ExamId.ToString() equals t.ExamId.ToString()
                      select t
                      ).ToList();
            List<Models.Exam> examuser = new List<Models.Exam>();

            foreach (var item in ds1)
            {
                var t = db.Results.SingleOrDefault(t => t.ExamId == item.ExamId && t.IdUser.Equals(ExamUserId) && t.TimeTech !=0);

                if (t!=null)
                {
                  
                }
                else
                {
                    examuser.Add(item);
                }

            }

            ViewBag.ds = examuser;
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

            var ExamUserId = db.Users.SingleOrDefault(t => t.UserName.Equals(HttpContext.Session.GetString("Mail"))).Id;
            var examid = HttpContext.Session.GetString("examid");
            var res = db.Results.SingleOrDefault(t =>t.IdUser.Equals(ExamUserId) && t.ExamId ==  int.Parse(examid));
            if (res ==null)
            {
               

                Models.ResultsModel results = new Models.ResultsModel();
                results.ExamId = int.Parse(examid);
                results.IdUser = ExamUserId;
                results.ExamDate = DateTime.Now;

                 db.Results.Add(results);
                 db.SaveChanges();
                 HttpContext.Session.SetString("count", "0");
                ViewBag.count = HttpContext.Session.GetString("count");


            }
            else
            {
                if (HttpContext.Session.GetString("count") ==null)
                {

                    if (res.TimeGK==0)
                    {

                        HttpContext.Session.SetString("count", "0");
                        ViewBag.count = HttpContext.Session.GetString("count");
                    }else
                    if (res.TimeMath == 0)
                    {
                        HttpContext.Session.SetString("count", "1");
                        ViewBag.count = HttpContext.Session.GetString("count");
                    }else
                    {
                        HttpContext.Session.SetString("count", "2");
                        ViewBag.count = HttpContext.Session.GetString("count");
                    }

                }
                else
                {
                    ViewBag.count = HttpContext.Session.GetString("count");

                }

            }

          

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
            mark *=  10;

            var ExamUserId11 = db.Users.SingleOrDefault(t => t.UserName.Equals(HttpContext.Session.GetString("Mail"))).Id;
            var examid11 = HttpContext.Session.GetString("examid");
            var res111 = db.Results.SingleOrDefault(t => t.IdUser.Equals(ExamUserId11) && t.ExamId == int.Parse(examid11));




            int countnumber =int.Parse(HttpContext.Session.GetString("count"));

 

            if (res111 != null)
            {
                if (countnumber == 0)
                {
                    res111.GKScore = mark;
                    res111.TimeGK = int.Parse(json["time"]);
                    db.SaveChanges();
                }
                if (countnumber == 1)
                {
                    res111.MathScore = mark;
                    res111.TimeMath = int.Parse(json["time"]);
                    db.SaveChanges();
                }
                if (countnumber == 2)
                {
                    double pass = Math.Round((res111.GKScore + res111.MathScore + res111.TechScore) * 1.5);

                    var examm = db.Exams.SingleOrDefault(t => t.ExamId == int.Parse(examid11));

                    if (pass>= examm.PassPercent )
                    {
                        res111.IsPass = true;
                    }
                    else
                    {
                        res111.IsPass = false;

                    }

                    res111.TechScore = mark;
                    res111.TimeTech = int.Parse(json["time"]);
                    db.SaveChanges();
                }
            }

            countnumber += 1;
            if (countnumber == 3)
            {
                HttpContext.Session.Remove("count");
            }

            HttpContext.Session.SetString("count", countnumber.ToString());

           

            //return json["data[0][QuesionID]"];
            return mark;



        }


        public IActionResult ExamResult(string ? idexam)
        {
            string examid = "";
            if (HttpContext.Session.GetString("examid") != null)
            {
                 examid = HttpContext.Session.GetString("examid");
            }
            else
            {
                 examid = idexam;

            }

            var ExamUserId = db.Users.SingleOrDefault(t => t.UserName.Equals(HttpContext.Session.GetString("Mail"))).Id;
           


            var res = db.Results.SingleOrDefault(t => t.IdUser.Equals(ExamUserId) && t.ExamId == int.Parse(examid));
            string examname = db.Exams.SingleOrDefault(t =>t.ExamId== int.Parse(examid)).ExamName;
            string name = db.Users.SingleOrDefault(t => t.Id.Equals(ExamUserId)).FirstName + db.Users.SingleOrDefault(t => t.Id.Equals(ExamUserId)).LastName;

            ViewBag.mail = HttpContext.Session.GetString("Mail");
            ViewBag.res = res;
            ViewBag.examname = examname;
            ViewBag.name = name;



            return View();
        }


        public IActionResult ExamHistoryList()
        {
       

            var exam = db.Exams.ToList();

            var ExamUserId = db.Users.SingleOrDefault(t => t.UserName.Equals(HttpContext.Session.GetString("Mail"))).Id;

            var list = db.ExamUsers.Where(s => s.UserId.Equals(ExamUserId)).ToList();


            var ds1 = (from s in list
                       join t in exam
                       on s.ExamId.ToString() equals t.ExamId.ToString()
                       select t
                      ).ToList();
            List<Models.Exam> examuser = new List<Models.Exam>();
            List<Models.ResultsModel> result = new List<Models.ResultsModel>();


            foreach (var item in ds1)
            {
                var t = db.Results.SingleOrDefault(t => t.ExamId == item.ExamId && t.IdUser.Equals(ExamUserId) && t.TimeTech != 0);

                if (t != null)
                {
                    examuser.Add(item);
                    result.Add(t);
                }
                else
                {
                }

            }

            ViewBag.ds = examuser.ToArray();
            ViewBag.ds1 = result.ToArray();


            return View();
        }
    }
}
