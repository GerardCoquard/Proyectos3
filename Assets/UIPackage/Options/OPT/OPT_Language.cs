using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OPT_Language : MonoBehaviour,IOption
{
    public TMP_Dropdown dropdown;
    bool lastFrameOpened = false;
    string [] languages;
    public OptionType optionType = OptionType.Others;
    OptionType IOption.type => optionType;
    public UnityEvent onOpen;
    public UnityEvent onClose;
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
        SelectableHandler selectable = dropdown.GetComponent<SelectableHandler>();
        SetLanguages();
        _description = FindObjectOfType<Description>();
        selectable.onHighlight.AddListener(SetDescription);
        selectable.onUnhighlight.AddListener(ClearDescription);
        dropdown.onValueChanged.AddListener(OnChange);
    }
    void OnChange(int _language)
    {
        LocalizationManager.ChangeLanguage(_language);
    }
    private void Update() {
        bool openedThisFrame = dropdown.transform.childCount != 4;
        if(openedThisFrame && !lastFrameOpened)
        {
            InputManager.GetAction("Back").action += CloseDropdown;
            onOpen?.Invoke();
        }
        if(!openedThisFrame && lastFrameOpened)
        {
            InputManager.GetAction("Back").action -= CloseDropdown;
            onClose?.Invoke();
            if(InputManager.device == Devices.Keyboard) EventSystem.current.SetSelectedGameObject(null);
        }
        lastFrameOpened = openedThisFrame;
    }
    void CloseDropdown(InputAction.CallbackContext context)
    {
        if(context.started) dropdown.Hide();
    }
    void SetLanguages()
    {
        languages = LocalizationManager.GetLanguages().ToArray();

        dropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (string _language in languages)
        {
            options.Add(_language);
        }
        dropdown.AddOptions(options);
        dropdown.SetValueWithoutNotify(LocalizationManager.GetCurrentLanguage());
        dropdown.RefreshShownValue();
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
        LocalizationManager.ChangeLanguage(0);
        dropdown.SetValueWithoutNotify(0);
        dropdown.RefreshShownValue();
    }
}
