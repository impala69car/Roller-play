using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadManager : MonoBehaviour
{
    BasicManager basicManager;
    private static Vector3 vector2position;
    private void Awake()
    {
        basicManager = GameObject.FindObjectOfType<BasicManager>(); vector2position = new Vector3(0, 2.5f, 0);
    }
    public void LoadBasic()
    {
        basicManager.ClearSpheres();basicManager.clearSphereVec();
        SphereGem[] spheres2 = new SphereGem[PlayerPrefs.GetInt("SavedCount")];
        FindObjectOfType<LevelManager>().query(spheres2);
        foreach (SphereGem gemsp in spheres2)
        {
            CreateGems2(gemsp.row, gemsp.col, basicManager.spheresPref()[gemsp.color], gemsp.RotationNum, gemsp.sradius);
        }
    }
    public void CreateGems2(float row, float c, SphereGem hgem, int rotnum, float rad)
    {
        Vector3 vectorgem = vector2position + (new Vector3(c, 0, row));
        basicManager.AddSphereVec(vectorgem);
        SphereGem sphere = ((GameObject)Instantiate(hgem.gameObject, vectorgem, Quaternion.identity)).GetComponent<SphereGem>();
        sphere.RotationNum = rotnum;
        sphere.transform.localScale = new Vector3(
                rad, rad, rad
                );
        sphere.RotAll();
        basicManager.AddSphere(sphere);
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(95, 2, 80, 20), "load"))
        {
            LoadBasic();
            basicManager.loadREdBlue();
        }
    }
}