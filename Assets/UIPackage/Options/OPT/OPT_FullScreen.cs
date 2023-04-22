using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPT_FullScreen : MonoBehaviour,IOption
{
    public Toggle toggle;
    public OptionType optionType = OptionType.Graphics;
    OptionType IOption.type => optionType;
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
        SelectableHandler selectable = toggle.GetComponent<SelectableHandler>();
        toggle.SetIsOnWithoutNotify(OptionsManager.fullScreen);
        _description = FindObjectOfType<Description>();
        selectable.onHighlight.AddListener(SetDescription);
        selectable.onUnhighlight.AddListener(ClearDescription);
        toggle.onValueChanged.AddListener(OnChange);
    }

    void OnChange(bool isOn)
    {
        OptionsManager.fullScreen = isOn;
        Screen.fullScreen = isOn;
    }
    void SetDescription()
    {
        _description.Set(description);
    }
    void ClearDescription()
    {
        _description.Clear();
    }
    public void Reset()
    {
        OptionsManager.fullScreen = OptionsManager.defaultData.fullScreen;
        toggle.SetIsOnWithoutNotify(OptionsManager.fullScreen);
    }
}
