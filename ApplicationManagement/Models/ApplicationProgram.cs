namespace ApplicationManagement.Models
{
    using System.Collections.Generic;

    public class ApplicationProgram
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // "Paragraph", "YesNo", "Dropdown", "MultipleChoice", "Date", "Number"
        public List<string> Options { get; set; } // Only used for "Dropdown" and "MultipleChoice"
    }

}
