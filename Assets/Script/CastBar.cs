using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CastBar
{
    [SerializeField]
    public string name;

    [SerializeField]
    public Sprite sprite;

    [SerializeField]
    public float castSpeed;

    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public Color barColor;

    public string Name { get; }
    public Sprite Sprite { get; }
    public float CastSpeed{ get; }
    public GameObject Prefab { get; }
    public Color BarColor { get; }
}
