using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(ImageDisplay))]
public class ImageDisplayEditor : Editor
{
    // Script Variables
    ImageDisplay myImage; // Target
    SerializedProperty m_ImageScript;
    SerializedProperty m_TextScript;
    SerializedProperty m_EdgeSize;
    SerializedProperty m_BufferSize;
    SerializedProperty m_TextStr;
    SerializedProperty m_ButtonScript;
    SerializedProperty m_ButtonActive;
    SerializedProperty m_ButtonInteractable;
    SerializedProperty m_Center;
    SerializedProperty m_UseCenter;
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
        m_ImageScript = serializedObject.FindProperty("DisplayImage");
        m_EdgeSize = serializedObject.FindProperty("EdgePixelSize");
        m_BufferSize = serializedObject.FindProperty("TitleBufferSize");
        m_TextStr = serializedObject.FindProperty("DisplayStr");
        m_ButtonScript = serializedObject.FindProperty("DisplayButton");
        m_ButtonActive = serializedObject.FindProperty("DisplayButtonActive");
        m_ButtonInteractable = serializedObject.FindProperty("DisplayButtonInteractable");
        m_Center = serializedObject.FindProperty("Center");
        m_CenterAt = serializedObject.FindProperty("CenterAt");
        m_UseCenter = serializedObject.FindProperty("UseCenter");

        // Get Target entity
        myImage = (ImageDisplay)target;
        // Get children
        Children = myImage.GetComponentsInChildren<RectTransform>();
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
        EditorGUILayout.PropertyField(m_ImageScript, new GUIContent("Image Script"));
        EditorGUILayout.PropertyField(m_TextScript, new GUIContent("Text Script"));
        EditorGUILayout.PropertyField(m_ButtonScript, new GUIContent("Button Script"));

        // Display
        if (myImage.GetDisplayImage != null)
        {
            // Display Variables
            EditorGUILayout.PropertyField(m_TextStr, new GUIContent("Text"));
            EditorGUILayout.PropertyField(m_UseCenter, new GUIContent("Use Center Coord"));
            EditorGUILayout.PropertyField(m_Center, new GUIContent("Center Location"));
            EditorGUILayout.PropertyField(m_CenterAt, new GUIContent("Center To"));
            EditorGUILayout.PropertyField(m_EdgeSize, new GUIContent("Edge Size"));
            EditorGUILayout.PropertyField(m_BufferSize, new GUIContent("Buffer Size"));

        }

        if (myImage.GetDisplayButton != null)
        {
            EditorGUILayout.PropertyField(m_ButtonActive, new GUIContent("Button Starts Active"));
            EditorGUILayout.PropertyField(m_ButtonInteractable, new GUIContent("Button Starts Interactable"));

        }

        // Update Inspector and change variables
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        // Show Effective Changes
        if (myImage.GetDisplayText != null)
        {
            myImage.GetDisplayText.text = myImage.GetDisplayStr;
            Resize();
        }

        if (myImage.GetDisplayButton != null)
        {
            myImage.GetDisplayButton.interactable = myImage.DisplayButtonInteractable;
            myImage.GetDisplayButton.image.enabled = myImage.DisplayButtonActive;
            myImage.GetDisplayButton.enabled = myImage.DisplayButtonActive;
        }
    }

    private void Resize()
    {
        float prefferedH = myImage.GetDisplayText.preferredHeight;
        float prefferedW = myImage.GetDisplayText.preferredWidth;
        float ImageH = myImage.GetDisplayImage.rectTransform.rect.height;
        float ImageW = myImage.GetDisplayImage.rectTransform.rect.width;
        Vector2 BaseObjectResize = new Vector2(ImageW, ImageH) + myImage.GetTitleBufferSize;
        Vector2 EdgeObjectResize = BaseObjectResize + myImage.GetEdgeSize;

        myImage.GetComponent<RectTransform>().sizeDelta = EdgeObjectResize;
        if (TextChild)
            TextChild.sizeDelta = BaseObjectResize;
        BackgroundChild.sizeDelta = BaseObjectResize;
        EdgeChild.sizeDelta = EdgeObjectResize;
        ButtonChild.sizeDelta = EdgeObjectResize;
        if (myImage.UseCenter)
        {
            Vector2 AmountToMove = Vector2.zero;
            if (myImage.GetCenterAt == ImageDisplay.ImageCenter.LeftTop)
            {
                AmountToMove = new Vector2(EdgeObjectResize.x / 2 + myImage.GetCenter.x, -EdgeObjectResize.y / 2 + myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.LeftMiddle)
            {
                AmountToMove = new Vector2(EdgeObjectResize.x / 2 + myImage.GetCenter.x, myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.LeftBottom)
            {
                AmountToMove = new Vector2(EdgeObjectResize.x / 2 + myImage.GetCenter.x, EdgeObjectResize.y / 2 + myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.CenterTop)
            {
                AmountToMove = new Vector2(myImage.GetCenter.x, -EdgeObjectResize.y / 2 + myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.CenterMid)
            {
                AmountToMove = new Vector2(myImage.GetCenter.x, myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.CenterBottom)
            {
                AmountToMove = new Vector2(myImage.GetCenter.x, EdgeObjectResize.y / 2 + myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.RightTop)
            {
                AmountToMove = new Vector2(-EdgeObjectResize.x / 2 + myImage.GetCenter.x, -EdgeObjectResize.y / 2 + myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.RightMiddle)
            {
                AmountToMove = new Vector2(-EdgeObjectResize.x / 2 + myImage.GetCenter.x, myImage.GetCenter.y);
            }
            else if (myImage.GetCenterAt == ImageDisplay.ImageCenter.RightBottom)
            {
                AmountToMove = new Vector2(-EdgeObjectResize.x / 2 + myImage.GetCenter.x, EdgeObjectResize.y / 2 + myImage.GetCenter.y);
            }
            RectTransform RT = myImage.GetComponent<RectTransform>();
            try
            {
                RectTransform CS_RT = myImage.GetComponentInParent<CanvasScaler>().GetComponent<RectTransform>();
                //Debug.Log("Amount to Move: " + AmountToMove + ", Scale Factor: " + CS_RT.localScale.x + ", Amount to Move*: " + AmountToMove * CS_RT.localScale.x);
                RT.position = new Vector3(AmountToMove.x * CS_RT.localScale.x, AmountToMove.y * CS_RT.localScale.y, myImage.transform.position.z);
            }
            catch
            {
                RT.position = new Vector3(AmountToMove.x, AmountToMove.y, myImage.transform.position.z);
            }
        }
    }
}
