using QuizSytem;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace QuizSystem
{
    #if UNITY_EDITOR
    public static class Utility
    {
        private const string DATABASE_PATH = @"Assets/QuizSystem/QuizDatabase.asset";
        public static int CurrentQuiz;
        public static int CurrentQuestion;

        public static QuizDatabase LoadDatabase()
        {
            QuizDatabase asset = AssetDatabase.LoadAssetAtPath<QuizDatabase>(DATABASE_PATH);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<QuizDatabase>();
                AssetDatabase.CreateAsset(asset, DATABASE_PATH);
            }
            return AssetDatabase.LoadAssetAtPath<QuizDatabase>(DATABASE_PATH);
        }
    }
    #endif
}