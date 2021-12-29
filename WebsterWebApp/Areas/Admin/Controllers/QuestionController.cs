using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using WebsterWebApp.Areas.Admin.Models;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        private readonly Data.DatabaseContext _db;

        public QuestionController(Data.DatabaseContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Questions.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.selectAnswer = new List<String> {
                "FirstAnswerContent", "SecondAnswerContent", "ThirdAnswerContent", "FourthAnswerContent"
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question, IFormFile photoQuestion, String questionType, String firstAnswerContent,
        String secondAnswerContent, String thirdAnswerContent, String fourthAnswerContent, IFormFile photoFirstAnswerContent,
        IFormFile photoSecondAnswerContent, IFormFile photoThirdAnswerContent, IFormFile photoFourthAnswerContent)
        {
            ViewBag.selectAnswer = new List<String> {
                "FirstAnswerContent", "SecondAnswerContent", "ThirdAnswerContent", "FourthAnswerContent"
            };
            if (ModelState.IsValid)
            {
                if (photoQuestion != null)
                {
                    var filePath = Path.Combine("wwwroot/images", photoQuestion.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoQuestion.CopyToAsync(stream);
                    question.Photo = $"images/{photoQuestion.FileName}";
                   
                }
                if(questionType == "true")
                {
                    question.QuestionType = true;
                }
                else
                {
                    question.QuestionType = false;
                }
                _db.Questions.Add(question);
                await _db.SaveChangesAsync();

                Answer firstAnswer = new Answer();
                firstAnswer.QuestionId = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(question)).QuestionId;
                firstAnswer.AnswerContent = firstAnswerContent;
                if (photoFirstAnswerContent != null)
                {
                    var filePath = Path.Combine("wwwroot/images", photoFirstAnswerContent.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoFirstAnswerContent.CopyToAsync(stream);
                    firstAnswer.Photo = $"images/{photoFirstAnswerContent.FileName}";
                }
                

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
