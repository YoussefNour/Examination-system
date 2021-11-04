using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination
{
    class Program
    {
        static void Main(string[] args)
        {
            Subject subject = new Subject("OOP");
            Question q1 = new ChooseOne("which access modifier allows access of parent attributes in child class", new string[3] { "private", "protected", "public" }, 5, 1);
            Question q2 = new TrueFalse("can a class inherit mupltiple classes in c#", 5, 0);
            Question q3 = new ChooseAll("choose access modifiers in classes", new string[5] { "private", "protected", "public", "internal" , "free" },4, new int[4] {0,1,2,3});
            QuestionList questions = new QuestionList("questions.txt");
            questions.Add(q1);
            questions.Add(q2);
            questions.Add(q3);
            DateTime date = new DateTime(2021,10,27);
            Exam ex1 = new FinalExam(date, subject, questions);
            ex1.printExam();
            //Exam ex2 = new PracticeExam(date, subject, questions);
            //ex2.printExam();
            Console.Write("done?");
            Console.ReadLine();
        }
    }

    class Subject
    {
        string Name;

        public Subject(string Name)
        {
            this.Name = Name;
        }

        public string getName() { return Name; }
        public void SetNames(string Name) { this.Name = Name; }

        ~Subject()
        {
            Console.WriteLine("Subject " + Name + " destroyed");
        }
    }

    enum Mode
    {
        Starting,
        Queued,
        Finished
    }

    class QuestionList : List<Question>
    {
        string fileName;

        public string FileName { get => fileName; set => fileName = value; }

        public QuestionList(string fileName):base()
        {
            this.fileName = fileName;
        }

        public new void Add(Question question)
        {
            base.Add(question);
            StreamWriter file = new StreamWriter(FileName, append: true);
            file.WriteLine(question.ToString());
            file.Close();
        }
    }

    abstract class Exam
    {
        DateTime date;
        Subject subject;
        QuestionList questions;
        Mode mode;

        public DateTime Date { get => date; set => date = value; }
        public Subject Subject { get => subject; set => subject = value; }
        public QuestionList Questions { get => questions; set => questions = value; }
        internal Mode Mode { get => mode; set => mode = value; }

        public Exam(DateTime date, Subject subject, QuestionList questions)
        {
            this.date = date;
            this.subject = subject;
            this.questions = questions;
        }

        abstract public void printExam();
    }

    class FinalExam: Exam
    {
        public FinalExam(DateTime date, Subject subject, QuestionList questions): base(date, subject, questions)
        {

        }

        public override void printExam()
        {
            if (Questions.Count > 0)
            {
                Console.WriteLine(Subject.getName() + " Exam\t" + Date + "\nNumber of Questions:" + Questions.Count);
                for (int i = 0; i < Questions.Count; i++)
                {
                    Console.Write("Q" + (i + 1) + " ");
                    Questions[i].printQuestion();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Exam is Empty");
            }

        }
    }

    class PracticeExam : Exam
    {
        public PracticeExam(DateTime date, Subject subject, QuestionList questions) : base(date, subject, questions)
        {

        }

        public override void printExam()
        {
            if (Questions.Count > 0)
            {
                Console.WriteLine(Subject.getName() + " Exam\t" + Date + "\nNumber of Questions:" + Questions.Count);
                int marks = 0;
                int Totalmarks = 0;
                for (int i = 0; i < Questions.Count; i++)
                {
                    Question q = Questions[i];
                    Console.Write("Q" + (i + 1) + " ");
                    q.printQuestion();
                    q.takeAnswer();
                    marks += q.markQuestion();
                    Totalmarks += q.Mark;
                    q.showCorrectAnswer();
                    Console.WriteLine();
                }
                Console.WriteLine("you scored: " + marks+" out of "+ Totalmarks);
            }
            else
            {
                Console.WriteLine("Exam is Empty");
            }

        }
    }

    abstract class Question
    {
        string header;
        string body;
        int mark;

        public string Header { get => header; set => header = value; }
        public string Body { get => body; set => body = value; }
        public int Mark { get => mark; set => mark = value; }

        public Question(string body,int mark)
        {
            this.body = body;
            this.mark = mark;
        }

        public abstract void printQuestion();
        public abstract void takeAnswer();
        public abstract int markQuestion();
        public abstract void showCorrectAnswer();

        public void printChoice(int number,string choice)
        {
            Console.WriteLine(number + "." + choice);
        }

        public abstract override string ToString();

        public abstract string choicesToString();
    }

    class ChooseOne : Question
    {
        string[] choices;
        int correctAnswerIndex;
        int studentanswerIndex = -1;

        public string[] Choices { get => choices; set => choices = value; }
        public int CorrectAnswerIndex { get => correctAnswerIndex; set => correctAnswerIndex = value; }
        public int StudentanswerIndex { get => studentanswerIndex; set => studentanswerIndex = value; }

        public ChooseOne(string body, string[] choices, int mark, int correctAnswerIndex) : base(body, mark)
        {
            Header = "Choose One:";
            this.choices = choices;
            this.correctAnswerIndex = correctAnswerIndex;
        }

        public override void printQuestion()
        {
            Console.WriteLine(Body + "\t" + "Marks:" + Mark + "\n" + Header);
            for (int i = 0; i < choices.Length; i++)
            {
                printChoice(i, choices[i]);
            }
        }
        public override string choicesToString()
        {
            string choicesString = "";
            for (int i = 0; i < Choices.Length; i++)
            {
                choicesString += "\n" + i + "." + Choices[i];
            }
            return choicesString;
        }

        public override string ToString()
        {
            return Body + "\t" + "Marks: " + Mark + "\n"+ Header + choicesToString() + "\n";
        }

        public override int markQuestion()
        {
            if(correctAnswerIndex == studentanswerIndex)
            {
                return Mark;
            }
            else
            {
                return 0;
            }
        }

        public override void takeAnswer()
        {
            Console.Write("Answer Index: ");
            int ans = int.Parse(Console.ReadLine());
            StudentanswerIndex = ans;
        }

        public override void showCorrectAnswer()
        {
            Console.WriteLine("Correct Answer Index: "+correctAnswerIndex);
        }
    }

    class TrueFalse : Question
    {
        string[] choices = new string[2];
        int correctAnswerIndex;
        int studentanswerIndex = -1;

        public string[] Choices { get => choices; set => choices = value; }
        public int CorrectAnswerIndex { get => correctAnswerIndex; set => correctAnswerIndex = value; }
        public int StudentanswerIndex { get => studentanswerIndex; set => studentanswerIndex = value; }

        public TrueFalse(string body, int mark, int correctAnswerIndex) : base(body, mark)
        {
            Header = "Choose True or False:";
            choices[0] = "true";
            choices[1] = "false";
            this.correctAnswerIndex = correctAnswerIndex;
        }

        public override void printQuestion()
        {
            Console.WriteLine(Body + "\t" + "Marks:" + Mark + "\n" + Header);
            for (int i = 0; i < choices.Length; i++)
            {
                printChoice(i, choices[i]);
            }
        }

        public override string choicesToString()
        {
            string choicesString = "";
            for (int i = 0; i < Choices.Length; i++)
            {
                choicesString += "\n" + i + "." + Choices[i];
            }
            return choicesString;
        }

        public override string ToString()
        {
            return Body + "\t" + "Marks: " + Mark + "\n" + Header + choicesToString() + "\n";
        }

        public override int markQuestion()
        {
            if (correctAnswerIndex == studentanswerIndex)
            {
                return Mark;
            }
            else
            {
                return 0;
            }
        }

        public override void takeAnswer()
        {
            Console.Write("Answer Index: ");
            int ans = int.Parse(Console.ReadLine());
            StudentanswerIndex = ans;
        }
        public override void showCorrectAnswer()
        {
            Console.WriteLine("Correct Answer Index: " + correctAnswerIndex);
        }
    }

    class ChooseAll : Question
    {
        string[] choices = new string[2];
        int[] correctAnswerIndex;
        int[] studentanswerIndex;

        public string[] Choices { get => choices; set => choices = value; }
        public int[] CorrectAnswerIndex { get => correctAnswerIndex; set => correctAnswerIndex = value; }
        public int[] StudentanswerIndex { get => studentanswerIndex; set => studentanswerIndex = value; }

        public ChooseAll(string body, string[] choices, int mark, int[] correctAnswerIndex) : base(body, mark)
        {
            Header = "Choose all the correct answers:";
            this.choices = choices;
            this.correctAnswerIndex = correctAnswerIndex;
        }

        public override void printQuestion()
        {
            Console.WriteLine(Body + "\t" + "Marks:" + Mark+ "\n" + Header);
            for (int i = 0; i < choices.Length; i++)
            {
                printChoice(i, choices[i]);
            }
        }

        public override string choicesToString()
        {
            string choicesString = "";
            for (int i = 0; i < Choices.Length; i++)
            {
                choicesString += "\n"+ i+"." + Choices[i];
            }
            return choicesString;
        }

        public override string ToString()
        {
            return Body + "\t" + "Marks: " + Mark + "\n" + Header + choicesToString() + "\n";
        }

        public override int markQuestion()
        {
            bool isCorrect = true;
            int wrongCount = 0;
            Array.Sort(correctAnswerIndex);
            Array.Sort(studentanswerIndex);
            for (int i = 0; i < correctAnswerIndex.Length; i++)
            {
                if (correctAnswerIndex[i] != studentanswerIndex[i])
                {
                    isCorrect = false;
                    wrongCount++;
                }
            }

            if (isCorrect)
            {
                return Mark;
            }
            else
            {
                int finalMark = Mark - wrongCount;
                finalMark = finalMark < 0 ?   0 : finalMark;
                return finalMark;
            }
        }

        public override void takeAnswer()
        {
            Console.Write("Answer Index: ");
            int[] ans = new int[correctAnswerIndex.Length];
            for (int i = 0; i < ans.Length; i++)
            {
                Console.WriteLine("enter answer index");
                ans[i] = int.Parse(Console.ReadLine());
            }
            StudentanswerIndex = ans;
        }

        public override void showCorrectAnswer()
        {
            Console.Write("Correct Answer Index: ");
            for (int i = 0; i < correctAnswerIndex.Length; i++)
            {
                Console.Write(correctAnswerIndex[i]+" ");
            }
            Console.WriteLine();
        }
    }

    class Answer
    {
        string value;
        int index;

        public string Value { get => value; set => this.value = value; }
        public int Index { get => index; set => index = value; }

        public Answer(string value, int index)
        {
            this.value = value;
            this.index = index;
        }
    }
}
