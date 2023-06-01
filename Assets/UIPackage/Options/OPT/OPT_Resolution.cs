using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OPT_Resolution : MonoBehaviour,IOption
{
    public TMP_Dropdown dropdown;
    Resolution[] resolutions;
    bool lastFrameOpened = false;
    public OptionType optionType = OptionType.Graphics;
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
        SetResolutions();
        _description = FindObjectOfType<Description>();
        selectable.onHighlight.AddListener(SetDescription);
        selectable.onUnhighlight.AddListener(ClearDescription);
        dropdown.onValueChanged.AddListener(OnChange);
    }
    
    void OnChange(int res)
    {
        OptionsManager.resolution = res;
        Resolution resolution = resolutions[res];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
        if(context.ReadValueAsButton()) dropdown.Hide();
    }
    void SetResolutions()
    {
        resolutions = GetResolutions().ToArray();

        dropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        dropdown.AddOptions(options);
        if(OptionsManager.defaultResolution == OptionsManager.defaultData.resolution)
        {
            OptionsManager.defaultResolution = currentResolutionIndex;
            OptionsManager.resolution = currentResolutionIndex;
        }
        dropdown.SetValueWithoutNotify(OptionsManager.resolution);
        dropdown.RefreshShownValue();
    }
    List<Resolution> GetResolutions()
    {
        //Filters out all resolutions with low refresh rate:
        Resolution[] resolutions = Screen.resolutions;
        HashSet<System.ValueTuple<int, int>> uniqResolutions = new HashSet<System.ValueTuple<int, int>>();
        Dictionary<System.ValueTuple<int, int>, int> maxRefreshRates = new Dictionary<System.ValueTuple<int, int>, int>();
        for (int i = 0; i < resolutions.GetLength(0); i++)
        {
            //Add resolutions (if they are not already contained)
            System.ValueTuple<int, int> resolution = new System.ValueTuple<int, int>(resolutions[i].width, resolutions[i].height);
            uniqResolutions.Add(resolution);
            //Get highest framerate:
            if (!maxRefreshRates.ContainsKey(resolution))
            {
                maxRefreshRates.Add(resolution, resolutions[i].refreshRate);
            }
            else
            {
                maxRefreshRates[resolution] = resolutions[i].refreshRate;
            }
        }
        //Build resolution list:
        List<Resolution> uniqResolutionsList = new List<Resolution>(uniqResolutions.Count);
        foreach (System.ValueTuple<int, int> resolution in uniqResolutions)
        {
            Resolution newResolution = new Resolution();
            newResolution.width = resolution.Item1;
            newResolution.height = resolution.Item2;
            if (maxRefreshRates.TryGetValue(resolution, out int refreshRate))
            {
                newResolution.refreshRate = refreshRate;
            }
            uniqResolutionsList.Add(newResolution);
        }
        return uniqResolutionsList;
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
        OptionsManager.resolution = OptionsManager.defaultResolution;
        dropdown.SetValueWithoutNotify(OptionsManager.resolution);
        dropdown.RefreshShownValue();
        Resolution resolution = resolutions[OptionsManager.resolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
