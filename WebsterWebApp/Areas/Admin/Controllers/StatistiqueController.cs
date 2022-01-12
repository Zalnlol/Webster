using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    }
}
