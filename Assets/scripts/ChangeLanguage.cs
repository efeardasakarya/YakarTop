using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Collections.Generic;

public class LanguageDropdownTMP : MonoBehaviour
{
    public TMP_Dropdown dropdown; // TMP Dropdown
    private bool initialized = false;

    void Start()
    {
        StartCoroutine(SetupDropdown());
    }

    IEnumerator SetupDropdown()
    {
        yield return LocalizationSettings.InitializationOperation;

        dropdown.ClearOptions();

        List<string> languageNames = new List<string>();
        int currentIndex = 0;

        var locales = LocalizationSettings.AvailableLocales.Locales;

        for (int i = 0; i < locales.Count; i++)
        {
            var locale = locales[i];
            languageNames.Add(locale.LocaleName); // örn: English, Türkçe

            if (LocalizationSettings.SelectedLocale == locale)
                currentIndex = i;
        }

        dropdown.AddOptions(languageNames);
        dropdown.value = currentIndex;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(ChangeLanguage);
        initialized = true;
    }

    void ChangeLanguage(int index)
    {
        if (!initialized) return;

        var selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = selectedLocale;

        Debug.Log("Dil deðiþtirildi: " + selectedLocale.LocaleName);
    }
}
