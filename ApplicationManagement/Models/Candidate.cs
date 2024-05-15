namespace ApplicationManagement.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Candidate
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string ProgramId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)] 
        public string LastName { get; set; }
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Nationality { get; set; }
        public string? Residence { get; set; }
        public string? IdNumber { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public string QuestionId { get; set; }
        public string Question { get; set; }
        public dynamic AnswerValue { get; set; } // Can be string, DateTime, int, List<string> depending on QuestionType
    }

}
