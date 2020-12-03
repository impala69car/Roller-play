using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Одиночка — это порождающий паттерн, который гарантирует существование только одного объекта определённого класса, а также позволяет достучаться до этого объекта из любого места программы.
public class LevelManager : BasicManager
{
    private LevelManager() { }
    [System.Serializable]
    private class SavedConfig
    {
        public int color;
        public int RotationNum = 1;
        public float row, c;
        public float sradius;
    }
    [SerializeField] Transform[] CameraPoints;
    private int stateCamera;
    private static LevelManager _instance;
    // У нас теперь есть объект-блокировка для синхронизации потоков во
    // время первого доступа к Одиночке.
    private static readonly object _lock = new object();
    [SerializeField] private Quaternion characterRot, CameraTartgetRot;
    private bool ClampVerticalRotation = false, smooth = true;
    private float YRot2, XRot2,newYRot=3.45f,newXRot=1.5f;
    private bool isScrolling=false;
    private static BasicManager basicManager;
    private Querable querable1;
    private void Awake()
    {
        basicManager = GameObject.FindObjectOfType<BasicManager>();querable1 = new Querable();
    }
    private static SavedConfig[] savedConfigs;
    private static SavedConfig[] LoadedConfigs;
    private static void InstaceSaved()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                _instance = new LevelManager();
            }
        }
        lock (_lock)
        {
            print("lock");
            PlayerPrefs.SetInt("SavedCount", basicManager.gems().Count); PlayerPrefs.Save();
            savedConfigs = new SavedConfig[basicManager.gems().Count];
            for (int i = 0; i < basicManager.gems().Count; i++)
            {
                SavedConfig saved = new SavedConfig();
                saved.color = basicManager.gems()[i].color;
                saved.RotationNum = basicManager.gems()[i].RotationNum;
                saved.row = basicManager.gems()[i].transform.position.z;
                saved.c = basicManager.gems()[i].transform.position.x;
                saved.sradius = basicManager.gems()[i].gameObject.transform.localScale.x;
                savedConfigs[i] = saved;
                string json = JsonUtility.ToJson(savedConfigs[i]);
                print(json);
                PlayerPrefs.SetString("SavedConfig" + i, json); PlayerPrefs.Save();
            }
        }
    }
    private static void InstanceLoaded()
    {
        LoadedConfigs = new SavedConfig[PlayerPrefs.GetInt("SavedCount")];
        for (int i = 0; i < PlayerPrefs.GetInt("SavedCount"); i++)
        {
            string jsonData = PlayerPrefs.GetString("SavedConfig"+i);
            LoadedConfigs[i] = JsonUtility.FromJson<SavedConfig>(jsonData);
        }
    }
    public void query(SphereGem[] spheres)
    {
        InstanceLoaded();
        for(int i=0;i< PlayerPrefs.GetInt("SavedCount");i++)
        {
            SphereGem sphereGem1 = new SphereGem();
            sphereGem1.color = LoadedConfigs[i].color;
            sphereGem1.RotationNum = LoadedConfigs[i].RotationNum;
            sphereGem1.row = LoadedConfigs[i].row;
            sphereGem1.col = LoadedConfigs[i].c;
            sphereGem1.sradius = LoadedConfigs[i].sradius;
            spheres[i] = sphereGem1;
        }
    }
    public void Init(Transform character, Transform camera)
    {
        characterRot = character.localRotation;
        CameraTartgetRot = camera.localRotation;
    }
    public void LookRotation(Transform character, Transform camera,float YRot,float XRot)
    {
        if (YRot > newYRot)// && XRot > newXRot)
        {
            characterRot *= Quaternion.Euler(0f, (YRot - YRot2) + myAngle, 0);
            CameraTartgetRot *= Quaternion.Euler(0f, (XRot - XRot2) + myAngle, 0);
        }
        else
        {
            characterRot *= Quaternion.Euler(0f, -(YRot - YRot2), 0);
            CameraTartgetRot *= Quaternion.Euler(0f,-(XRot - XRot2), 0);
        }
        if (ClampVerticalRotation)
        {

        }
        if (!smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, characterRot, smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, CameraTartgetRot, smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = characterRot;
            camera.localRotation = CameraTartgetRot;
        }
        XRot2 = 0;
        YRot2 = 0;
        
    }
    public float myAngle=0,alphapos;
    private readonly float smoothTime;

    private void transformCamera(int i)
    {
        Camera.main.transform.position = CameraPoints[i].position;
        Camera.main.transform.rotation = CameraPoints[i].rotation;
    }
    // Use this for initialization
    void Start()
    {
        Init(transform, Camera.main.transform);
        basicManager.SpawnCountQuery(querable1);
    }

    // Update is called once per frame
    void Update()
    {
        float YRot = Input.mousePosition.x * 0.008f;
        float XRot = Input.mousePosition.y * 0.008f;
        if (Input.GetMouseButtonDown(0))
        {
            isScrolling = true;
            YRot2 = Input.mousePosition.x * 0.008f;
            XRot2 = Input.mousePosition.y * 0.008f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isScrolling = false;
        }
        if (isScrolling == true)
        {
            LookRotation(transform, Camera.main.transform, YRot, XRot);
        }

    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(5, 2, 80, 20), "save"))
        {
            InstaceSaved();
            basicManager.queryRedBlue();
        }
        /*
        if (stateCamera == 0)
        {
            if (GUI.Button(new Rect(95, 20, 80, 20), "next>>"))
            {
                stateCamera = 1;
                transformCamera(1);
            }
            
        }
        if (stateCamera == 1)
        {
            if (GUI.Button(new Rect(5, 20, 80, 20), "<<back"))
            {
                stateCamera = 0; transformCamera(0);
            }
            if (GUI.Button(new Rect(95, 20, 80, 20), "next>>"))
            {
                stateCamera = 2; transformCamera(2);
            }
        }
        if (stateCamera == 2)
        {
            if (GUI.Button(new Rect(5, 20, 80, 20), "<<back"))
            {
                stateCamera = 1; transformCamera(1);
            }
        }*/
    }
}
