﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsterWebApp.Areas.Admin.Models;
using WebsterWebApp.Data;
using WebsterWebApp.Models;
using Newtonsoft.Json;

namespace WebsterWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExamController : Controller
    {
        private readonly DatabaseContext _context;

        public ExamController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Exams.ToList());
        }
        public IActionResult Create()
        {

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
                    for (int i = 1; i <= 5; i++)
                    {
                        int pas = pass.Next((abc.Count) - 1);
                        char cha = abc.ElementAt(pas);
                        pwd = pwd + cha.ToString();
                    }
                    if (exam.ExamType == true)
                    {
                            
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
                        exam.PassWord = pwd;
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
            ViewBag.index1 = (from c in _context.Questions where c.Subject.Equals("Math") select c).ToList();
            ViewBag.index2 = (from c in _context.Questions where c.Subject.Equals("Tech") select c).ToList();
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
    }
}
