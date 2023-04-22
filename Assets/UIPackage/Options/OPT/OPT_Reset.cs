using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPT_Reset : MonoBehaviour
{
    public OptionType optionType;
    public Button button;
    List<IOption> options;
    string description;
    Description _description;
    private void OnEnable() {
        description = LocalizationManager.GetLocalizedValue("Description_"+this.GetType().ToString());
        LocalizationManager.onLanguageChange += () => description = LocalizationManager.GetLocalizedValue("Description_"+this.GetType().ToString());
    }
    private void OnDisable() {
        LocalizationManager.onLanguageChange -= () => description = LocalizationManager.GetLocalizedValue("Description_"+this.GetType().ToString());
    }
    private void Start() {
        SelectableHandler selectable = button.GetComponent<SelectableHandler>();
        options = new List<IOption>(GetComponents<IOption>());
        _description = FindObjectOfType<Description>();
        selectable.onHighlight.AddListener(SetDescription);
        selectable.onUnhighlight.AddListener(ClearDescription);
        button.onClick.AddListener(OnChange);
    }
    void OnChange()
    {
        foreach (IOption opt in options)
        {
            if(opt.type == optionType) opt.Reset();
        }
        SetDescription();
    }
    void SetDescription()
    {
        int index = description.IndexOf('*');
        string processedDescription = description.Remove(index);
        processedDescription = processedDescription.Insert(index,LocalizationManager.GetLocalizedValue(optionType.ToString()));
        _description.Set(processedDescription);
    }
    void ClearDescription()
    {
        _description.Clear();
    }
}
