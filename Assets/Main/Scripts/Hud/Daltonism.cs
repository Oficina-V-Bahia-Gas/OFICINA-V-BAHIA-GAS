using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using SOG.CVDFilter;
using System.Reflection;

public class Daltonism : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;
    public TextMeshProUGUI visionTypeText;
    public CVDFilter cvdFilter;

    const string VisionTypePrefKey = "SelectedVisionType";
    const string VisionTypeStringPrefKey = "SelectedVisionTypeString";

    void Awake()
    {
        if (cvdFilter == null)
            cvdFilter = CVDFilter.Instance;
    }

    void OnEnable()
    {
        LoadVisionTypePreference();
        SetupButtons();
    }

    void OnDisable()
    {
        CleanupButtons();
    }

    void SetupButtons()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClicked);

        if (prevButton != null)
            prevButton.onClick.AddListener(OnPrevButtonClicked);
    }

    void CleanupButtons()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(OnNextButtonClicked);

        if (prevButton != null)
            prevButton.onClick.RemoveListener(OnPrevButtonClicked);
    }

    public void SetCVDFilter(CVDFilter _filter)
    {
        cvdFilter = _filter;
        ChangeVisionType(0);
    }

    void OnNextButtonClicked() => ChangeVisionType(1);
    void OnPrevButtonClicked() => ChangeVisionType(-1);

    public void ChangeVisionType(int _direction)
    {
        if (cvdFilter == null)
        {
            Debug.LogError("Erro: CVDFilter não foi atribuído corretamente!");
            return;
        }

        int _currentTypeIndex = (int)cvdFilter.visionTypeNames;
        int _newTypeIndex = _currentTypeIndex + _direction;

        int _visionTypesCount = Enum.GetValues(typeof(VisionTypeNames)).Length;
        if (_newTypeIndex < 0) _newTypeIndex = _visionTypesCount - 1;
        else if (_newTypeIndex >= _visionTypesCount) _newTypeIndex = 0;

        VisionTypeNames _newVisionType = (VisionTypeNames)_newTypeIndex;

        cvdFilter.ChangeCurrentType(_newVisionType);
        cvdFilter.ChangeProfile();

        visionTypeText.text = _newVisionType.ToString();
    }

    public void ChangeVisionTypeByString(string _typeName)
    {
        if (Enum.TryParse(_typeName, out VisionTypeNames visionType))
        {
            if (cvdFilter != null)
            {
                cvdFilter.ChangeCurrentType(visionType);
                SaveVisionTypePreference(visionType);
                UpdateVisionTypeText();
            }
        }
        else
        {
            Debug.LogErrorFormat("[{0}] ({1}): Error - Invalid vision type name \"{2}\".",
                GetType().Name, MethodBase.GetCurrentMethod().Name, _typeName);
        }
    }

    void UpdateVisionTypeText()
    {
        if (visionTypeText != null && cvdFilter != null)
            visionTypeText.text = cvdFilter.visionTypeNames.ToString();
        else
            Debug.LogWarning("VisionTypeText ou CVDFilter não estão definidos.");
    }

    void SaveVisionTypePreference(VisionTypeNames _visionType)
    {
        PlayerPrefs.SetInt(VisionTypePrefKey, (int)_visionType);
        PlayerPrefs.SetString(VisionTypeStringPrefKey, _visionType.ToString());
        PlayerPrefs.Save();
    }

    void LoadVisionTypePreference()
    {
        if (cvdFilter != null)
        {
            if (PlayerPrefs.HasKey(VisionTypePrefKey))
            {
                int _savedVisionType = PlayerPrefs.GetInt(VisionTypePrefKey);
                VisionTypeNames _visionType = (VisionTypeNames)_savedVisionType;
                cvdFilter.ChangeCurrentType(_visionType);
            }
            else
                cvdFilter.ChangeCurrentType(VisionTypeNames.Normal);

            UpdateVisionTypeText();
        }
        else
            Debug.LogError("CVDFilter is not set.");
    }
}