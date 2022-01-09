using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Models
{
    public class QuestionData
    {
        public int QuesionID { get; set; }
        public string QuestionTilte { get; set; }
        public bool QuestionType { get; set; }

        public List<Models.AnwserData> Anwser { get; set; } = new List<AnwserData>();

    }
}
