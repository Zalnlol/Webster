using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class QuestionController : Controller
    {
        private readonly Data.ApplicationDbContext _db;

        public QuestionController(Data.ApplicationDbContext db)
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
        IFormFile photoSecondAnswerContent, IFormFile photoThirdAnswerContent, IFormFile photoFourthAnswerContent, String IsCorrectAnswer,
        String CorrectAnswer1, String CorrectAnswer2, String CorrectAnswer3, String CorrectAnswer4)
        {
            ViewBag.selectAnswer = new List<String> {
                "FirstAnswerContent", "SecondAnswerContent", "ThirdAnswerContent", "FourthAnswerContent"
            };
            if (ModelState.IsValid)
            {
                if (photoQuestion != null)
                {
                    var filePath = Path.Combine("wwwroot/images/QA", photoQuestion.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoQuestion.CopyToAsync(stream);
                    question.Photo = $"images/QA/{photoQuestion.FileName}";
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
                firstAnswer.QuestionId = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(question.QuestionId)).QuestionId;
                firstAnswer.AnswerContent = firstAnswerContent;
                if (photoFirstAnswerContent != null)
                {
                    var filePath = Path.Combine("wwwroot/images/QA", photoFirstAnswerContent.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoFirstAnswerContent.CopyToAsync(stream);
                    firstAnswer.Photo = $"images/QA/{photoFirstAnswerContent.FileName}";
                }
                if (IsCorrectAnswer == "FirstAnswerContent" || CorrectAnswer1 == "true")
                {
                    firstAnswer.IsCorrectAnswer = true;
                }
                else
                {
                    firstAnswer.IsCorrectAnswer = false;
                }
                _db.Answers.Add(firstAnswer);
                await _db.SaveChangesAsync();

                Answer secondAnswer = new Answer();
                secondAnswer.QuestionId = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(question.QuestionId)).QuestionId;
                secondAnswer.AnswerContent = secondAnswerContent;
                if (photoFirstAnswerContent != null)
                {
                    var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswerContent.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoSecondAnswerContent.CopyToAsync(stream);
                    secondAnswer.Photo = $"images/QA/{photoSecondAnswerContent.FileName}";
                }
                if (IsCorrectAnswer == "SecondAnswerContent" || CorrectAnswer2 == "true")
                {
                    secondAnswer.IsCorrectAnswer = true;
                }
                else
                {
                    secondAnswer.IsCorrectAnswer = false;
                }
                _db.Answers.Add(secondAnswer);
                await _db.SaveChangesAsync();

                Answer thirdAnswer = new Answer();
                thirdAnswer.QuestionId = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(question.QuestionId)).QuestionId;
                thirdAnswer.AnswerContent = thirdAnswerContent;
                if (photoFirstAnswerContent != null)
                {
                    var filePath = Path.Combine("wwwroot/images/QA", photoThirdAnswerContent.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoThirdAnswerContent.CopyToAsync(stream);
                    thirdAnswer.Photo = $"images/QA/{photoThirdAnswerContent.FileName}";
                }
                if (IsCorrectAnswer == "ThirdAnswerContent" || CorrectAnswer3 == "true")
                {
                    thirdAnswer.IsCorrectAnswer = true;
                }
                else
                {
                    thirdAnswer.IsCorrectAnswer = false;
                }
                _db.Answers.Add(thirdAnswer);
                await _db.SaveChangesAsync();

                Answer fourthAnswer = new Answer();
                fourthAnswer.QuestionId = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(question.QuestionId)).QuestionId;
                fourthAnswer.AnswerContent = fourthAnswerContent;
                if (photoFirstAnswerContent != null)
                {
                    var filePath = Path.Combine("wwwroot/images/QA", photoFourthAnswerContent.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoFourthAnswerContent.CopyToAsync(stream);
                    fourthAnswer.Photo = $"images/QA/{photoFourthAnswerContent.FileName}";
                }
                if (IsCorrectAnswer == "FourthAnswerContent" || CorrectAnswer4 == "true")
                {
                    fourthAnswer.IsCorrectAnswer = true;
                }
                else
                {
                    fourthAnswer.IsCorrectAnswer = false;
                }
                _db.Answers.Add(fourthAnswer);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
