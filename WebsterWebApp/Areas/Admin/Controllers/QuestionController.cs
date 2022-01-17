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
            TempData["form"] = new List<String> { "Default Form", "Yes & No" };
            ViewBag.yesnoAnswer = new List<String> { "Yes", "No" };
            ViewBag.selectAnswer = new List<String> { "Answer A", "Answer B", "Answer C", "Answer D" };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question, String questionTitle, String _form, String IsCorrectAnswer2,
        String subject, IFormFile photoQuestion, String questionType, String firstAnswerContent, String secondAnswerContent,
        String thirdAnswerContent, String fourthAnswerContent, IFormFile photoFirstAnswer, IFormFile photoSecondAnswer,
        IFormFile photoThirdAnswer, IFormFile photoFourthAnswer, String isCorrectAnswer,
        String CorrectAnswer1, String CorrectAnswer2, String CorrectAnswer3, String CorrectAnswer4)
        {
            int count = 1;
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            TempData["form"] = new List<String> { "Default Form", "Yes & No" };
            ViewBag.yesnoAnswer = new List<String> { "Yes", "No" };
            ViewBag.selectAnswer = new List<String> { "Answer A", "Answer B", "Answer C", "Answer D" };
            List<Answer> answers = new List<Answer>();
            if (ModelState.IsValid)
            {
                foreach (var item in TempData["optionSubject"] as List<String>)
                {
                    if(subject == item)
                    {
                        question.Subject = item;
                    }
                }
                question.QuestionTitle = questionTitle != null ? question.QuestionTitle = questionTitle : null;
                if(question.QuestionTitle != null)
                {
                    question.QuestionTitle = question.QuestionTitle;
                }
                else
                {
                    ViewBag.questionTitleMsg = "Question title is required!";
                    ViewBag.firstAnswerContent = firstAnswerContent;
                    ViewBag.secondAnswerContent = secondAnswerContent;
                    ViewBag.thirdAnswerContent = thirdAnswerContent;
                    ViewBag.fourthAnswerContent = fourthAnswerContent;
                }

                if (photoQuestion != null)
                {
                    var filePath = Path.Combine("wwwroot/images/QA", photoQuestion.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await photoQuestion.CopyToAsync(stream);
                    question.Photo = $"images/QA/{photoQuestion.FileName}";
                    stream.Close();
                }

                question.QuestionType = questionType == "true" ? true : false;
                if (_form == "Default Form")
                {
                    Answer firstAnswer = new Answer();
                    if (firstAnswerContent != null)
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

                    if (photoFirstAnswer != null)
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoFirstAnswer.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoFirstAnswer.CopyToAsync(stream);
                        firstAnswer.Photo = $"images/QA/{photoFirstAnswer.FileName}";
                        stream.Close();
                    }

                    if (questionType == "true")
                    {
                        firstAnswer.IsCorrectAnswer = isCorrectAnswer == "Answer A" ? firstAnswer.IsCorrectAnswer = true
                                                                                    : firstAnswer.IsCorrectAnswer = false;
                    }
                    else
                    {
                        firstAnswer.IsCorrectAnswer = CorrectAnswer1 == "true" ? true : false;
                    }
                    answers.Add(firstAnswer);

                    Answer secondAnswer = new Answer();
                    if (secondAnswerContent != null)
                    {
                        secondAnswer.AnswerContent = secondAnswerContent;
                    }
                    else
                    {
                        ViewBag.questionTitle = questionTitle;
                        ViewBag.secondAnswerMsg = "Answer B is required!";
                        ViewBag.firstAnswerContent = firstAnswerContent;
                        ViewBag.thirdAnswerContent = thirdAnswerContent;
                        ViewBag.fourthAnswerContent = fourthAnswerContent;
                        count++;
                    }
                    if (photoSecondAnswer != null)
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswer.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoSecondAnswer.CopyToAsync(stream);
                        secondAnswer.Photo = $"images/QA/{photoSecondAnswer.FileName}";
                        stream.Close();
                    }

                    if (questionType == "true")
                    {
                        secondAnswer.IsCorrectAnswer = isCorrectAnswer == "Answer B" ? secondAnswer.IsCorrectAnswer = true
                                                                                     : secondAnswer.IsCorrectAnswer = false;
                    }
                    else
                    {
                        secondAnswer.IsCorrectAnswer = CorrectAnswer2 == "true" ? true : false;
                    }
                    answers.Add(secondAnswer);

                    Answer thirdAnswer = new Answer();
                    if (thirdAnswerContent != null)
                    {
                        thirdAnswer.AnswerContent = thirdAnswerContent;
                    }
                    else
                    {
                        ViewBag.questionTitle = questionTitle;
                        ViewBag.thirdAnswerMsg = "Answer C is required!";
                        ViewBag.firstAnswerContent = firstAnswerContent;
                        ViewBag.secondAnswerContent = secondAnswerContent;
                        ViewBag.fourthAnswerContent = fourthAnswerContent;
                        count++;
                    }
                    if (photoThirdAnswer != null)
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoThirdAnswer.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoThirdAnswer.CopyToAsync(stream);
                        thirdAnswer.Photo = $"images/QA/{photoThirdAnswer.FileName}";
                        stream.Close();
                    }

                    if (questionType == "true")
                    {
                        thirdAnswer.IsCorrectAnswer = isCorrectAnswer == "Answer C" ? thirdAnswer.IsCorrectAnswer = true
                                                                                    : thirdAnswer.IsCorrectAnswer = false;
                    }
                    else
                    {
                        thirdAnswer.IsCorrectAnswer = CorrectAnswer3 == "true" ? true : false;
                    }
                    answers.Add(thirdAnswer);

                    Answer fourthAnswer = new Answer();
                    if (fourthAnswerContent != null)
                    {
                        fourthAnswer.AnswerContent = fourthAnswerContent;
                    }
                    else
                    {
                        ViewBag.questionTitle = questionTitle;
                        ViewBag.fourthAnswerMsg = "Answer D is required!";
                        ViewBag.firstAnswerContent = firstAnswerContent;
                        ViewBag.secondAnswerContent = secondAnswerContent;
                        ViewBag.thirdAnswerContent = thirdAnswerContent;
                        count++;
                    }
                    if (photoFourthAnswer != null)
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoFourthAnswer.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoFourthAnswer.CopyToAsync(stream);
                        fourthAnswer.Photo = $"images/QA/{photoFourthAnswer.FileName}";
                        stream.Close();
                    }

                    if (questionType == "true")
                    {
                        fourthAnswer.IsCorrectAnswer = isCorrectAnswer == "Answer D" ? fourthAnswer.IsCorrectAnswer = true
                                                                                     : fourthAnswer.IsCorrectAnswer = false;
                    }
                    else
                    {
                        fourthAnswer.IsCorrectAnswer = CorrectAnswer4 == "true" ? true : false;
                    }
                    answers.Add(fourthAnswer);
                }
                else
                {
                    Answer firstAnswer = new Answer();
                    firstAnswer.AnswerContent = "Yes";
                    if (IsCorrectAnswer2 == "Yes")
                    {
                        firstAnswer.IsCorrectAnswer = true;
                    }
                    else
                    {
                        firstAnswer.IsCorrectAnswer = false;
                    }
                    answers.Add(firstAnswer);

                    Answer secondAnswer = new Answer();
                    secondAnswer.AnswerContent = "No";

                    if (IsCorrectAnswer2 == "No")
                    {
                        secondAnswer.IsCorrectAnswer = true;
                    }
                    else
                    {
                        secondAnswer.IsCorrectAnswer = false;
                    }
                    answers.Add(secondAnswer);
                }
                if(questionType == "false" && CorrectAnswer1 == null && CorrectAnswer2 == null && CorrectAnswer3 == null && CorrectAnswer4 == null)
                {
                    ViewBag.correctAns = "Choose correct answer please!";
                }

                if (count == 1)
                {
                    _db.Questions.Add(question);
                    await _db.SaveChangesAsync();
                    int questionId = question.QuestionId;
                    for (int i = 0; i < answers.Count; i++)
                    {
                        answers[i].QuestionId = questionId;
                        _db.Answers.Add(answers[i]);
                        await _db.SaveChangesAsync();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    String[] arr = new String[answers.Count];
                    for (int i = 0; i < answers.Count; i++)
                    {
                        arr[i] = answers[i].AnswerContent; 
                    }
                    answers.RemoveAll(ans => arr.Any(a => a == ans.AnswerContent));
                }
               
            }
            return View();
        }

        public IActionResult Details(int id)
        {
            Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
            List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(id));
            ViewBag.selectAnswer = new List<String> { "Answer A" , "Answer B" , "Answer C" , "Answer D" };
            if (answers.Count > 2)
            {
                for (int i = 0; i < answers.Count; i++)
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
            else
            {
                return RedirectToAction("Deta1ls", "Question", new { _id = id });
            }

        }

        public IActionResult Deta1ls(int _id)
        {
            Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(_id));
            List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(_id));
            for (int i = 0; i < answers.Count; i++)
            {
                ViewBag.firstAnswerContent = answers[0].AnswerContent;
                ViewBag.CorrectAnswer1 = answers[0].IsCorrectAnswer;
                ViewBag.secondAnswerContent = answers[1].AnswerContent;
                ViewBag.CorrectAnswer2 = answers[1].IsCorrectAnswer;
            }
            ViewBag.yesnoAnswer = new List<String> { "Yes", "No" };
            return View(question);
        }

        public IActionResult Edit(int id)
        {
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            TempData["form"] = new List<String> { "Default Form", "Yes & No" };
            ViewBag.yesnoAnswer = new List<String> { "Yes", "No" };
            ViewBag.selectAnswer = new List<String> { "Answer A" , "Answer B" , "Answer C" , "Answer D" };
            Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
            ViewBag.subject = question.Subject;
            ViewBag.questionTitle = question.QuestionTitle;
            ViewBag.photoQuestion = question.Photo;
            List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(id));
            if (answers.Count > 2)
            {
                for (int i = 0; i < answers.Count; i++)
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
            else
            {
                for (int i = 0; i < answers.Count; i++)
                {
                    ViewBag.firstAnswerContent = answers[0].AnswerContent;
                    ViewBag.CorrectAnswer1 = answers[0].IsCorrectAnswer;
                    ViewBag.secondAnswerContent = answers[1].AnswerContent;
                    ViewBag.CorrectAnswer2 = answers[1].IsCorrectAnswer;
                    ViewBag.checkForm = "1";
                }
                return View(question);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, String subject, Question question, String _form, IFormFile photoQuestion,
        String questionType, String firstAnswerContent, String secondAnswerContent, String thirdAnswerContent, String fourthAnswerContent,
        IFormFile photoFirstAnswer, IFormFile photoSecondAnswer, IFormFile photoThirdAnswer,
        IFormFile photoFourthAnswer, String isCorrectAnswer, String CorrectAnswer1, String CorrectAnswer2, String CorrectAnswer3,
        String CorrectAnswer4, String isCorrectAnswer2)
        {
            TempData["optionSubject"] = new List<String> { "General Knowledge", "Math", "Tech" };
            TempData["form"] = new List<String> { "Default Form", "Yes & No" };
            ViewBag.yesnoAnswer = new List<String> { "Yes", "No" };
            ViewBag.selectAnswer = new List<String> { "Answer A" , "Answer B" , "Answer C" , "Answer D" };
            List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(id));
            Question _question = _db.Questions.SingleOrDefault(_q => _q.QuestionId.Equals(id));
            if (answers.Count > 2)
            {
                for (int i = 0; i < answers.Count; i++)
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
            }
            else
            {
                for (int i = 0; i < answers.Count; i++)
                {
                    ViewBag.firstAnswerContent = answers[0].AnswerContent;
                    ViewBag.CorrectAnswer1 = answers[0].IsCorrectAnswer;
                    ViewBag.secondAnswerContent = answers[1].AnswerContent;
                    ViewBag.CorrectAnswer2 = answers[1].IsCorrectAnswer;
                }
            }
            if (_question != null)
            {
                if (ModelState.IsValid)
                {
                    _question.Subject = subject;
                    _question.QuestionTitle = question.QuestionTitle != null ? _question.QuestionTitle = question.QuestionTitle
                                                                             : _question.QuestionTitle = _question.QuestionTitle;
                    ViewBag.questionTitle = question.QuestionTitle;

                    if (photoQuestion != null)
                    {
                        var filePath = Path.Combine("wwwroot/images/QA", photoQuestion.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        await photoQuestion.CopyToAsync(stream);
                        _question.Photo = $"images/QA/{photoQuestion.FileName}";
                        stream.Close();
                    }
                    else
                    {
                        _question.Photo = _db.Questions.Find(id).Photo;
                    }
                    _question.QuestionType = questionType == "true" ? true : false;
                    await _db.SaveChangesAsync();

                    int cnt = 0;
                    if (_form == "Default Form" && answers.Count == 4)
                    {
                        for (int i = 0; i < answers.Count; i++)
                        {
                            if (firstAnswerContent != null)
                            {
                                answers[0].AnswerContent = firstAnswerContent;
                            }
                            else
                            {
                                ViewBag.AnswerAMsg = "Answer A is required!";
                                ViewBag.secondAnswerContent = secondAnswerContent;
                                ViewBag.thirdAnswerContent = secondAnswerContent;
                                ViewBag.fourthAnswerContent = fourthAnswerContent;
                                cnt++;
                            }
                            if (photoFirstAnswer != null && i == 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/QA", photoFirstAnswer.FileName);
                                var stream = new FileStream(filePath, FileMode.Create);
                                await photoFirstAnswer.CopyToAsync(stream);
                                answers[0].Photo = $"images/QA/{photoFirstAnswer.FileName}";
                                stream.Close();
                            }
                            else
                            {
                                answers[0].Photo = ViewBag.photoFirstAns;
                            }
                            if (questionType == "true")
                            {
                                if (isCorrectAnswer == "Answer A")
                                {
                                    answers[i].IsCorrectAnswer = i == 0 ? answers[i].IsCorrectAnswer = true
                                                                        : answers[i].IsCorrectAnswer = false;
                                }
                                else
                                {
                                    answers[0].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                answers[0].IsCorrectAnswer = CorrectAnswer1 == "true" ? true : false;
                            }

                            if (secondAnswerContent != null)
                            {
                                answers[1].AnswerContent = secondAnswerContent;
                            }
                            else
                            {
                                ViewBag.AnswerBMsg = "Answer B is required!";
                                ViewBag.firstAnswerContent = firstAnswerContent;
                                ViewBag.thirdAnswerContent = thirdAnswerContent;
                                ViewBag.fourthAnswerContent = fourthAnswerContent;
                                cnt++;
                            }

                            if (photoSecondAnswer != null && i == 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswer.FileName);
                                var stream = new FileStream(filePath, FileMode.Create);
                                await photoSecondAnswer.CopyToAsync(stream);
                                answers[1].Photo = $"images/QA/{photoSecondAnswer.FileName}";
                                stream.Close();
                            }
                            else
                            {
                                answers[1].Photo = ViewBag.photoSecondAns;
                            }

                            if (questionType == "true")
                            {
                                if (isCorrectAnswer == "Answer B")
                                {
                                    answers[i].IsCorrectAnswer = i == 1 ? answers[i].IsCorrectAnswer = true
                                                                        : answers[i].IsCorrectAnswer = false;
                                }
                                else
                                {
                                    answers[1].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                answers[1].IsCorrectAnswer = CorrectAnswer2 == "true" ? true : false;
                            }

                            if (thirdAnswerContent != null)
                            {
                                answers[2].AnswerContent = thirdAnswerContent;
                            }
                            else
                            {
                                ViewBag.AnswerCMsg = "Answer C is required!";
                                ViewBag.firstAnswerContent = firstAnswerContent;
                                ViewBag.secondAnswerContent = secondAnswerContent;
                                ViewBag.fourthAnswerContent = fourthAnswerContent;
                                cnt++;
                            }
                            if (photoThirdAnswer != null && i == 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/QA", photoThirdAnswer.FileName);
                                var stream = new FileStream(filePath, FileMode.Create);
                                await photoThirdAnswer.CopyToAsync(stream);
                                answers[2].Photo = $"images/QA/{photoThirdAnswer.FileName}";
                                stream.Close();
                            }
                            else
                            {
                                answers[2].Photo = ViewBag.photoThirdAns;
                            }
                            if (questionType == "true")
                            {
                                if (isCorrectAnswer == "Answer C")
                                {
                                    answers[i].IsCorrectAnswer = i == 2 ? answers[i].IsCorrectAnswer = true
                                                                        : answers[i].IsCorrectAnswer = false;
                                }
                                else
                                {
                                    answers[2].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                answers[2].IsCorrectAnswer = CorrectAnswer3 == "true" ? true : false;
                            }

                            if (fourthAnswerContent != null)
                            {
                                answers[3].AnswerContent = fourthAnswerContent;
                            }
                            else
                            {
                                ViewBag.AnswerDMsg = "Answer D is required!";
                                ViewBag.firstAnswerContent = firstAnswerContent;
                                ViewBag.secondAnswerContent = secondAnswerContent;
                                ViewBag.thirdAnswerContent = thirdAnswerContent;
                                cnt++;
                            }
                            if (photoFourthAnswer != null && i == 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/QA", photoFourthAnswer.FileName);
                                var stream = new FileStream(filePath, FileMode.Create);
                                await photoFourthAnswer.CopyToAsync(stream);
                                answers[3].Photo = $"images/QA/{photoFourthAnswer.FileName}";
                                stream.Close();
                            }
                            else
                            {
                                answers[3].Photo = ViewBag.photoFourthAns;
                            }
                            if (questionType == "true")
                            {
                                if (isCorrectAnswer == "Answer D")
                                {
                                    answers[i].IsCorrectAnswer = i == 3 ? answers[i].IsCorrectAnswer = true
                                                                        : answers[i].IsCorrectAnswer = false;
                                }
                                else
                                {
                                    answers[3].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                answers[3].IsCorrectAnswer = CorrectAnswer4 == "true" ? true : false;
                            }
                            if (questionType == "false" && CorrectAnswer1 == null && CorrectAnswer2 == null && CorrectAnswer3 == null &&
                                CorrectAnswer4 == null)
                            {
                                ViewBag.correctAns = "Choose correct answer please!";
                                ViewBag.firstAnswerContent = firstAnswerContent;
                                ViewBag.secondAnswerContent = secondAnswerContent;
                                ViewBag.thirdAnswerContent = thirdAnswerContent;
                                ViewBag.fourthAnswerContent = fourthAnswerContent;
                                cnt++;
                            }
                            if(cnt != 0)
                            {
                                return View();
                            }
                        }
                    }
                    else if (_form == "Default Form" && answers.Count == 2)
                    {
                        for (int i = 0; i < answers.Count; i++)
                        {
                            if(firstAnswerContent != null)
                            {
                                answers[0].AnswerContent = firstAnswerContent;
                            }
                            else
                            {
                                ViewBag.AnswerAMsg = "Answer A is required!";
                                ViewBag.secondAnswerContent = secondAnswerContent;
                                ViewBag.thirdAnswerContent = secondAnswerContent;
                                ViewBag.fourthAnswerContent = fourthAnswerContent;
                                cnt++;
                            }
                            if (photoFirstAnswer != null && i == 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/QA", photoFirstAnswer.FileName);
                                var stream = new FileStream(filePath, FileMode.Create);
                                await photoFirstAnswer.CopyToAsync(stream);
                                answers[0].Photo = $"images/QA/{photoFirstAnswer.FileName}";
                                stream.Close();
                            }
                            else
                            {
                                answers[0].Photo = ViewBag.photoFirstAns;
                            }
                            if (questionType == "true")
                            {
                                if (isCorrectAnswer == "Answer A")
                                {
                                    answers[i].IsCorrectAnswer = i == 0 ? answers[i].IsCorrectAnswer = true
                                                                        : answers[i].IsCorrectAnswer = false;
                                }
                                else
                                {
                                    answers[0].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                answers[0].IsCorrectAnswer = CorrectAnswer1 == "true" ? true : false;
                            }

                            if (secondAnswerContent != null)
                            {
                                answers[1].AnswerContent = secondAnswerContent;
                            }
                            else
                            {
                                ViewBag.AnswerBMsg = "Answer B is required!";
                                ViewBag.firstAnswerContent = firstAnswerContent;
                                ViewBag.thirdAnswerContent = thirdAnswerContent;
                                ViewBag.fourthAnswerContent = fourthAnswerContent;
                                cnt++;
                            }

                            if (photoSecondAnswer != null && i == 0)
                            {
                                var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswer.FileName);
                                var stream = new FileStream(filePath, FileMode.Create);
                                await photoSecondAnswer.CopyToAsync(stream);
                                answers[1].Photo = $"images/QA/{photoSecondAnswer.FileName}";
                                stream.Close();
                            }
                            else
                            {
                                answers[1].Photo = ViewBag.photoSecondAns;
                            }
                            if (questionType == "true")
                            {
                                if (isCorrectAnswer == "Answer B")
                                {
                                    answers[i].IsCorrectAnswer = i == 1 ? answers[i].IsCorrectAnswer = true
                                                                        : answers[i].IsCorrectAnswer = false;
                                }
                                else
                                {
                                    answers[1].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                answers[1].IsCorrectAnswer = CorrectAnswer2 == "true" ? true : false;
                            }               
                        }
                        Answer ans = new Answer()
                        {
                            QuestionId = answers[new Random().Next(1)].QuestionId,
                        };
                        Answer _ans = new Answer()
                        {
                            QuestionId = answers[new Random().Next(1)].QuestionId,
                        };
                        if (thirdAnswerContent != null)
                        {
                            ans.AnswerContent = thirdAnswerContent;
                        }
                        else
                        {
                            ViewBag.AnswerCMsg = "Answer C is required!";
                            ViewBag.firstAnswerContent = firstAnswerContent;
                            ViewBag.secondAnswerContent = secondAnswerContent;
                            ViewBag.fourthAnswerContent = fourthAnswerContent;
                            cnt++;
                        }
                        if (fourthAnswerContent != null)
                        {
                            _ans.AnswerContent = fourthAnswerContent;
                        }
                        else
                        {
                            ViewBag.AnswerDMsg = "Answer D is required!";
                            ViewBag.firstAnswerContent = firstAnswerContent;
                            ViewBag.secondAnswerContent = secondAnswerContent;
                            ViewBag.thirdAnswerContent = thirdAnswerContent;
                            cnt++;
                        }
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "Answer C")
                            {
                                ans.IsCorrectAnswer = true;
                                _ans.IsCorrectAnswer = false;
                                for (int i = 0; i < answers.Count; i++)
                                {
                                    answers[i].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                ans.IsCorrectAnswer = false;
                            }
                        }
                        else
                        {
                            ans.IsCorrectAnswer = CorrectAnswer3 == "true" ? true : false;
                        }
     
                        if(photoThirdAnswer != null)
                        {
                            var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswer.FileName);
                            var stream = new FileStream(filePath, FileMode.Create);
                            await photoSecondAnswer.CopyToAsync(stream);
                            answers[1].Photo = $"images/QA/{photoSecondAnswer.FileName}";
                            stream.Close();
                        }
                        
                        if (questionType == "true")
                        {
                            if (isCorrectAnswer == "Answer D")
                            {
                                _ans.IsCorrectAnswer = true;
                                ans.IsCorrectAnswer = true;
                                for (int i = 0; i < answers.Count; i++)
                                {
                                    answers[i].IsCorrectAnswer = false;
                                }
                            }
                            else
                            {
                                _ans.IsCorrectAnswer = false;
                            }
                        }
                        else
                        {
                            _ans.IsCorrectAnswer = CorrectAnswer4 == "true" ? true : false;
                        }
                        if(photoFourthAnswer != null)
                        {
                            var filePath = Path.Combine("wwwroot/images/QA", photoSecondAnswer.FileName);
                            var stream = new FileStream(filePath, FileMode.Create);
                            await photoSecondAnswer.CopyToAsync(stream);
                            answers[1].Photo = $"images/QA/{photoSecondAnswer.FileName}";
                            stream.Close();
                        }
                        if (questionType == "false" && CorrectAnswer1 == null && CorrectAnswer2 == null && CorrectAnswer3 == null &&
                            CorrectAnswer4 == null)
                        {
                            ViewBag.correctAns = "Choose correct answer please!";
                            ViewBag.firstAnswerContent = firstAnswerContent;
                            ViewBag.secondAnswerContent = secondAnswerContent;
                            ViewBag.thirdAnswerContent = thirdAnswerContent;
                            ViewBag.fourthAnswerContent = fourthAnswerContent;
                            cnt++;
                        }

                        if (cnt != 0)
                        {
                            return View();
                        }
                        else
                        {
                            _db.Answers.Add(ans);
                            _db.Answers.Add(_ans);
                        }                  
                    }
                    else if (_form != "Default Form" && answers.Count == 4)
                    {
                        question.QuestionType = true;
                        for (int i = 0; i < answers.Count; i++)
                        {
                            answers[0].AnswerContent = "Yes";
                            answers[0].Photo = answers[0].Photo != null ? answers[0].Photo = null : null;

                            answers[0].IsCorrectAnswer = isCorrectAnswer2 == "Yes" && i == 0 ? answers[i].IsCorrectAnswer = true
                                                                                             : answers[i].IsCorrectAnswer = false;


                            answers[1].AnswerContent = "No";
                            answers[1].Photo = answers[1].Photo != null ? answers[1].Photo = null : null;

                            answers[1].IsCorrectAnswer = isCorrectAnswer2 == "No" && i == 1 ? answers[i].IsCorrectAnswer = true
                                                                                            : answers[i].IsCorrectAnswer = false;
                        }
                        _db.Answers.Remove(answers[2]);
                        _db.Answers.Remove(answers[3]);
                    }
                    else
                    {
                        question.QuestionType = true;
                        for (int i = 0; i < answers.Count; i++)
                        {
                            answers[0].AnswerContent = "Yes";
                            answers[0].Photo = answers[0].Photo != null ? answers[0].Photo = null : null;
                            answers[i].IsCorrectAnswer = isCorrectAnswer2 == "Yes" && i == 0 ? answers[i].IsCorrectAnswer = true
                                                                                             : answers[i].IsCorrectAnswer = false;

                            answers[1].AnswerContent = "No";
                            answers[1].Photo = answers[1].Photo != null ? answers[1].Photo = null : null;
                            answers[1].IsCorrectAnswer = isCorrectAnswer2 == "No" && i == 1 ? answers[i].IsCorrectAnswer = true
                                                                                            : answers[i].IsCorrectAnswer = false;
                        }
                    }
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return BadRequest("not found!");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Question question = _db.Questions.SingleOrDefault(q => q.QuestionId.Equals(id));
                List<Answer> answers = _db.Answers.ToList().FindAll(a => a.QuestionId.Equals(question.QuestionId));
                for (int i = 0; i < answers.Count; i++)
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
