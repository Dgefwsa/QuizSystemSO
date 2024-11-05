using System.Collections.Generic;
using QuizSystem;
using UnityEngine;

namespace QuizSytem
{
    public class Question : ScriptableObject
    {
        public string QuestionText = "Текст вопроса";
        public List<Answer> Answers = new List<Answer>();
        public int RightAnswer;
    }
}