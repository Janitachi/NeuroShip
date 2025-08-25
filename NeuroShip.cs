using OWML.Common;
using OWML.ModHelper;
using UnityEngine;

namespace NeuroShip
{
    public class NeuroShip : ModBehaviour
    {
        public static NeuroShip Instance;
        private static readonly Shader standardShader = Shader.Find("Standard");

        private GameObject _bow;
        private Texture _CampsiteProps;
        private Texture _SplashScreen;
        private Texture _PlayerShip_Posters;
        private Texture _PlayerShip_SignsDecal;
        private Texture _VillageCloth;
        private Texture _VillageClothOriginal;
        private bool bowactive;
        private bool faceactive;
        private bool breakingtexactive;
        private bool first = true;
        private bool loaded = false;
        private GameObject bow;
        private GameObject bow2;

        private void Start()
        {
            Instance = this;

            var bundle = ModHelper.Assets.LoadBundle("assets/bow");
            bowactive = ModHelper.Config.GetSettingsValue<bool>("Bow");
            faceactive = ModHelper.Config.GetSettingsValue<bool>("Face");
            breakingtexactive = ModHelper.Config.GetSettingsValue<bool>("BreakingTex");

            _bow = LoadPrefab(bundle, "Assets/Prefabs/bow.prefab");

            _CampsiteProps = LoadTex(bundle, "Assets/Texture/Props_HEA_CampsiteProps_da.png");
            _SplashScreen = LoadTex(bundle, "Assets/Texture/ShipComputer_SplashScreen_da.png");
            _PlayerShip_Posters = LoadTex(bundle, "Assets/Texture/Structure_HEA_PlayerShip_Posters_da.png");
            _PlayerShip_SignsDecal = LoadTex(bundle, "Assets/Texture/Structure_HEA_PlayerShip_SignsDecal_da.png");
            _VillageCloth = LoadTex(bundle, "Assets/Texture/Structure_HEA_VillageCloth_da.png");
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;

        }

        public override void Configure(IModConfig config)
        {
            bowactive = config.GetSettingsValue<bool>("Bow");
            faceactive = config.GetSettingsValue<bool>("Face");
            breakingtexactive = config.GetSettingsValue<bool>("BreakingTex");
            if (loaded) {
                if (bowactive)
                {
                    bow.SetActive(true);
                    bow2.SetActive(true); 
                }
                else
                {
                    bow.SetActive(false);
                    bow2.SetActive(false);
                }
                if (faceactive)
                {
                    var front = GameObject.Find("Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_GoldGlass").gameObject.GetComponent<MeshRenderer>().materials;
                    front[0].mainTexture = _VillageCloth;
                }
                else
                {
                    var front = GameObject.Find("Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_GoldGlass").gameObject.GetComponent<MeshRenderer>().materials;
                    front[0].mainTexture = _VillageClothOriginal;
                }
                if (breakingtexactive)
                {
                    GameObject.Find("Ship_Body/Module_Supplies/Geo_Supplies/Supplies_Geometry/Supplies_Interior").gameObject.GetComponent<MeshRenderer>().materials[5].mainTexture = _CampsiteProps;
                    GameObject.Find("Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Geometry/Cabin_Interior/Cabin_Interior 1/Cabin_Interior 1_MeshPart0").gameObject.GetComponent<MeshRenderer>().materials[2].mainTexture = _CampsiteProps;
                    Log("Broken Textures active, sadly couldn't fix the Lighting on that one... to repair the lighting turn the Option off and restart the game.");
                }
            }
            
        }

        private void OnDestroy()
        {
            LoadManager.OnCompleteSceneLoad -= OnCompleteSceneLoad;
        }

        private void OnCompleteSceneLoad(OWScene _, OWScene currentScene)
        {
            if (currentScene != OWScene.SolarSystem)
            {
                return;
            }
            loaded = true;
            Log("Changing materials and creating Bows");
            var ship = GameObject.Find("Ship_Body").gameObject;
            bow = GameObject.Instantiate(_bow, ship.transform);
            //bow.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            bow.transform.localPosition = new Vector3(-2.4f, 2.3f, -4.68f);
            bow.transform.Rotate(0f, -90f, 0f, Space.Self);
            bow.transform.localScale = new Vector3(2.34f, 2.4f,1.20453553f);
            bow2 = GameObject.Instantiate(_bow, ship.transform);
            //bow2.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            bow2.transform.localPosition = new Vector3(2.4f, 2.3f, 4.83f);
            bow2.transform.Rotate(0f, 90f, 0f, Space.Self);
            bow2.transform.localScale = new Vector3(2.34f, 2.4f, 1.20453553f);


            if (bowactive)
            {
                bow.SetActive(true);
                bow2.SetActive(true);
            }
            else {
                bow.SetActive(false);
                bow2.SetActive(false);
            }
            GameObject.Find("Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_Chassis").gameObject.GetComponent<MeshRenderer>().materials[3].mainTexture = _CampsiteProps;
            Log("Changed 1");
            GameObject.Find("Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_Chassis").gameObject.GetComponent<MeshRenderer>().materials[5].mainTexture = _PlayerShip_SignsDecal;
            Log("Changed 2");
            GameObject.Find("Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Geometry/Cabin_Exterior").gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = _PlayerShip_SignsDecal;
            Log("Changed 3");
            GameObject.Find("Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Geometry/Cabin_Exterior").gameObject.GetComponent<MeshRenderer>().materials[1].mainTexture = _CampsiteProps;
            Log("Changed 4");
            GameObject.Find("Ship_Body/Module_Supplies/Geo_Supplies/Supplies_Tech/Poster_LittleScout").gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = _PlayerShip_Posters;
            Log("Changed 5");
            GameObject.Find("Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Tech/Cabin_Tech_Interior/Cabin_Poster").gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = _PlayerShip_Posters;
            Log("Changed 6");
            GameObject.Find("Ship_Body/Module_Cabin/Systems_Cabin/ShipLogPivot/ShipLog/ShipLogPivot/ShipLogSplashScreen").gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = _SplashScreen;
            Log("Changed 7");           

            if (faceactive)
            {
                var front = GameObject.Find("Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_GoldGlass").gameObject.GetComponent<MeshRenderer>().materials;
                if (first) {
                    _VillageClothOriginal = front[0].mainTexture;
                    first = false;
                }
                front[0].mainTexture = _VillageCloth;
            }
            else {
                var front = GameObject.Find("Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_GoldGlass").gameObject.GetComponent<MeshRenderer>().materials;
                if (first)
                {
                    _VillageClothOriginal = front[0].mainTexture;
                    first = false;
                }
                front[0].mainTexture = _VillageClothOriginal;
            }
        }
        private static GameObject LoadPrefab(AssetBundle bundle, string path)
        {
            var prefab = bundle.LoadAsset<GameObject>(path);

            // Repair materials             
            foreach (var renderer in prefab.GetComponents<MeshRenderer>())
            {
                //renderer.shadowCastingMode = ShadowCastingMode.Off;
                foreach (var mat in renderer.materials)
                {
                    mat.shader = standardShader;
                    mat.renderQueue = 2000;
                }
            }
            prefab.SetActive(false);

            return prefab;
        }
        private static Texture LoadTex(AssetBundle bundle, string path)
        {
            var tex = bundle.LoadAsset<Texture2D>(path);
            return tex;
        }

        public static void Log(string message)
        {
            Instance.ModHelper.Console.WriteLine($"[NeuroShip]: {message}", MessageType.Info);
        }
    }
}