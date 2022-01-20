using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsterWebApp.Data;
using WebsterWebApp.Models;
using Newtonsoft.Json;
using WebsterWebApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    [Authorize(Roles = "Admin")]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository.IMailService _mailService;

        public ExamController(ApplicationDbContext context, Repository.IMailService _mailService)
        {
            _context = context;
            this._mailService = _mailService;
        }
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("msg") != null)
            {
                ViewBag.msg = HttpContext.Session.GetString("msg");
            }
            return View(_context.Exams.ToList());
        }
        public IActionResult Create()
        {

            return View();
        }
        public IActionResult Edit(int id)
        {
            var res = _context.Exams.Find(id);
            return View(res);
        }
        [HttpPost]
        public IActionResult Edit(Exam exam)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    var ex = _context.Exams.Find(exam.ExamId);
                    ex.ExamName = exam.ExamName;
                    ex.PassPercent = exam.PassPercent;
                    ex.FinishTime = exam.FinishTime;
                    ex.ThirdCountdown = exam.ThirdCountdown;
                    ex.SecondCountdown = exam.SecondCountdown;
                    ex.StartDate = exam.StartDate;
                    ex.FinishTime = exam.FinishTime;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Exam exam)
        {
            
            var result = _context.Exams.SingleOrDefaultAsync(s => s.ExamName.Equals(exam.ExamName)).Result;
            var ques1 = (from c in _context.Questions where c.Subject.Equals("General Knowledge") select c).ToList();
            var ques2 = (from c in _context.Questions where c.Subject.Equals("Math") select c).ToList();
            var ques3 = (from c in _context.Questions where c.Subject.Equals("Tech") select c).ToList();
            
        
            try
            {
                if (ModelState.IsValid && result == null)
                {
                    List<char> abc = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'v', 'w', 'y', 'z' };
                    Random pass = new Random();
                    string pwd = "";
                    for (int i = 1; i <= 6; i++)
                    {
                        int pas = pass.Next((abc.Count) - 1);
                        char cha = abc.ElementAt(pas);
                        pwd = pwd + cha.ToString();
                    }
                    if (exam.ExamType == true)
                    {
                        exam.ExamPass = pwd;  
                        await _context.Exams.AddAsync(exam);
                        await _context.SaveChangesAsync();
                        var des = _context.Exams.OrderByDescending(S => S.ExamId).First();
                        List<Question> list1 = ques1;
                        List<Question> list2 = ques2;
                        List<Question> list3 = ques3;
                        Random random = new Random();

                        for (int i = 0; i < 5; i++)
                        {                         
                            int random1 = random.Next((list1.Count) - 1);
                            Question k1 = list1.ElementAt(random1);
                            ExamType _exam = new ExamType();
                            _exam.ExamId = des.ExamId;
                            _exam.QuestionId = k1.QuestionId;
                            await _context.ExamTypes.AddAsync(_exam);
                            await _context.SaveChangesAsync();
                            list1.Remove(k1);                      
                        }

                        for (int i = 0; i < 5; i++)
                        {
                            int random2 = random.Next((list2.Count) - 1);
                            Question k2 = list2.ElementAt(random2);
                            ExamType _exam = new ExamType();
                            _exam.ExamId = des.ExamId;

                            _exam.QuestionId = k2.QuestionId;
                            await _context.ExamTypes.AddAsync(_exam);
                            await _context.SaveChangesAsync();
                            list2.Remove(k2);
                            
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            int random3 = random.Next(list3.Count - 1);
                            Question k3 = list3.ElementAt(random3);
                            ExamType _exam = new ExamType();
                            _exam.ExamId = des.ExamId;
                            _exam.QuestionId = k3.QuestionId;
                            await _context.ExamTypes.AddAsync(_exam);
                            await _context.SaveChangesAsync();
                            list3.Remove(k3);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        exam.ExamPass = pwd;
                        await _context.Exams.AddAsync(exam);
                        await _context.SaveChangesAsync();
                        //var des = _context.Exams.OrderByDescending(S => S.ExamId).First();
                        //return Redirect(Url.Action("CreateExamType", "Exam", new { id = exam.ExamId }));
                        return RedirectToAction("CreateExamType", "Exam", new { id = exam.ExamId });
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        public IActionResult CreateExamType(int id)

        {
            ViewBag.index = (from c in _context.Questions where c.Subject.Equals("General Knowledge") select c).ToList();
            ViewBag.index1 = (from c in _context.Questions where c.Subject.Equals("Mathematics") select c).ToList();
            ViewBag.index2 = (from c in _context.Questions where c.Subject.Equals("Technical") select c).ToList();
            var res = _context.Exams.SingleOrDefault(c => c.ExamId.Equals(id));
            ViewBag.idex = res.ExamId;
            ViewBag.nameex = res.ExamName;
            return View();
        }
        [HttpPost ]
        public  IActionResult CreateExamType()
        {

            var res = Request.Form;
            
            for (int i = 1; i <= 15; i++)
            {
                ExamType ext = new ExamType();
                ext.ExamId = int.Parse(res["examid"]);
                ext.QuestionId = int.Parse(res["ques" + i]);
                _context.ExamTypes.Add(ext);
                _context.SaveChanges();
            }
            return RedirectToAction("AddUser", "Exam", new { id= int.Parse(res["examid"]) });

            return View();
        }
        public IActionResult Details(int id)
        {   var des = from c in _context.ExamTypes join o in _context.Questions on c.QuestionId equals o.QuestionId where (c.ExamId == id && o.Subject=="Math") select o.QuestionTitle;
            var des2 = from c in _context.ExamTypes join o in _context.Questions on c.QuestionId equals o.QuestionId where (c.ExamId == id && o.Subject == "General Knowledge") select o.QuestionTitle;
            var des3 = from c in _context.ExamTypes join o in _context.Questions on c.QuestionId equals o.QuestionId where (c.ExamId == id && o.Subject == "Tech") select o.QuestionTitle;
            var res = _context.Exams.SingleOrDefault(s => s.ExamId.Equals(id));
            ViewBag.math = des.ToList();
            ViewBag.kn = des2.ToList();
            ViewBag.tech = des3.ToList();

            return View(res);
        }   
        public IActionResult UserList(int id)
        {
            var s = from c in _context.ExamUsers where c.ExamId.Equals(id) select c;
            var res = _context.Exams.SingleOrDefault(c => c.ExamId.Equals(id));
            ViewBag.id = res.ExamId;
            //return BadRequest(ViewBag.id);
            return View(s.ToList());
        }
        public IActionResult RemoveUser(int id)
        {   var res = _context.ExamUsers.SingleOrDefault(s=>s.ExamUserId == id);
            _context.ExamUsers.Remove(res);
            _context.SaveChanges();
            return View();
        }
        public IActionResult AddUser(int id)
        {
            var r = _context.Exams.SingleOrDefault(c => c.ExamId.Equals(id));
            var s = (from c in _context.ExamUsers where c.ExamId.Equals(id) select c).ToList();
            var list = (from c in _context.Users  select c).ToList();
            List<ApplicationUser> es = list;
            foreach (var item in s)
            {
                for (int i = 0; i < es.Count; i++)
                {   var sa =es.ElementAt(i);
                    if (sa.Id==item.UserId)
                    {
                        es.Remove(sa);
                    };
                }
            }
            ViewBag.exam = r.ExamId;

            var userlist = _context.UserRoles.Where(t => t.RoleId.Equals("de5167aa-1f3f-454d-b6ca-6407d8ab0dbb"));
            var res = from e in es
                      join us in userlist
                      on e.Id equals us.UserId
                      select e;
                     

            //return BadRequest(r);
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser()
        {
            WebsterWebApp.TemplateMail.Template template = new TemplateMail.Template();
            var res = Request.Form;
            List<string> s = res["id"].ToList();
            foreach (var item in s)
            {
                ExamUser exu = new ExamUser();
                exu.ExamId = int.Parse(res["examid"]);
                exu.UserId = item;
                _context.ExamUsers.Add(exu);
                _context.SaveChanges();

                string fullname = _context.Users.SingleOrDefault(t => t.Id.Equals(item)).FirstName + _context.Users.SingleOrDefault(t => t.Id.Equals(item)).LastName;
                string mail = _context.Users.SingleOrDefault(t => t.Id.Equals(item)).Email ;

                var exam = _context.Exams.SingleOrDefault(t => t.ExamId == int.Parse(res["examid"]));

                await _mailService.SendMail(mail, "Information Exam", template.sendexam(fullname, exam.ExamName, exam.ExamPass, exam.StartDate.ToString()));


            }

            return RedirectToAction("Index");
        
        }

        public async Task<IActionResult> Delete(int id)
        {
            Exam ex = _context.Exams.Find(id);
            List<ExamType> examTypeList = _context.ExamTypes.Where(examType => examType.ExamId == ex.ExamId).ToList();
            List<ExamUser> users = _context.ExamUsers.Where(user => user.ExamId.Equals(id)).ToList();
            List<ResultsModel> results = _context.Results.Where(result => result.ExamId == id).ToList();
            if (ex != null)
            {
                if(results.Count == 0)
                {
                    if(users.Count > 0)
                    {
                        for (int i = 0; i < users.Count; i++)
                        {
                            _context.ExamUsers.Remove(users[i]);
                            await _context.SaveChangesAsync();
                        }
                    }
                    if (examTypeList.Count > 0)
                    {
                        for (int i = 0; i < examTypeList.Count; i++)
                        {
                            _context.ExamTypes.Remove(examTypeList[i]);
                            await _context.SaveChangesAsync();
                        }
                    }
                    _context.Exams.Remove(ex);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("msg", "Delete sucessfully!");
                }
                else
                {
                    HttpContext.Session.SetString("msg", "Delete fail!");
                }
            }
            else
            {
                return BadRequest("not found!");
            }
            return RedirectToAction("Index");
        }
    }
}
