using UnityEngine;
using Zork.Common;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    private TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    private Image NewLinePrefab;

    [SerializeField]
    private Transform ContentTransform;

    [SerializeField]
    private ScrollRect ScrollBar;

    [SerializeField]
    [Range(0, 100)]
    private int MaxEntries = 60;

    public void Write(string message) => ParseAndWriteLine(message);

    public void Write(object obj) => ParseAndWriteLine(obj.ToString());

    public void WriteLine(string message) => ParseAndWriteLine(message);

    public void WriteLine(object obj) => ParseAndWriteLine(obj.ToString());

    private void ParseAndWriteLine(string message)
    {
        if(_entries.Count >= MaxEntries)
        {
            Destroy(_entries.Dequeue());
        }
        var textLine = Instantiate(TextLinePrefab, ContentTransform);
        textLine.text = message;
        _entries.Enqueue(textLine.gameObject);

        StartCoroutine(ApplyScrollPosition(ScrollBar, 0));

    }

    IEnumerator ApplyScrollPosition(ScrollRect scrollbar, float position)
    {
        yield return new WaitForEndOfFrame();
        scrollbar.verticalNormalizedPosition = position;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)scrollbar.transform);
    }

    private Queue<GameObject> _entries = new Queue<GameObject>();
}
