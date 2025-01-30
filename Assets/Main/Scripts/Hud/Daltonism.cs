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
        {
            cvdFilter = CVDFilter.Instance;
        }
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
        {
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }

        if (prevButton != null)
        {
            prevButton.onClick.AddListener(OnPrevButtonClicked);
        }
    }

    void CleanupButtons()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(OnNextButtonClicked);
        }

        if (prevButton != null)
        {
            prevButton.onClick.RemoveListener(OnPrevButtonClicked);
        }
    }

    public void SetCVDFilter(CVDFilter filter)
    {
        cvdFilter = filter;
        ChangeVisionType(0);
    }

    void OnNextButtonClicked()
    {
        ChangeVisionType(1);
    }

    void OnPrevButtonClicked()
    {
        ChangeVisionType(-1);
    }

    public void ChangeVisionType(int direction)
    {
        if (cvdFilter == null)
        {
            Debug.LogError("Erro: CVDFilter não foi atribuído corretamente!");
            return;
        }

        int currentTypeIndex = (int)cvdFilter.visionTypeNames;
        int newTypeIndex = currentTypeIndex + direction;

        int visionTypesCount = Enum.GetValues(typeof(VisionTypeNames)).Length;
        if (newTypeIndex < 0) newTypeIndex = visionTypesCount - 1;
        else if (newTypeIndex >= visionTypesCount) newTypeIndex = 0;

        VisionTypeNames newVisionType = (VisionTypeNames)newTypeIndex;

        cvdFilter.ChangeCurrentType(newVisionType);
        cvdFilter.ChangeProfile();

        visionTypeText.text = newVisionType.ToString();
    }

    public void ChangeVisionTypeByString(string typeName)
    {
        if (Enum.TryParse(typeName, out VisionTypeNames visionType))
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
                GetType().Name, MethodBase.GetCurrentMethod().Name, typeName);
        }
    }

    void UpdateVisionTypeText()
    {
        if (visionTypeText != null && cvdFilter != null)
        {
            visionTypeText.text = cvdFilter.visionTypeNames.ToString();
        }
        else
        {
            Debug.LogWarning("VisionTypeText ou CVDFilter não estão definidos.");
        }
    }

    void SaveVisionTypePreference(VisionTypeNames visionType)
    {
        PlayerPrefs.SetInt(VisionTypePrefKey, (int)visionType);
        PlayerPrefs.SetString(VisionTypeStringPrefKey, visionType.ToString());
        PlayerPrefs.Save();
    }

    void LoadVisionTypePreference()
    {
        if (cvdFilter != null)
        {
            if (PlayerPrefs.HasKey(VisionTypePrefKey))
            {
                int savedVisionType = PlayerPrefs.GetInt(VisionTypePrefKey);
                VisionTypeNames visionType = (VisionTypeNames)savedVisionType;
                cvdFilter.ChangeCurrentType(visionType);
            }
            else
            {
                cvdFilter.ChangeCurrentType(VisionTypeNames.Normal);
            }
            UpdateVisionTypeText();
        }
        else
        {
            Debug.LogError("CVDFilter is not set.");
        }
    }
}