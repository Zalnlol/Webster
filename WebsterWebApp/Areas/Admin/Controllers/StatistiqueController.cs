using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatistiqueController : Controller
    {


        private readonly Data.ApplicationDbContext db;

        public StatistiqueController(Data.ApplicationDbContext db)
        {
            this.db = db;
        }


        public IActionResult Index()
        {

            ViewBag.res = db.Exams.ToList();
            return View();
        }

        public IActionResult Detail(string  ExamId)
        {

            ViewBag.Exam = db.Exams.SingleOrDefault(t => t.ExamId == int.Parse(ExamId));
            ViewBag.user = (from t in db.ExamUsers
                        where t.ExamId == int.Parse(ExamId)
                        join b in db.Users on
                        t.UserId equals b.Id
                        select b).ToArray();
            ViewBag.result = db.Results.Where(t => t.ExamId == int.Parse(ExamId)).ToArray();

            return View();
        }


        public IActionResult Detailuser(string ExamId, string Maill)
        {

    


            string examid = ExamId;

            var ExamUserId = db.Users.SingleOrDefault(t => t.UserName.Equals(Maill)).Id;



            var res = db.Results.SingleOrDefault(t => t.IdUser.Equals(ExamUserId) && t.ExamId == int.Parse(examid));
            string examname = db.Exams.SingleOrDefault(t => t.ExamId == int.Parse(examid)).ExamName;
            string name = db.Users.SingleOrDefault(t => t.Id.Equals(ExamUserId)).FirstName + db.Users.SingleOrDefault(t => t.Id.Equals(ExamUserId)).LastName;

            ViewBag.mail = Maill;
            ViewBag.res = res;
            ViewBag.examname = examname;
            ViewBag.name = name;



            return View();
        }


        public IActionResult Report(string ExamId)
        {

            var users = (from examuser in db.ExamUsers
                      where examuser.ExamId == int.Parse(ExamId)
                      join user in db.Users
                      on examuser.UserId equals user.Id
                      join exam in db.Exams
                      on examuser.ExamId equals exam.ExamId
                      select new
                      {
                          user.FirstName, user.LastName, user.Email,exam.ExamName, user.Id, exam.ExamId
                      }).ToList();


            var results = db.Results.Where(t => t.ExamId == int.Parse(ExamId)).ToList();


            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[13] {
                    new DataColumn("First Name"),
                    new DataColumn("Last Name"),
                    new DataColumn("Mail"),
                    new DataColumn("Exam Name"),
                    new DataColumn("General Knowledge Score"),
                    new DataColumn("General Knowledge Time"),
                    new DataColumn("Math Score"),
                    new DataColumn("Math Time"),
                    new DataColumn("Tech Score"),
                    new DataColumn("Tech Time"),
                    new DataColumn("Exam Date "),
                    new DataColumn("Is Test"),
                    new DataColumn("Is Pass")
            });

            string examname = "";

            for (int i = 0; i < users.Count; i++)
            {
                examname = users[i].ExamName;
                var dataresult = results.SingleOrDefault(t => t.IdUser == users[i].Id && t.ExamId == users[i].ExamId);

                if (dataresult != null)
                {
                    dt.Rows.Add(users[i].FirstName, 
                                users[i].LastName,
                                users[i].Email,
                                users[i].ExamName,
                                dataresult.GKScore,
                                dataresult.TimeGK,
                                dataresult.MathScore,
                                dataresult.TimeMath,
                                dataresult.TechScore,
                                dataresult.TimeTech,
                                dataresult.ExamDate,
                                true,
                                dataresult.IsPass
                                );
                }
                else
                {
                    dt.Rows.Add(users[i].FirstName,
                               users[i].LastName,
                               users[i].Email,
                               users[i].ExamName,
                               "Not Found",
                               "Not Found",
                               "Not Found",
                               "Not Found",
                               "Not Found",
                               "Not Found",
                               "Not Found",
                               true,
                              "Not Found"
                               );
                }


            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadssheetml.sheet", "Report "+ examname + ".xlsx");
                }

            }


          
        }

    }
}
