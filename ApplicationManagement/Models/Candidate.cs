namespace ApplicationManagement.Models
{
    using System.Collections.Generic;

    public class Candidate
    {
        public string id { get; set; }
        public string ProgramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public string Residence { get; set; }
        public string IdNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public string QuestionId { get; set; }
        public string Question { get; set; }
        public dynamic AnswerValue { get; set; } // Can be string, DateTime, int, List<string> depending on QuestionType
    }

}
