using UnityEngine;
using UnityEngine.Rendering;
using System.Reflection;

namespace SOG.CVDFilter
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Volume))]
    public class CVDFilter : MonoBehaviour
    {
        public static CVDFilter Instance { get; private set; }

        private Volume postProcessVolume;

        private CVDProfilesSO profiles;
        [SerializeField] private VisionTypeNames currentType;

        public VisionTypeNames visionTypeNames { get { return currentType; } }

        public VisionTypeInfo SelectedVisionType { get; private set; }

        private const string soFileName = "CVDProfiles";

        void Awake()
        {
            if (Application.isPlaying)
            {
                if (Instance == null)
                {
                    Debug.Log("[Singleton] Initializing Instance " + typeof(CVDFilter) + "...");
                    Instance = this;
                    DontDestroyOnLoad(gameObject);

                }
                else if (Instance != this)
                {
                    Debug.Log("[Singleton - CVDFilter] Instance already exists: " + Instance);
                    Destroy(Instance.gameObject);
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }

                Setup();
            }
        }

        void Reset()
        {
            Setup();
            ChangeProfile();
        }

        void Start()
        {
            if (Instance == this)
            {
                LoadSavedProfile();
                ChangeProfile();
            }
        }

        private void Setup()
        {
            AssignProfileSO();
            ConfigureVolume();
        }

        void LoadSavedProfile()
        {
            if (PlayerPrefs.HasKey("SelectedVisionType"))
            {
                int savedType = PlayerPrefs.GetInt("SelectedVisionType");
                if (savedType >= 0 && savedType < System.Enum.GetValues(typeof(VisionTypeNames)).Length)
                {
                    currentType = (VisionTypeNames)savedType;
                }
            }
        }

        private void AssignProfileSO()
        {
            profiles = Resources.Load<CVDProfilesSO>(soFileName);
            if (profiles == null)
            {
                Debug.LogErrorFormat("[{0}] ({1}): Error - Unable to locate file \"{2}\". "
                + "There should be a single ScriptableObject called \"{2}.asset\" in Resources folder.", GetType().Name, MethodBase.GetCurrentMethod().Name, soFileName);
                return;
            }
            SelectedVisionType = profiles.VisionTypes[0];
        }

        private void ConfigureVolume()
        {
            postProcessVolume = GetComponent<Volume>();
            if (postProcessVolume == null)
            {
                return;
            }
            postProcessVolume.isGlobal = true;
        }

        public void ChangeProfile()
        {
            if (profiles == null)
            {
                return;
            }

            SelectedVisionType = profiles.VisionTypes[(int)currentType];

            if (postProcessVolume != null)
            {
                postProcessVolume.profile = SelectedVisionType.profile;
            }
        }

        public void ChangeCurrentType(VisionTypeNames newType)
        {

            currentType = newType;
            ChangeProfile();

            PlayerPrefs.SetInt("SelectedVisionType", (int)newType);
            PlayerPrefs.Save();
        }
    }

    public enum VisionTypeNames
    {
        Normal,
        Protanopia,
        Protanomalia,
        Deuteranopia,
        Deuteranomalia,
        Tritanopia,
        Tritanomalia,
        Achromatopsia,
        Achromatomalia
    }

    [System.Serializable]
    public struct VisionTypeInfo
    {
        public VisionTypeNames typeName;
        public string description;
        public VolumeProfile profile;
        public Texture2D previewImage;
    }
}