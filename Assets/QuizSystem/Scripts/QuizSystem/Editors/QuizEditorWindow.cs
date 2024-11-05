using System.Collections.Generic;
using QuizSytem;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace QuizSystem
{
    #if UNITY_EDITOR
    public class QuizEditorWindow : EditorWindow
    {   
        private QuizDatabase _quizzes;
        private Quiz _currentQuiz;
        Vector2 scrollPosition;
        public static void Init()
        {
            EditorWindow window = GetWindow<QuizEditorWindow>();
            window.maxSize = window.minSize = new Vector2(700, 500);
        }

        void OnGUI()
        {
            _quizzes = Utility.LoadDatabase();
            _currentQuiz = _quizzes.Quizzes[Utility.CurrentQuiz];

            SerializedObject serializedObject = new SerializedObject(_quizzes);
            serializedObject.Update();
            SerializedProperty serializedPropertyList = serializedObject.FindProperty("Quizzes");

            DrawQuiz(serializedPropertyList);

        }
        private void DrawQuiz(SerializedProperty list)
        {
            SerializedObject serializedObject = new SerializedObject(list.GetArrayElementAtIndex(Utility.CurrentQuiz).objectReferenceValue);
            serializedObject.Update();
            var title = serializedObject.FindProperty("Title");
            var description = serializedObject.FindProperty("Description");
            var image = serializedObject.FindProperty("Image");
            var questions = serializedObject.FindProperty("Questions");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box");
            DrawImage(image);
            image.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(image.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(150));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            title.stringValue = EditorGUILayout.TextField("Название : ", title.stringValue);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorStyles.textField.wordWrap = true;
            description.stringValue = EditorGUILayout.TextField("Описание : ", description.stringValue, GUILayout.MaxHeight(150));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            serializedObject.ApplyModifiedProperties();
            DrawQuestions(questions);
            if (GUILayout.Button("Add Question", GUILayout.Height(20)))
            {
                AddQuestion();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Done!", GUILayout.MaxHeight(40)))
            {
                AssetDatabase.SaveAssets();
                Close();
            }
        }

        private void DrawQuestions(SerializedProperty list)
        {
            EditorGUILayout.BeginVertical("box");
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < list.arraySize; i++)
            {
                SerializedObject serializedObject = new SerializedObject(list.GetArrayElementAtIndex(i).objectReferenceValue);
                var answersCount = serializedObject.FindProperty("Answers").arraySize.ToString();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(serializedObject.FindProperty("QuestionText").stringValue, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(200));
                EditorGUILayout.LabelField($"Количество ответов : {answersCount}", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(150));
                if (GUILayout.Button("Edit", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50)))
                {
                    Utility.CurrentQuestion = i;
                    QuestionEditorWindow.Init();
                }
                if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50)))
                {
                    RemoveQuestion(list, i);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawImage(SerializedProperty image)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(image.objectReferenceValue);
            GUILayout.Label("", GUILayout.Width(150), GUILayout.Height(150));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture, ScaleMode.ScaleToFit);
        }
        private void AddQuestion()
        {
            Question question = CreateInstance<Question>();
            _currentQuiz.Questions.Add(question);
            AssetDatabase.AddObjectToAsset(question, _currentQuiz);
            AssetDatabase.SaveAssets();
        }
        private void RemoveQuestion(SerializedProperty questionList, int index)
        {
            foreach (Answer answer in _currentQuiz.Questions[index].Answers)
            {
                DestroyImmediate(answer, true);
            }
            _currentQuiz.Questions[index].Answers.Clear();
            _currentQuiz.Questions.RemoveAt(index);
            DestroyImmediate(questionList.GetArrayElementAtIndex(index).objectReferenceValue, true);
            AssetDatabase.SaveAssets();
        }
    }
    #endif
}