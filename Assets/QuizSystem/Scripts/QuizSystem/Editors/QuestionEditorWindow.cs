using QuizSytem;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace QuizSystem
{
    #if UNITY_EDITOR
    public class QuestionEditorWindow : EditorWindow
    {
        Quiz _currentQuiz;
        Question _currentQuestion;
        Vector2 scrollPosition;
        public static void Init()
        {
            EditorWindow window = GetWindow<QuestionEditorWindow>();
            window.maxSize = window.minSize = new Vector2(450, 300);
        }
        void OnGUI()
        {
            _currentQuiz = Utility.LoadDatabase().Quizzes[Utility.CurrentQuiz];
            _currentQuestion = _currentQuiz.Questions[Utility.CurrentQuestion];
            SerializedObject serializedObject = new SerializedObject(_currentQuiz);
            serializedObject.Update();

            SerializedProperty serializedPropertyList = serializedObject.FindProperty("Questions");
            DrawQuestion(serializedPropertyList);
        }

        private void DrawQuestion(SerializedProperty list)
        {
            SerializedObject serilizedObject = new SerializedObject(list.GetArrayElementAtIndex(Utility.CurrentQuestion).objectReferenceValue);
            serilizedObject.Update();
            var questionText = serilizedObject.FindProperty("QuestionText");
            var answers = serilizedObject.FindProperty("Answers");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Вопрос : ", GUILayout.ExpandWidth(false), GUILayout.Width(50));
            EditorStyles.textField.wordWrap = true;
            questionText.stringValue = EditorGUILayout.TextField(questionText.stringValue, GUILayout.MaxHeight(100), GUILayout.MaxWidth(350));
            EditorGUILayout.EndHorizontal();
            DrawAnswers(answers);
            serilizedObject.ApplyModifiedProperties();
        }

        private void DrawAnswers(SerializedProperty list)
        {
            EditorGUILayout.BeginVertical("box");
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < list.arraySize; i++)
            {
                SerializedObject serializedObject = new SerializedObject(list.GetArrayElementAtIndex(i).objectReferenceValue);
                var answerText = serializedObject.FindProperty("AnswerText");
                var isRight = serializedObject.FindProperty("IsRight");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ответ : ", GUILayout.ExpandWidth(false), GUILayout.Width(50));
                answerText.stringValue = EditorGUILayout.TextField(answerText.stringValue, GUILayout.Width(150));
                EditorGUILayout.LabelField("Правильный? : ", GUILayout.ExpandWidth(false), GUILayout.Width(100));
                isRight.boolValue = EditorGUILayout.Toggle(isRight.boolValue);
                serializedObject.ApplyModifiedProperties();
                if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, GUILayout.MaxWidth(50)))
                {
                    RemoveAnswer(list, i);
                }
                EditorGUILayout.EndHorizontal();
                
            }
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Add answer", EditorStyles.miniButtonRight, GUILayout.MaxWidth(100)))
            {
                AddAnswer();
            }
            EditorGUILayout.EndVertical();
            
            
        }
        private void AddAnswer()
        {
            Answer answer = CreateInstance<Answer>();
            _currentQuestion.Answers.Add(answer);
            AssetDatabase.AddObjectToAsset(answer, _currentQuiz);
            AssetDatabase.SaveAssets();
        }
        private void RemoveAnswer(SerializedProperty answerList, int index)
        {
            _currentQuestion.Answers.RemoveAt(index);
            DestroyImmediate(answerList.GetArrayElementAtIndex(index).objectReferenceValue, true);
            AssetDatabase.SaveAssets();
        }
    }
    #endif
}