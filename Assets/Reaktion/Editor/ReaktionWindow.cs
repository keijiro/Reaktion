//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;

public class ReaktionWindow : EditorWindow
{
    const int updateInterval = 15;

    Vector2 scrollPosition;
    int updateCounter;

    [MenuItem ("Window/Reaktion")]
    static void Init ()
    {
        EditorWindow.GetWindow<ReaktionWindow> ("Reaktion");
    }

    void OnEnable ()
    {
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
    }

    public void OnPlaymodeStateChanged ()
    {
        autoRepaintOnSceneChange = !EditorApplication.isPlaying;
        Repaint ();
    }

    void Update ()
    {
        if (EditorApplication.isPlaying)
        {
            if (updateCounter % updateInterval == 0) Repaint ();
            updateCounter++;
        }
    }

    void OnGUI ()
    {
        scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);

        GUILayout.Label ("Reaktor List", EditorStyles.boldLabel);

        foreach (var reaktor in FindObjectsOfType<Reaktor> ())
        {
            EditorGUILayout.BeginHorizontal ();

            if (EditorApplication.isPlaying)
            {
                var value = EditorGUILayout.Slider (reaktor.name, reaktor.Output, 0, 1);
                if (value != reaktor.Output) reaktor.OverrideOutput (value);
            }
            else
            {
                EditorGUILayout.Slider (reaktor.name, 0, 0, 1);
            }

            if (reaktor.Overridden)
            {
                if (GUILayout.Button ("Release", EditorStyles.miniButtonRight, GUILayout.Width (48)))
                    reaktor.StopOverriding();
            }
            else
            {
                if (GUILayout.Button ("Select", EditorStyles.miniButtonRight, GUILayout.Width (48)))
                    Selection.activeGameObject = reaktor.gameObject;
            }

            EditorGUILayout.EndHorizontal ();
        }

        EditorGUILayout.EndScrollView ();
    }
}
