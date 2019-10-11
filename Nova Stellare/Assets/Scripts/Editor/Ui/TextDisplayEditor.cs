using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(TextDisplay))]
[CanEditMultipleObjects]
public class TextDisplayEditor : Editor
{
    // Script Variables
    TextDisplay myTD; // Target
    SerializedProperty m_TextScript;
    SerializedProperty m_EdgeSize;
    SerializedProperty m_BufferSize;
    SerializedProperty m_TextStr;
    SerializedProperty m_ButtonScript;
    SerializedProperty m_ButtonActive;
    SerializedProperty m_ButtonInteractable;
    SerializedProperty m_Center;
    SerializedProperty m_CenterAt;

    // Editor Variables
    RectTransform[] Children;
    RectTransform EdgeChild;
    RectTransform BackgroundChild;
    RectTransform ButtonChild;
    RectTransform TextChild;


    private void OnEnable()
    {
        // Find variables
        m_TextScript = serializedObject.FindProperty("DisplayText");
        m_EdgeSize = serializedObject.FindProperty("EdgePixelSize");
        m_BufferSize = serializedObject.FindProperty("TitleBufferSize");
        m_TextStr = serializedObject.FindProperty("DisplayStr");
        m_ButtonScript = serializedObject.FindProperty("DisplayButton");
        m_ButtonActive = serializedObject.FindProperty("DisplayButtonActive");
        m_ButtonInteractable = serializedObject.FindProperty("DisplayButtonInteractable");
        m_Center = serializedObject.FindProperty("Center");
        m_CenterAt = serializedObject.FindProperty("CenterAt");

        // Get Target entity
        myTD = (TextDisplay)target;
        // Get children
        Children = myTD.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform RT in Children)
        {
            if (RT.name == "Edge")
                EdgeChild = RT;
            else if (RT.name == "Background")
                BackgroundChild = RT;
            else if (RT.name == "Text")
                TextChild = RT;
            else if (RT.name == "Button")
                ButtonChild = RT;
        }
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        // Layout Base Variables
        EditorGUILayout.PropertyField(m_TextScript, new GUIContent("Text Script"));
        EditorGUILayout.PropertyField(m_ButtonScript, new GUIContent("Button Script"));

        // Display
        if (myTD.GetDisplayText != null)
        {
            // Display Variables
            EditorGUILayout.PropertyField(m_TextStr, new GUIContent("Text"));
            EditorGUILayout.PropertyField(m_Center, new GUIContent("Center Location"));
            EditorGUILayout.PropertyField(m_CenterAt, new GUIContent("Center To"));
            EditorGUILayout.PropertyField(m_EdgeSize, new GUIContent("Edge Size"));
            EditorGUILayout.PropertyField(m_BufferSize, new GUIContent("Buffer Size"));

        }

        if (myTD.GetDisplayButton != null)
        {
            EditorGUILayout.PropertyField(m_ButtonActive, new GUIContent("Button Starts Active"));
            EditorGUILayout.PropertyField(m_ButtonInteractable, new GUIContent("Button Starts Interactable"));

        }

        // Update Inspector and change variables
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        // Show Effective Changes
        if (myTD.GetDisplayText != null)
        {
            myTD.GetDisplayText.text = myTD.GetDisplayStr;
            Resize();
        }

        if (myTD.GetDisplayButton != null)
        {
            myTD.GetDisplayButton.interactable = myTD.DisplayButtonInteractable;
            myTD.GetDisplayButton.image.enabled = myTD.DisplayButtonActive;
            myTD.GetDisplayButton.enabled = myTD.DisplayButtonActive;
        }
    }

    public void Resize()
    {
        myTD.Resize();
    }
}