namespace ApplicationManagement.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationProgram
    {
        [Key]
        public string id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)] 
        public string Name { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)] 
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
        public virtual List<Candidate> Candidates { get; set; }
    }

    public class Question
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // "Paragraph", "YesNo", "Dropdown", "MultipleChoice", "Date", "Number"
        public List<string>? Options { get; set; } // Only used for "Dropdown" and "MultipleChoice"
    }

}
