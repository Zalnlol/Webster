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
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            ViewBag.selectAnswer = new List<String> {
                "FirstAnswerContent", "SecondAnswerContent", "ThirdAnswerContent", "FourthAnswerContent"
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question, String subject, IFormFile photoQuestion, String questionType, String firstAnswerContent,
        String secondAnswerContent, String thirdAnswerContent, String fourthAnswerContent, IFormFile photoFirstAnswerContent,
        IFormFile photoSecondAnswerContent, IFormFile photoThirdAnswerContent, IFormFile photoFourthAnswerContent, String IsCorrectAnswer,
        String CorrectAnswer1, String CorrectAnswer2, String CorrectAnswer3, String CorrectAnswer4)
        {
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            ViewBag.selectAnswer = new List<String>
            {
                "FirstAnswerContent", "SecondAnswerContent", "ThirdAnswerContent", "FourthAnswerContent"
            };
            if (ModelState.IsValid)
            {
                foreach (var item in TempData["optionSubject"] as List<String>)
                {
                    if(subject == item)
                    {
                        question.Subject = item;
                    }
                }
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

        public IActionResult Details(int id)
        {
            Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
            return View(question);
        }

        public IActionResult Edit(int id)
        {
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            ViewBag.selectAnswer = new List<String> {
                "FirstAnswerContent", "SecondAnswerContent", "ThirdAnswerContent", "FourthAnswerContent"
            };
            Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
            ViewBag.subject = question.Subject;
            List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(id));
            for (int i = 0; i < answers.Capacity; i++)
            {
                ViewBag.photoFirstAns = answers[0].Photo;
                ViewBag.firstAnswerContent = answers[0].AnswerContent;
                ViewBag.CorrectAnswer1 = answers[0].IsCorrectAnswer;
                ViewBag.photoSecondAns = answers[1].Photo;
                ViewBag.secondAnswerContent = answers[1].AnswerContent;
                ViewBag.CorrectAnswer2 = answers[1].IsCorrectAnswer;
                ViewBag.photoThirdAns = answers[2].Photo;
                ViewBag.thirdAnswerContent = answers[2].AnswerContent;
                ViewBag.CorrectAnswer3 = answers[2].IsCorrectAnswer;
                ViewBag.photoFourthAns = answers[3].Photo;
                ViewBag.fourthAnswerContent = answers[3].AnswerContent;
                ViewBag.CorrectAnswer4 = answers[3].IsCorrectAnswer;
            }
            ViewBag.questionType = question.QuestionType;
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, String subject, Question question, IFormFile photoQuestion, String questionType,
        String firstAnswerContent, String secondAnswerContent, String thirdAnswerContent, String fourthAnswerContent,
        IFormFile photoFirstAnswerContent, IFormFile photoSecondAnswerContent, IFormFile photoThirdAnswerContent, 
        IFormFile photoFourthAnswerContent, String isCorrectAnswer, String CorrectAnswer1, String CorrectAnswer2,
        String CorrectAnswer3, String CorrectAnswer4)
        {
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            ViewBag.selectAnswer = new List<String> 
            {
                "FirstAnswerContent", "SecondAnswerContent", "ThirdAnswerContent", "FourthAnswerContent"
            };
            List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(id));
            for (int i = 0; i < answers.Capacity; i++)
            {
                ViewBag.photoFirstAns = answers[0].Photo;
                ViewBag.firstAnswerContent = answers[0].AnswerContent;
                ViewBag.CorrectAnswer1 = answers[0].IsCorrectAnswer;
                ViewBag.photoSecondAns = answers[1].Photo;
                ViewBag.secondAnswerContent = answers[1].AnswerContent;
                ViewBag.CorrectAnswer2 = answers[1].IsCorrectAnswer;
                ViewBag.photoThirdAns = answers[2].Photo;
                ViewBag.thirdAnswerContent = answers[2].AnswerContent;
                ViewBag.CorrectAnswer3 = answers[2].IsCorrectAnswer;
                ViewBag.photoFourthAns = answers[3].Photo;
                ViewBag.fourthAnswerContent = answers[3].AnswerContent;
                ViewBag.CorrectAnswer4 = answers[3].IsCorrectAnswer;
            }
            Question _question = _db.Questions.SingleOrDefault(_q => _q.QuestionId.Equals(id));
            if (_question != null)
            {
                if (ModelState.IsValid)
                {
                    _question.Subject = subject;
                    _question.QuestionTitle = question.QuestionTitle;
                    if (photoQuestion != null)
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoQuestion.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoQuestion.CopyToAsync(stream);
                        _question.Photo = $"images/QA/{photoQuestion.FileName}";
                    }
                    else
                    {
                        _question.Photo = _db.Questions.SingleOrDefault(_q => _q.QuestionId.Equals(id)).Photo;
                    }
                    if (questionType == "true")
                    {
                        _question.QuestionType = true;
                    }
                    else
                    {
                        _question.QuestionType = false;
                    }
                    await _db.SaveChangesAsync();

                    for (int i = 0; i < answers.Capacity; i++)
                    {
                        answers[0].AnswerContent = firstAnswerContent;
                        if(photoFirstAnswerContent != null)
                        {
                            var filePath = Path.Combine("wwwroot/images/QA", photoFirstAnswerContent.FileName);
                            var stream = new FileStream(filePath, FileMode.Create);
                            await photoFirstAnswerContent.CopyToAsync(stream);
                            answers[0].Photo = $"images/QA/{photoFirstAnswerContent.FileName}";
                        }
                        else
                        {
                            answers[0].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[0].AnswerId)).Photo;                           
                        }
                        if (questionType == "true")
                        {
                            if(isCorrectAnswer == "FirstAnswerContent")
                            {
                                answers[0].IsCorrectAnswer = true;
                                answers[1].IsCorrectAnswer = false;
                                answers[2].IsCorrectAnswer = false;
                                answers[3].IsCorrectAnswer = false;
                            }
                            else
                            {
                                answers[0].IsCorrectAnswer = false;
                            }
                        }
                        else
                        {
                            if(CorrectAnswer1 == "true")
                            {
                                answers[0].IsCorrectAnswer = true;
                            }
                            else
                            {
                                answers[0].IsCorrectAnswer = false;
                            }
                        }
                        

                        answers[1].AnswerContent = secondAnswerContent;
                        if (photoSecondAnswerContent != null)
                        {
                            var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswerContent.FileName);
                            var stream = new FileStream(filePath, FileMode.Create);
                            await photoSecondAnswerContent.CopyToAsync(stream);
                            answers[1].Photo = $"images/QA/{photoSecondAnswerContent.FileName}";
                        }
                        else
                        {
                            answers[1].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[1].AnswerId)).Photo;                         
                        }
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "SecondAnswerContent")
                            {
                                answers[0].IsCorrectAnswer = false;
                                answers[1].IsCorrectAnswer = true;
                                answers[2].IsCorrectAnswer = false;
                                answers[3].IsCorrectAnswer = false;
                            }
                            else
                            {
                                answers[1].IsCorrectAnswer = false;
                            }
                        }
                        else
                        {
                            if (CorrectAnswer2 == "true")
                            {
                                answers[1].IsCorrectAnswer = true;
                            }
                            else
                            {
                                answers[1].IsCorrectAnswer = false;
                            }
                        }

                        answers[2].AnswerContent = thirdAnswerContent;
                        if (photoThirdAnswerContent != null)
                        {
                            var filePath = Path.Combine("wwwroot/images/QA", photoThirdAnswerContent.FileName);
                            var stream = new FileStream(filePath, FileMode.Create);
                            await photoThirdAnswerContent.CopyToAsync(stream);
                            answers[2].Photo = $"images/QA/{photoThirdAnswerContent.FileName}";
                        }
                        else
                        {
                            answers[2].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[2].AnswerId)).Photo;                           
                        }
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "ThirdAnswerContent")
                            {
                                answers[0].IsCorrectAnswer = false;
                                answers[1].IsCorrectAnswer = false;
                                answers[2].IsCorrectAnswer = true;
                                answers[3].IsCorrectAnswer = false;
                            }
                            else
                            {
                                answers[2].IsCorrectAnswer = false;
                            }
                        }
                        else
                        {
                            if (CorrectAnswer3 == "true")
                            {
                                answers[2].IsCorrectAnswer = true;
                            }
                            else
                            {
                                answers[2].IsCorrectAnswer = false;
                            }
                        }

                        answers[3].AnswerContent = fourthAnswerContent;
                        if (photoFourthAnswerContent != null)
                        {
                            var filePath = Path.Combine("wwwroot/images/QA", photoFourthAnswerContent.FileName);
                            var stream = new FileStream(filePath, FileMode.Create);
                            await photoFourthAnswerContent.CopyToAsync(stream);
                            answers[3].Photo = $"images/QA/{photoFourthAnswerContent.FileName}";
                        }
                        else
                        {
                            answers[3].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[3].AnswerId)).Photo;                       
                        }
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "FourthAnswerContent")
                            {
                                answers[0].IsCorrectAnswer = false;
                                answers[1].IsCorrectAnswer = false;
                                answers[2].IsCorrectAnswer = false;
                                answers[3].IsCorrectAnswer = true;
                            }
                            else
                            {
                                answers[3].IsCorrectAnswer = false;
                            }
                        }
                        else
                        {
                            if (CorrectAnswer4 == "true")
                            {
                                answers[3].IsCorrectAnswer = true;
                            }
                            else
                            {
                                answers[3].IsCorrectAnswer = false;
                            }
                        }
                    }
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return BadRequest("not found question!");
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
                List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(question.QuestionId));
                for (int i = 0; i < answers.Capacity; i++)
                {
                    _db.Answers.Remove(answers[i]);
                    await _db.SaveChangesAsync();
                }
                _db.Questions.Remove(question);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
