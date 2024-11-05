using QuizSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace QuizSytem
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(QuizDatabase))]
    public class QuizDatabaseEditor : Editor
    {
        QuizDatabase quizzes;


        void OnEnable()
        {
            quizzes = (QuizDatabase)target;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            SerializedObject serializedObject = new SerializedObject(quizzes);
            serializedObject.Update();

            SerializedProperty serializedPropertyList = serializedObject.FindProperty("Quizzes");
            
            Draw(serializedPropertyList);
            
            serializedObject.ApplyModifiedProperties();

            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Quiz", GUILayout.Height(20)))
            {
                AddQuiz();
            }
            EditorGUILayout.EndHorizontal();

        }

        private void Draw(SerializedProperty list)
        {
            EditorGUILayout.BeginVertical("box");
            ShowElements(list);
            EditorGUILayout.EndVertical();
        }
        private void ShowElements(SerializedProperty list)
        {
            for (int i = 0; i < list.arraySize; i++)
            {

                EditorGUILayout.BeginVertical("box");
                SerializedObject serializedObject = new SerializedObject(list.GetArrayElementAtIndex(i).objectReferenceValue);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(serializedObject.FindProperty("Title").stringValue);
                var questionsCount = serializedObject.FindProperty("Questions").arraySize.ToString();
                
                
                if (GUILayout.Button("Edit", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(50)))
                {
                    Utility.CurrentQuiz = i;
                    QuizEditorWindow.Init();
                }
                if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(50)))
                {
                    RemoveQuiz(list, i);
                }
                EditorGUILayout.EndHorizontal();
                if  (serializedObject.FindProperty("Questions") != null)
                {
                    EditorGUILayout.LabelField($"Количество вопросов : {questionsCount}");
                }
                EditorGUILayout.EndVertical();
            }
        }
        
        private void AddQuiz()
        {
            Quiz quiz = CreateInstance<Quiz>();
            quizzes.Quizzes.Add(quiz);
            AssetDatabase.AddObjectToAsset(quiz, quizzes);
            AssetDatabase.SaveAssets();
        }
        private void RemoveQuiz(SerializedProperty quizList, int index)
        {
            foreach (Question question in quizzes.Quizzes[index].Questions)
            {
                foreach (Answer answer in question.Answers)
                {
                    DestroyImmediate(answer, true);
                }
                question.Answers.Clear();
                DestroyImmediate(question, true);
            }
            quizzes.Quizzes[index].Questions.Clear();
            quizzes.Quizzes.RemoveAt(index);
            DestroyImmediate(quizList.GetArrayElementAtIndex(index).objectReferenceValue, true);
            AssetDatabase.SaveAssets();
        }
    }
    #endif
}