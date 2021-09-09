using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class InputFieldHandler: MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    public void Awake()
    {
        var args = Environment.GetCommandLineArgs();

        if (args.Length >= 2 && !args[0].Contains("Unity.exe"))
        {
            _inputField.text = args[1];
        }
    }
}