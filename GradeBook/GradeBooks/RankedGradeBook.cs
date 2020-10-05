using GradeBook.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name) : base(name)
        {
            Type = GradeBookType.Ranked;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            if (Students.Count < 5)
            {
                throw new InvalidOperationException("Ranked-grading requires a minimum of 5 students to work");
            }

            var grade = 'F';
            var rankedGrades = getRankedGrades();
            var calculatedPercentile = calculateGradePercentile(rankedGrades, averageGrade);

            if (calculatedPercentile < 20)
            {
                grade = 'A';
            }
            else if (calculatedPercentile < 40)
            {
                grade = 'B';
            }
            else if (calculatedPercentile < 60)
            {
                grade = 'C';
            }
            else if (calculatedPercentile < 80)
            {
                grade = 'D';
            }

            return grade;
        }

        private double calculateGradePercentile(List<double> rankedGrades, double averageGrade)
        {
            var calculatedPercentile = (double)0;

            for (var rankedGradeIndex = 0; rankedGradeIndex < rankedGrades.Count; rankedGradeIndex++)
            {
                if (averageGrade <= rankedGrades[rankedGradeIndex])
                {
                    calculatedPercentile = 100 - (double)((double)(rankedGradeIndex+1) / rankedGrades.Count * 100);
                    break;
                }
            }

            return calculatedPercentile;
        }

        private List<double> getRankedGrades()
        {
            var grades = Students.Select(student => student.AverageGrade).ToList();
            grades.Sort();

            return grades;
        }

        public override void CalculateStatistics()
        {
            if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
            }
            else
            {
                base.CalculateStatistics();
            }
        }

        public override void CalculateStudentStatistics(string name)
        {
            if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }

            base.CalculateStudentStatistics(name);
        }
    }
}
