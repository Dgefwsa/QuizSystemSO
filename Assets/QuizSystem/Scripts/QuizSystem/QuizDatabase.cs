using System.Collections.Generic;
using UnityEngine;

namespace QuizSytem
{
    [CreateAssetMenu(fileName = "QuizDatabase", menuName = "QuizSystem")]
    public class QuizDatabase : ScriptableObject
    {
        public List<Quiz> Quizzes = new List<Quiz>();
    }
}