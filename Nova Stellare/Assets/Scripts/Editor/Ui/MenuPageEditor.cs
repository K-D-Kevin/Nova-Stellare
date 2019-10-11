using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(MenuPage))]
[CanEditMultipleObjects]
public class MenuPageEditor : Editor
{
    MenuPage myPage;
    SerializedProperty m_ParentObject;
    SerializedProperty m_IsTitlePage;
    SerializedProperty m_PagePrefab;
    SerializedProperty m_TextDisplayPrefab;
    SerializedProperty m_MaxPages;
    SerializedProperty m_TextDisplayPrefabEdgeSize;
    SerializedProperty m_TextDisplayPrefabBufferSize;
    SerializedProperty m_CorrespondingPages;
    SerializedProperty m_TextDisplayTitle;
    SerializedProperty m_TextDisplayExit;
    SerializedProperty m_MenuCenter;
    SerializedProperty m_PageName;
    SerializedProperty m_FollowingPages;
    SerializedProperty NewPageName;
    bool Set = false;
    // Get Property Variables
    private void OnEnable()
    {
        // Get Target entity
        myPage = (MenuPage)target;

        // Get Variables
        m_ParentObject = serializedObject.FindProperty("ParentObject");
        m_IsTitlePage = serializedObject.FindProperty("IsTitle");
        m_PagePrefab = serializedObject.FindProperty("DefualtPagePrefab");
        m_TextDisplayPrefab = serializedObject.FindProperty("DefualtTextDisplayPrefab");
        m_MaxPages = serializedObject.FindProperty("MaxPages");
        m_TextDisplayPrefabEdgeSize = serializedObject.FindProperty("EdgePixelSize");
        m_TextDisplayPrefabBufferSize = serializedObject.FindProperty("TitleBufferSize");
        m_CorrespondingPages = serializedObject.FindProperty("CorrespondingPages");
        m_TextDisplayTitle = serializedObject.FindProperty("TitleTextDisplay");
        m_TextDisplayExit = serializedObject.FindProperty("ExitTextDisplay");
        m_MenuCenter = serializedObject.FindProperty("CenteredOnPixel");
        m_PageName = serializedObject.FindProperty("PageName");
        m_FollowingPages = serializedObject.FindProperty("FollowingPagesTransform");
        NewPageName = serializedObject.FindProperty("NewPageName");
        Set = true;

        // Find Following Pages list
        myPage.FindCorrespondingPages();
    }

    public override void OnInspectorGUI()
    {

        if (Set)
        {
            // Layout Base Variables
            EditorGUILayout.PropertyField(m_PageName, new GUIContent("Page Name"));
            EditorGUILayout.PropertyField(m_TextDisplayTitle, new GUIContent("Title Display"));
            EditorGUILayout.PropertyField(m_TextDisplayExit, new GUIContent("Exit Display"));
            EditorGUILayout.PropertyField(m_IsTitlePage, new GUIContent("Is Title Page"));

            if (myPage.IsTitle)
            {
                EditorGUILayout.PropertyField(m_MaxPages, new GUIContent("Max Pages"));
                EditorGUILayout.PropertyField(m_TextDisplayPrefabEdgeSize, new GUIContent("Edge Size"));
                EditorGUILayout.PropertyField(m_TextDisplayPrefabBufferSize, new GUIContent("Buffer Size"));
                EditorGUILayout.PropertyField(m_MenuCenter, new GUIContent("Centered On X Pixel"));
                EditorGUILayout.PropertyField(m_PagePrefab, new GUIContent("Page Prefab"));
                EditorGUILayout.PropertyField(m_TextDisplayPrefab, new GUIContent("Text Display Prefab"));
                EditorGUILayout.PropertyField(m_ParentObject, new GUIContent("Parent Object"));
                EditorGUILayout.PropertyField(m_FollowingPages, new GUIContent("Following Pages Transform"));

                if (myPage.GetDefualtTextDisplayPrefab != null && myPage.GetDefualtPagePrefab != null && myPage.GetParent != null)
                {
                    if (myPage.GetMaxPages > myPage.CorrespondingPages.Count)
                    {
                        if (myPage.CorrespondingPages.Count > 0)
                        {
                            EditorGUILayout.PropertyField(m_CorrespondingPages, new GUIContent("Connected Pages"));
                        }
                        EditorGUILayout.PropertyField(NewPageName, new GUIContent("New Page Name"));
                    }
                }
            }

            // Want to put a drop down menu with a serialized list of variables to adjust those pages and tie them into this one
            //if (myPage.CorrespondingPages.Count > 0)
            //{
            //    foreach (MenuPage MP in myPage.CorrespondingPages)
            //    {
            //        SerializedProperty f_PageTitleDisplay = SerializedO;
            //        EditorGUILayout.PropertyField(m_TextDisplayPrefab, new GUIContent("Text Display Prefab"));

            //    }
            //}

            // If title page remove exit button
            
            if (myPage.IsTitle && myPage.transform.parent.name != "Following Pages")
                myPage.GetExitTextDisplay.gameObject.SetActive(false);
            else
                myPage.GetExitTextDisplay.gameObject.SetActive(true);

            // Update Inspector and change variables
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (myPage.IsTitle)
            {
                if (myPage.GetDefualtTextDisplayPrefab != null && myPage.GetDefualtPagePrefab != null)
                {
                    if (myPage.GetParent != null)
                    {
                        if (myPage.GetMaxPages > myPage.CorrespondingPages.Count)
                        {
                            if (GUILayout.Button("Add Page"))
                            {
                                myPage.AddPage();
                                TextDisplay temp = Instantiate(myPage.GetDefualtTextDisplayPrefab, myPage.transform);
                                //temp.SetDisplay(myPage.NewPageName, new Vector3(30, 30), new Vector3(100, 10));
                                RectTransform RT_Temp = temp.GetComponent<RectTransform>();
                                float X_Position = RT_Temp.rect.width / 2 + myPage.GetCenteredOnPixel;
                                float Y_Position = -Mathf.Abs(RT_Temp.rect.height / 2 - (134 + 60) * (myPage.CorrespondingPages.Count + 1));
                                Debug.Log("Rect W: " + RT_Temp.rect.width + " / Rect H: " + RT_Temp.rect.height);
                                Debug.Log("X Pos: " + X_Position + " / Rect H: " + Y_Position);
                                // Scaler Transform to get scale
                                RectTransform CS_RT = myPage.GetComponentInParent<CanvasScaler>().GetComponent<RectTransform>();
                                RT_Temp.position = new Vector3(X_Position * CS_RT.localScale.x, Y_Position * CS_RT.localScale.y, 0);
                                // Where to put the buttons
                                RectTransform[] Nav_Buttons = myPage.GetComponentsInChildren<RectTransform>();
                                foreach (RectTransform rt in Nav_Buttons)
                                {
                                    if (rt.gameObject.name == "Navigation Buttons")
                                    {
                                        temp.transform.SetParent(myPage.transform);
                                        RT_Temp.localScale = Vector3.one;
                                        break;
                                    }
                                }
                            }
                        }
                        if (myPage.CorrespondingPages.Count > 0)
                        {
                            if (GUILayout.Button("Clear Pages"))
                            {
                                myPage.ClearPages();
                            }
                        }
                    }
                }
            }

            myPage.gameObject.name = myPage.GetPageName;
            //myPage.GetTitleTextDisplay.SetDisplay(myPage.GetPageName, new Vector2(30, 30), new Vector2(100, 10), true, true);
        }
    }
}
