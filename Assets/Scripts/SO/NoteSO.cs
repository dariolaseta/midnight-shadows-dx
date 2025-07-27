using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Notes", menuName = "Items/Create new Note")]
public class NoteSO : ScriptableObject
{
    [TextArea(10, 10)]
    [SerializeField] private string text;

    [SerializeField] private string title;
    
    public string Text => text;
    public string Title => title;
}
