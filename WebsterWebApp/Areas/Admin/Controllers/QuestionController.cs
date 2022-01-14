using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public IActionResult IndexCharts()
        {
            int gkQuantity = _db.Questions.Where(q => q.Subject == "General Knowledge").Count();
            int mathQuantity = _db.Questions.Where(q => q.Subject == "Math").Count();
            int techQuantity = _db.Questions.Where(q => q.Subject == "Tech").Count();
            List<int> data = new List<int> { gkQuantity, mathQuantity, techQuantity };
            return  Json(new { JsonList = data });
        }

        public IActionResult Index(String subject, String questionTitle)
        {
            var model = _db.Questions.ToList();
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            if (String.IsNullOrEmpty(subject) && String.IsNullOrEmpty(questionTitle))
            {
                return View(model);
            }
            else if (!String.IsNullOrEmpty(subject) && String.IsNullOrEmpty(questionTitle))
            {
                ViewBag.optionSubject = subject;
                model = model.ToList().FindAll(m => m.Subject.ToUpper().Contains(subject.ToUpper()));
                return View(model);
            }
            else if(String.IsNullOrEmpty(subject) && !String.IsNullOrEmpty(questionTitle))
            {
                model = model.ToList().FindAll(m => m.QuestionTitle.ToUpper().Contains(questionTitle.ToUpper()));
                return View(model);
            }
            else
            {
                ViewBag.optionSubject = subject;
                model = model.ToList().FindAll(m => m.Subject.ToUpper().Contains(subject.ToUpper()) 
                && m.QuestionTitle.ToUpper().Contains(questionTitle.ToUpper()));
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult AutoComplete(String prefix)
        {
            var questions = (from question in _db.Questions
                             where question.QuestionTitle.StartsWith(prefix)
                             select new
                             {
                                 label = question.QuestionTitle,
                             }).ToList();

            return Json(questions);
        }

        public IActionResult BatchQuestionUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BatchQuestionUpload(IFormFile batchQuestions)
        {
            if (batchQuestions?.Length > 0)
            {
                var stream = batchQuestions.OpenReadStream();
                try
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                        int rowCount = worksheet.Dimension.Rows;
                        String tmp = "0";
                        List<Question> questions = new List<Question>();
                        List<Answer> answers = new List<Answer>();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            tmp = "0";
                            String subject = worksheet.Cells[row, 1].Value?.ToString();
                            String title = worksheet.Cells[row, 2].Value?.ToString();
                            String photoQuestion = worksheet.Cells[row, 3].Value?.ToString();
                            String type = worksheet.Cells[row, 4].Value?.ToString();

                            if (subject == null || title == null || type == null)
                            {
                                tmp = "1";
                            }

                            Question question = new Question()
                            {
                                Subject = subject,
                                QuestionTitle = title,
                                Photo = photoQuestion,
                                QuestionType = type == "QuestionHasOneAnswer" ? true : false,
                            };

                            if (tmp == "0")
                            {
                                questions.Add(question);
                            }
                            else
                            {
                                ViewBag.msg = "upload fail!";
                                return View();
                            }

                            String firstAnswer = worksheet.Cells[row, 5].Value?.ToString();
                            String photoFirstAnswer = worksheet.Cells[row, 6].Value?.ToString();
                            String secondAnswer = worksheet.Cells[row, 7].Value?.ToString();
                            String photoSecondAnswer = worksheet.Cells[row, 8].Value?.ToString();
                            String thirdAnswer = worksheet.Cells[row, 9].Value?.ToString();
                            String photoThirdAnswer = worksheet.Cells[row, 10].Value?.ToString();
                            String fourthAnswer = worksheet.Cells[row, 11].Value?.ToString();
                            String photoFourthAnswer = worksheet.Cells[row, 12].Value?.ToString();
                            String answerCorrect = worksheet.Cells[row, 13].Value?.ToString();
                            if (firstAnswer == null || secondAnswer == null || thirdAnswer == null || fourthAnswer == null || answerCorrect == null)
                            {
                                tmp = "1";
                            }

                            List<String> listQ = new List<string>()
                            {
                                photoFirstAnswer, photoSecondAnswer, photoThirdAnswer, photoFourthAnswer
                            };
                            List<String> listA = new List<string>()
                            {
                                photoQuestion, photoSecondAnswer, photoThirdAnswer, photoFourthAnswer
                            };
                            List<String> listB = new List<string>()
                            {
                                photoQuestion, photoFirstAnswer, photoThirdAnswer, photoFourthAnswer
                            };
                            List<String> listC = new List<string>()
                            {
                                photoQuestion, photoFirstAnswer, photoSecondAnswer, photoFourthAnswer
                            };
                            List<String> listD = new List<string>()
                            {
                                photoQuestion, photoFirstAnswer, photoThirdAnswer, photoSecondAnswer
                            };
                            for (int i = 0; i < 4; i++)
                            {
                                if (photoQuestion != null && photoQuestion == listQ[i] || 
                                    photoFirstAnswer != null && photoFirstAnswer == listA[i] ||
                                    photoSecondAnswer != null && photoSecondAnswer == listB[i] ||
                                    photoThirdAnswer != null && photoThirdAnswer == listC[i] ||
                                    photoFourthAnswer != null && photoFourthAnswer == listD[i])
                                {
                                    ViewBag.msg = "Image must not duplicate!";
                                    return View();
                                }
                            }
                        
                            if (type == "QuestionHasOneAnswer")
                            {
                                Answer answer1 = new Answer()
                                {
                                    AnswerContent = firstAnswer,
                                    Photo = photoFirstAnswer,
                                    IsCorrectAnswer = answerCorrect == "firstAnswer" ? true : false,
                                };

                                Answer answer2 = new Answer()
                                {
                                    AnswerContent = secondAnswer,
                                    Photo = photoSecondAnswer,
                                    IsCorrectAnswer = answerCorrect == "secondAnswer" ? true : false,
                                };

                                Answer answer3 = new Answer()
                                {
                                    AnswerContent = thirdAnswer,
                                    Photo = photoThirdAnswer,
                                    IsCorrectAnswer = answerCorrect == "thirdAnswer" ? true : false,
                                };

                                Answer answer4 = new Answer()
                                {
                                    AnswerContent = fourthAnswer,
                                    Photo = photoFourthAnswer,
                                    IsCorrectAnswer = answerCorrect == "fourthAnswer" ? true : false,
                                };

                                if (tmp == "0")
                                {
                                    answers.Add(answer1);
                                    answers.Add(answer2);
                                    answers.Add(answer3);
                                    answers.Add(answer4);
                                }
                                else
                                {
                                    ViewBag.msg = "upload fail!";
                                }
                            }
                            else
                            {
                                Answer answer1 = new Answer()
                                {
                                    AnswerContent = firstAnswer,
                                    Photo = photoFirstAnswer,
                                    IsCorrectAnswer = answerCorrect.Contains("firstAnswer") ? true : false,
                                };

                                Answer answer2 = new Answer()
                                {
                                    AnswerContent = secondAnswer,
                                    Photo = photoSecondAnswer,
                                    IsCorrectAnswer = answerCorrect.Contains("secondAnswer") ? true : false,
                                };

                                Answer answer3 = new Answer()
                                {
                                    AnswerContent = thirdAnswer,
                                    Photo = photoThirdAnswer,
                                    IsCorrectAnswer = answerCorrect.Contains("thirdAnswer") ? true : false,
                                };

                                Answer answer4 = new Answer()
                                {
                                    AnswerContent = thirdAnswer,
                                    Photo = photoFourthAnswer,
                                    IsCorrectAnswer = answerCorrect.Contains("fourthAnswer") ? true : false,
                                };

                                if (tmp == "0")
                                {
                                    answers.Add(answer1);
                                    answers.Add(answer2);
                                    answers.Add(answer3);
                                    answers.Add(answer4);
                                }
                                else
                                {
                                    ViewBag.msg = "upload fail!";
                                }
                            }
                        }

                        if (tmp == "0")
                        {
                            int i = 0;
                            _db.Questions.Add(questions[i]);
                            await _db.SaveChangesAsync();
                            for (int j = 0; j < answers.Count; j++)
                            {
                                if (j != 0 && j % 4 == 0)
                                {
                                    i++;
                                    _db.Questions.Add(questions[i]);
                                    await _db.SaveChangesAsync();
                                }
                                answers[j].QuestionId = questions[i].QuestionId;
                                _db.Answers.Add(answers[j]);
                                await _db.SaveChangesAsync();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }                
                catch (Exception e)
                {
                    ViewBag.msg = e.Message;
                }
            }
            else
            {
                ViewBag.msg = "not found!";
            }
            return View();
        }

        public IActionResult Create()
        {
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            ViewBag.selectAnswer = new List<String> 
            {
                "Answer A", "Answer B", "Answer C", "Answer D"
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question, String subject, IFormFile photoQuestion, String questionType, String firstAnswerContent,
        String secondAnswerContent, String thirdAnswerContent, String fourthAnswerContent, IFormFile photoFirstAnswerContent,
        IFormFile photoSecondAnswerContent, IFormFile photoThirdAnswerContent, IFormFile photoFourthAnswerContent, String IsCorrectAnswer,
        String CorrectAnswer1, String CorrectAnswer2, String CorrectAnswer3, String CorrectAnswer4)
        {
            int count = 1;
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            ViewBag.selectAnswer = new List<String>
            {
                "Answer A", "Answer B", "Answer C", "Answer D"
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
                    try
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoQuestion.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoQuestion.CopyToAsync(stream);
                        question.Photo = $"images/QA/{photoQuestion.FileName}";
                    }
                    catch (Exception)
                    {
                        ViewBag.msg = "Image must not duplicate!";
                        return View();
                    }
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
                if(firstAnswerContent != null)
                {
                    firstAnswer.AnswerContent = firstAnswerContent;
                }
                else
                {
                    ViewBag.firstAnswerMsg = "Answer A is required!";
                    ViewBag.secondAnswerContent = secondAnswerContent;
                    ViewBag.thirdAnswerContent = thirdAnswerContent;
                    ViewBag.fourthAnswerContent = fourthAnswerContent;
                    count++;
                }
                if (photoFirstAnswerContent != null)
                {
                    try
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoFirstAnswerContent.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoFirstAnswerContent.CopyToAsync(stream);
                        firstAnswer.Photo = $"images/QA/{photoFirstAnswerContent.FileName}";
                    }
                    catch (Exception)
                    {
                        _db.Questions.Remove(question);
                        await _db.SaveChangesAsync();
                        ViewBag.msg = "Image must not duplicate!";
                        return View();
                    }
                }

                if (IsCorrectAnswer == "Answer A" || CorrectAnswer1 == "true")
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
                if (secondAnswerContent != null)
                {
                    secondAnswer.AnswerContent = secondAnswerContent;
                }
                else
                {
                    ViewBag.secondAnswerMsg = "Answer B is required!";
                    ViewBag.firstAnswerContent = firstAnswerContent;
                    ViewBag.thirdAnswerContent = thirdAnswerContent;
                    ViewBag.fourthAnswerContent = fourthAnswerContent;
                    count++;
                }
                if (photoSecondAnswerContent != null)
                {
                    try
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswerContent.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoSecondAnswerContent.CopyToAsync(stream);
                        secondAnswer.Photo = $"images/QA/{photoSecondAnswerContent.FileName}";
                    }
                    catch (Exception)
                    {
                        _db.Answers.Remove(firstAnswer);
                        await _db.SaveChangesAsync();
                        _db.Questions.Remove(question);
                        await _db.SaveChangesAsync();
                        ViewBag.msg = "Image must not duplicate!";
                        return View();
                    }
                }

                if (IsCorrectAnswer == "Answer B" || CorrectAnswer2 == "true")
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
                if (thirdAnswerContent != null)
                {
                    thirdAnswer.AnswerContent = thirdAnswerContent;
                }
                else
                {
                    ViewBag.thirdAnswerMsg = "Answer C is required!";
                    ViewBag.firstAnswerContent = firstAnswerContent;
                    ViewBag.secondAnswerContent = secondAnswerContent;
                    ViewBag.fourthAnswerContent = fourthAnswerContent;
                    count++;
                }
                if (photoThirdAnswerContent != null)
                {
                    try
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoThirdAnswerContent.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoThirdAnswerContent.CopyToAsync(stream);
                        thirdAnswer.Photo = $"images/QA/{photoThirdAnswerContent.FileName}";
                    }
                    catch(Exception)
                    {
                        _db.Answers.Remove(firstAnswer);
                        _db.Answers.Remove(secondAnswer);
                        await _db.SaveChangesAsync();
                        _db.Questions.Remove(question);
                        await _db.SaveChangesAsync();
                        ViewBag.msg = "Image must not duplicate!";
                        return View();
                    }
                }

                if (IsCorrectAnswer == "Answer C" || CorrectAnswer3 == "true")
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
                if (fourthAnswerContent != null)
                {
                    fourthAnswer.AnswerContent = fourthAnswerContent;
                }
                else
                {
                    ViewBag.fourthAnswerMsg = "Answer D is required!";
                    ViewBag.firstAnswerContent = firstAnswerContent;
                    ViewBag.secondAnswerContent = secondAnswerContent;
                    ViewBag.thirdAnswerContent = thirdAnswerContent;
                    count++;
                }
                if (photoFourthAnswerContent != null)
                {
                    try
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoFourthAnswerContent.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoFourthAnswerContent.CopyToAsync(stream);
                        fourthAnswer.Photo = $"images/QA/{photoFourthAnswerContent.FileName}";
                    }
                    catch (Exception)
                    {
                        _db.Answers.Remove(firstAnswer);
                        _db.Answers.Remove(secondAnswer);
                        _db.Answers.Remove(thirdAnswer);
                        await _db.SaveChangesAsync();
                        _db.Questions.Remove(question);
                        await _db.SaveChangesAsync();
                        ViewBag.msg = "Image must not duplicate!";
                        return View();
                    }
                }

                if (IsCorrectAnswer == "Answer D" || CorrectAnswer4 == "true")
                {
                    fourthAnswer.IsCorrectAnswer = true;
                }
                else
                {
                    fourthAnswer.IsCorrectAnswer = false;
                }

                if(count == 1)
                {
                    _db.Answers.Add(fourthAnswer);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    _db.Answers.Remove(firstAnswer);
                    _db.Answers.Remove(secondAnswer);
                    _db.Answers.Remove(thirdAnswer);
                    _db.Questions.Remove(question);
                    await _db.SaveChangesAsync();
                    return View();
                }
               
            }
            return View();
        }

        public IActionResult Details(int id)
        {
            Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
            List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(id));
            ViewBag.selectAnswer = new List<String> 
            {
                "Answer A" , "Answer B" , "Answer C" , "Answer D"
            };
            for (int i = 0; i < answers.Capacity; i++)
            {
                ViewBag.firstAnswerContent = answers[0].AnswerContent;
                ViewBag.photoFirstAns = answers[0].Photo;
                ViewBag.CorrectAnswer1 = answers[0].IsCorrectAnswer;
                ViewBag.secondAnswerContent = answers[1].AnswerContent;
                ViewBag.photoSecondAns = answers[1].Photo;
                ViewBag.CorrectAnswer2 = answers[1].IsCorrectAnswer;
                ViewBag.thirdAnswerContent = answers[2].AnswerContent;
                ViewBag.photoThirdAns = answers[2].Photo;
                ViewBag.CorrectAnswer3 = answers[2].IsCorrectAnswer;
                ViewBag.fourthAnswerContent = answers[3].AnswerContent;
                ViewBag.photoFourthAns = answers[3].Photo;
                ViewBag.CorrectAnswer4 = answers[3].IsCorrectAnswer;
            }
            return View(question);
        }

        public IActionResult Edit(int id)
        {
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            ViewBag.selectAnswer = new List<String> {
                "Answer A" , "Answer B" , "Answer C" , "Answer D"
            };
            Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
            ViewBag.subject = question.Subject;
            ViewBag.questionTitle = question.QuestionTitle;
            ViewBag.photoQuestion = question.Photo;
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
                "Answer A" , "Answer B" , "Answer C" , "Answer D"
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
            ViewBag.questionTitle = _question.QuestionTitle;
            if (_question != null)
            {
                if (ModelState.IsValid)
                {
                    _question.Subject = subject;
                    if(question.QuestionTitle != null)
                    {
                        _question.QuestionTitle = question.QuestionTitle;
                    }
                    else
                    {
                        _question.QuestionTitle = ViewBag.questionTitle;
                    }
                   
                    if (photoQuestion != null)
                    {
                        try
                        {
                            var filePath = Path.Combine("wwwroot/images/QA", photoQuestion.FileName);
                            var stream = new FileStream(filePath, FileMode.Create);
                            await photoQuestion.CopyToAsync(stream);
                            _question.Photo = $"images/QA/{photoQuestion.FileName}";
                        }
                        catch(Exception)
                        {
                            ViewBag.msg = "Image must not duplicate!";
                            return View();
                        }
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
                        if(firstAnswerContent == null)
                        {
                            answers[0].AnswerContent = answers[0].AnswerContent;
                        }
                        else
                        {
                            answers[0].AnswerContent = firstAnswerContent;
                        }
                        if(photoFirstAnswerContent != null)
                        {
                            if(i == 0)
                            {
                                try
                                {
                                    var filePath = Path.Combine("wwwroot/images/QA", photoFirstAnswerContent.FileName);
                                    var stream = new FileStream(filePath, FileMode.CreateNew);
                                    await photoFirstAnswerContent.CopyToAsync(stream);
                                    answers[0].Photo = $"images/QA/{photoFirstAnswerContent.FileName}";
                                }
                                catch(Exception)
                                {
                                    ViewBag.msg = "Image must not duplicate!";
                                    return View();
                                }
                            }                         
                        }
                        else
                        {
                            answers[0].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[0].AnswerId)).Photo;                           
                        }
                        if (questionType == "true")
                        {
                            if(isCorrectAnswer == "Answer A")
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

                        if(secondAnswerContent == null)
                        {
                            answers[1].AnswerContent = ViewBag.secondAnswerContent;
                        }
                        else
                        {
                            answers[1].AnswerContent = secondAnswerContent;
                        }
                        
                        if (photoSecondAnswerContent != null)
                        {
                            if (i == 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswerContent.FileName);
                                var stream = new FileStream(filePath, FileMode.Create);
                                await photoSecondAnswerContent.CopyToAsync(stream);
                                answers[1].Photo = $"images/QA/{photoSecondAnswerContent.FileName}";
                            }
                        }
                        else
                        {
                            answers[1].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[1].AnswerId)).Photo;                         
                        }
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "Answer B")
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

                        if (thirdAnswerContent == null)
                        {
                            answers[2].AnswerContent = ViewBag.thirdAnswerContent;
                        }
                        else
                        {
                            answers[2].AnswerContent = thirdAnswerContent;
                        }
                        if (photoThirdAnswerContent != null)
                        {
                            if (i == 0)
                            {
                                try
                                {
                                    var filePath = Path.Combine("wwwroot/images/QA", photoThirdAnswerContent.FileName);
                                    var stream = new FileStream(filePath, FileMode.Create);
                                    await photoThirdAnswerContent.CopyToAsync(stream);
                                    answers[2].Photo = $"images/QA/{photoThirdAnswerContent.FileName}";
                                }
                                catch(Exception)
                                {
                                    ViewBag.msg = "Image must not duplicate!";
                                    return View();
                                }
                            }
                        }
                        else
                        {
                            answers[2].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[2].AnswerId)).Photo;                           
                        }
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "Answer C")
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

                        if (fourthAnswerContent == null)
                        {
                            answers[3].AnswerContent = ViewBag.fourthAnswerContent;
                        }
                        else
                        {
                            answers[3].AnswerContent = fourthAnswerContent;
                        }
                        if (photoFourthAnswerContent != null)
                        {
                            if (i == 0)
                            {
                                try
                                {
                                    var filePath = Path.Combine("wwwroot/images/QA", photoFourthAnswerContent.FileName);
                                    var stream = new FileStream(filePath, FileMode.Create);
                                    await photoFourthAnswerContent.CopyToAsync(stream);
                                    answers[3].Photo = $"images/QA/{photoFourthAnswerContent.FileName}";
                                }
                                catch(Exception)
                                {
                                    ViewBag.msg = "Image must not duplicate!";
                                    return View();
                                }
                            }
                        }
                        else
                        {
                            answers[3].Photo = _db.Answers.SingleOrDefault(a => a.AnswerId.Equals(answers[3].AnswerId)).Photo;                       
                        }
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "Answer D")
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
