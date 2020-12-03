using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Querable
{
    public int gameAreaWidth, gameAreaHeight, numUnitsToSpawn;
    public float unitSpawnDelay, unitSpawnMinRadius, unitSpawnMaxRadius, unitSpawnMinSpeed, unitSpawnMaxSpeed, unitDestroyRadius;
}
public class BasicManager : MonoBehaviour
{
   
    [System.Serializable]
    private class GameConfig
    {
        public int gameAreaWidth, gameAreaHeight, numUnitsToSpawn;
        public float unitSpawnDelay, unitSpawnMinRadius, unitSpawnMaxRadius, unitSpawnMinSpeed, unitSpawnMaxSpeed, unitDestroyRadius;

        public GameConfig(int width,int height,int num,float spawnDelay,float spawnMinRadius,
            float spawnMaxRadius,float spawnMinSpeed, float spawnMaxSpeed,float destroyRadius)
        {
            gameAreaWidth = width;
            gameAreaHeight = height;
            numUnitsToSpawn = num;
            unitSpawnDelay = spawnDelay;unitSpawnMinRadius = spawnMinRadius;unitSpawnMaxRadius = spawnMaxRadius;
            unitSpawnMinSpeed = spawnMinSpeed;unitSpawnMaxSpeed = spawnMaxSpeed;unitDestroyRadius = destroyRadius;
        }
    }
    private static GameConfig databaseTable;
    protected const string _saveFolderName = "";
    string saveString = "saver";
    private static Vector3 vector2position;
    [SerializeField]private static List<Vector3> sphereAll = new List<Vector3>();
    [SerializeField] SphereGem[] spherePrefab;
    private static int sec = 1;
    [SerializeField]private List<SphereGem> GetSpheres = new List<SphereGem>();
    [SerializeField] private Text GetText;
    public SphereGem[] spheresPref()
    {
        return spherePrefab;
    }
    public void AddSphereVec(Vector3 item)
    {
        sphereAll.Add(item);
    }
    public void loadspVector(List<Vector3> vector3s)
    {
        vector3s = sphereAll;
    }
    public void clearSphereVec()
    {
        sphereAll.Clear();
    }
    public List<SphereGem> gems()
    {
        return GetSpheres;
    }
    public void RemoveSphere(SphereGem sp)
    {

        if (sp.name == "robot(Clone)") { blue -= 1; }
        if (sp.name == "robot2(Clone)") { red -= 1; }
        GetSpheres.Remove(sp);
        Destroy(sp.gameObject);
    }
    public void AddSphere(SphereGem sp)
    {
        GetSpheres.Add(sp);
    }
    public void ClearSpheres()
    {
        foreach (SphereGem sp in GetSpheres)
        {
            Destroy(sp.gameObject);
        }
        GetSpheres.Clear();
    }
    public int LenghtSphere()
    {
       return GetSpheres.Count;
    }
    public float numUnitsToSpawnLoad()
    {
        return databaseTable.unitDestroyRadius;
    }
    public static void GetInstance()
    {
        vector2position = new Vector3(0, 2.5f, 0);
        if (databaseTable == null)
        {
            SaveTable();
        }
    }
    private static void SaveTable()
    {
        databaseTable = new GameConfig(100, 100, 100,5,0.5f,1,5,10,0.2f);
        string json = JsonUtility.ToJson(databaseTable);
        PlayerPrefs.SetString("GameConfig", json);PlayerPrefs.Save();
        GameObject.Find("Cube").transform.localScale = new Vector3(databaseTable.gameAreaWidth, 9.2f, 1);
        GameObject.Find("Cube (1)").transform.localScale = new Vector3(databaseTable.gameAreaWidth, 9.2f, 1);
        GameObject.Find("Cube (2)").transform.localScale = new Vector3(databaseTable.gameAreaHeight, 9.2f, 1);
        GameObject.Find("Cube (3)").transform.localScale = new Vector3(databaseTable.gameAreaHeight, 9.2f, 1);
    }
    private void LoadTable()
    {
        string jsonData = PlayerPrefs.GetString("GameConfig");
        print(jsonData);
        GameConfig loadedData = JsonUtility.FromJson<GameConfig>(jsonData);
    }
    private void Awake()
    {
        GetInstance();
    }
    // Start is called before the first frame update
    void Start()
    {
        //GetInstance();
        CreateGem(Random.Range(0, 100), Random.Range(0, 100), spherePrefab[Random.Range(0, spherePrefab.Length)]);
        StartCoroutine(SpawnSphereCoroutine());
    }
    public void query(Querable quer)
    {
        quer.gameAreaHeight = databaseTable.gameAreaHeight;
        quer.gameAreaWidth = databaseTable.gameAreaWidth;
        quer.numUnitsToSpawn = databaseTable.numUnitsToSpawn;
        quer.unitSpawnDelay = databaseTable.unitSpawnDelay;
        quer.unitSpawnMinRadius = databaseTable.unitSpawnMinRadius;
        quer.unitSpawnMaxRadius = databaseTable.unitSpawnMaxRadius;
        quer.unitSpawnMinSpeed = databaseTable.unitSpawnMinSpeed;
        quer.unitSpawnMaxSpeed = databaseTable.unitSpawnMaxSpeed;
        quer.unitDestroyRadius = databaseTable.unitDestroyRadius;
        SaveTable();
    }
    int red=0, blue=0;
    public void CreateGem(int row, int c, SphereGem hgem)
    {
        if (!sphereAll.Contains(new Vector3(c, 0, row)))
        {
            Vector3 vectorgem = vector2position + (new Vector3(c, 0, row));
            sphereAll.Add(vectorgem);
            float radiussphere = Random.Range(databaseTable.unitSpawnMinRadius, databaseTable.unitSpawnMaxRadius);
            SphereGem sphere = ((GameObject)Instantiate(hgem.gameObject, vectorgem, Quaternion.identity)).GetComponent<SphereGem>();
            sphere.transform.localScale = new Vector3(
                sphere.transform.localScale.x * radiussphere, 
                sphere.transform.localScale.y * radiussphere, 
                sphere.transform.localScale.z * radiussphere
                );
            sphere.RotationNum = Random.Range(1, 4);
            if (sphere.name == "robot(Clone)") { blue += 1; }
            if (sphere.name == "robot2(Clone)") { red += 1; }
            //sphere.RotAll();
            GetSpheres.Add(sphere);
        }
        else
        {
            CreateGem(Random.Range(0, 100), Random.Range(0, 100), spherePrefab[Random.Range(0, spherePrefab.Length)]);
        }
    }
    void WinLoad()
    {
        StopCoroutine(SpawnSphereCoroutine());
        ClearSpheres();
        red = 0;blue = 0;
    }
    bool restart;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) { LoadTable(); }
        if(restart==false) GetText.text = "red:" + red + "/blue:" + blue;
        if (red >= 10) { GetText.text = "red win:"+ red + "/" + blue; WinLoad(); restart = true; }
        if (blue >= 10) { GetText.text = "blue win:" + red + "/" + blue; WinLoad(); restart = true; }
    }
    public void SpawnSpheresForCircle(int loadnum)
    {

    }
    public void SpawnCountQuery(Querable quer)
    {
        quer.gameAreaHeight = databaseTable.gameAreaHeight;
        quer.gameAreaWidth = databaseTable.gameAreaWidth;
        quer.numUnitsToSpawn = sec;
        quer.unitSpawnDelay = databaseTable.unitSpawnDelay;
        quer.unitSpawnMinRadius = databaseTable.unitSpawnMinRadius;
        quer.unitSpawnMaxRadius = databaseTable.unitSpawnMaxRadius;
        quer.unitSpawnMinSpeed = databaseTable.unitSpawnMinSpeed;
        quer.unitSpawnMaxSpeed = databaseTable.unitSpawnMaxSpeed;
        quer.unitDestroyRadius = databaseTable.unitDestroyRadius;
    }
    IEnumerator SpawnSphereCoroutine()
    {
        while (sec != databaseTable.numUnitsToSpawn)
        {
            yield return new WaitForSeconds(databaseTable.unitSpawnDelay);
            CreateGem(Random.Range(0, 100), Random.Range(0, 100), spherePrefab[Random.Range(0, spherePrefab.Length)]);
            sec++;
        }
    }
    private void OnGUI()
    {
        if (restart == true)
        {
            if (GUI.Button(new Rect(5, 25, 80, 20), "playAgain"))
            {
                StartCoroutine(SpawnSphereCoroutine());
                restart = false;
            }
        }
    }
}
