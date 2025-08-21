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


        private void Start()
        {
            Instance = this;

            var bundle = ModHelper.Assets.LoadBundle("assets/bow");

            _bow = LoadPrefab(bundle, "Assets/Prefabs/bow.prefab");

            _CampsiteProps = LoadTex(bundle, "Assets/Texture/Props_HEA_CampsiteProps_da.png");
            _SplashScreen = LoadTex(bundle, "Assets/Texture/ShipComputer_SplashScreen_da.png");
            _PlayerShip_Posters = LoadTex(bundle, "Assets/Texture/Structure_HEA_PlayerShip_Posters_da.png");
            _PlayerShip_SignsDecal = LoadTex(bundle, "Assets/Texture/Structure_HEA_PlayerShip_SignsDecal_da.png");
            _VillageCloth = LoadTex(bundle, "Assets/Texture/Structure_HEA_VillageCloth_da.png");
            
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;

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

            Log("Changing materials and creating Bows");
            var ship = GameObject.Find("Ship_Body").gameObject;
            var bow = GameObject.Instantiate(_bow, ship.transform);
            bow.transform.localPosition = new Vector3(-2.4f, 2.3f, -4.6f);
            bow.transform.Rotate(0f, -90f, 0f, Space.Self);
            bow.transform.localScale = Vector3.one * 2.3f;
            //bow.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            var bow2 = GameObject.Instantiate(_bow, ship.transform);
            bow2.transform.localPosition = new Vector3(2.4f, 2.3f, 4.75f);
            bow2.transform.Rotate(0f, 90f, 0f, Space.Self);
            bow2.transform.localScale = Vector3.one * 2.3f;
            //bow2.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;

            bow.SetActive(true);
            bow2.SetActive(true);

            var toUpdate = new string[]
            {
                "Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_Chassis",
                "Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Interior/Cockpit_Interior_Chassis",
                "Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Geometry/Cabin_Exterior",
                "Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Geometry/Cabin_Interior/Cabin_Interior 1/Cabin_Interior 1_MeshPart0",
                "Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Geometry/Cabin_Interior/Cabin_Interior 1/Cabin_Interior 1_MeshPart1",
                "Ship_Body/Module_Cabin/Geo_Cabin/Cabin_Tech/Cabin_Tech_Interior/Cabin_Poster",
                "Ship_Body/Module_Cabin/Systems_Cabin/ShipLogPivot/ShipLog/ShipLogPivot/ShipLogSplashScreen",
                "Ship_Body/Module_Supplies/Geo_Supplies/Supplies_Geometry/Supplies_Interior",
                "Ship_Body/Module_Supplies/Geo_Supplies/Supplies_Tech/Poster_LittleScout",
                "Ship_Body/Module_Engine/Geo_Engine/Engine_Geometry/Engine_Interior"
            };
            foreach (var s in toUpdate)
            {
                var work=GameObject.Find(s).gameObject.GetComponent<MeshRenderer>().materials;
                foreach (var mat in work)
                {
                    if (mat.mainTexture == null) Log($"Couldn't find {mat.name} MainTexture");
                    else mat.mainTexture = getTexture(mat);
                }
            }
            var front=GameObject.Find("Ship_Body/Module_Cockpit/Geo_Cockpit/Cockpit_Geometry/Cockpit_Exterior/CockpitExterior_GoldGlass").gameObject.GetComponent<MeshRenderer>().materials;
            front[0].mainTexture = _VillageCloth;
        }
        private Texture getTexture(Material startMat)
        {
            Texture foundmat= startMat.mainTexture;
            string name = startMat.name.Remove(startMat.name.Length - 11);
            if (name== "ShipExterior_HEA_CampsiteProps_mat") foundmat = _CampsiteProps;
            if (name == "ShipExterior_HEA_SignsDecal_mat") foundmat = _PlayerShip_SignsDecal;
            if (name == "ShipInterior_HEA_CampsiteProps_mat") foundmat = _CampsiteProps;
            if (name == "ShipInterior_HEA_SignsDecal_mat") foundmat = _PlayerShip_SignsDecal;
            if (name == "ShipInterior_HEA_ComputerSplashScreen_mat") foundmat = _SplashScreen;
            if (name == "Structure_HEA_PlayerShip_Posters_mat") foundmat = _PlayerShip_Posters;
            return foundmat;
        }

        private static GameObject LoadPrefab(AssetBundle bundle, string path)
        {
            var prefab = bundle.LoadAsset<GameObject>(path);

            // Repair materials             
            foreach (var renderer in prefab.GetComponents<MeshRenderer>())
            {
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