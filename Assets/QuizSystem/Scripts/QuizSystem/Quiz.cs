using System.Collections.Generic;
using UnityEngine;

namespace QuizSytem
{
    public class Quiz : ScriptableObject
    {
        public string Title = "Название теста";
        public string Description = "Описание теста";
        public Sprite Image;
        public List<Question> Questions = new List<Question>();
    }
}
