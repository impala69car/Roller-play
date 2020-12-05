using UnityEngine;
using System.Collections;

public class SphereGem : MonoBehaviour
{
    public int color;
    public int rotationCount = 1;
    public int RotationNum=1;
    float unitDestroyRadiusLoads;
    public Vector3 loadedVector;
    public float sradius;
    float speed=1f;
    private Vector3 dir;
    private float angleTarget;
    public float walkLeftRight = 220;
    [SerializeField] GameObject GameColor;
    public float row, col;
    public string type;
    public Gem GetGem { get; private set; }
    public bool isBonus;
    public int BonusMatchType;
    public bool isSwirl = false;
    public int seconds = 0;
    void CalculateAngle(Vector3 temp)
    {
        dir = new Vector3(temp.x, transform.position.y, temp.z) - transform.position;
        angleTarget = Vector3.Angle(dir, transform.forward);
    }
    void LookAtThis(Vector3 targ)
    {
        CalculateAngle(transform.position + targ);
        if (angleTarget > 1)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), walkLeftRight * UnityEngine.Time.deltaTime);
            //print("sad");
        }
    }
    void loadColor()
    {
        GameColor.GetComponent<Shader>().GetPropertyAttributes(0);
    }
    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        unitDestroyRadiusLoads = FindObjectOfType<BasicManager>().numUnitsToSpawnLoad();
        StartCoroutine(GetROtationCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 100 || transform.position.x < 0 ||
                   transform.position.z > 100 || transform.position.z < 0
                   || transform.position.y < -10f
                   )
        {
            transform.position = new Vector3(25f, 2.5f, 25);
        }
        if (RotationNum == 1)
        {
            //transform.Translate(new Vector3(1f, 0, 0));
            //LookAtThis(new Vector3(100, 0, 0));
            /*Vector3 targetDirection = (transform.position + new Vector3(1, 0, 0)) - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(new Vector3(1, 0, 0), targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newdir, Color.red);
            transform.rotation = Quaternion.LookRotation(newdir);*/
        }
        if (RotationNum == 2)
        {
            //transform.Translate(new Vector3(-1f, 0, 0));
            //LookAtThis(new Vector3(-100, 0, 0));
            /*Vector3 targetDirection = (transform.position + new Vector3(-1, 0, 0)) - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(new Vector3(-1, 0, 0), targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newdir, Color.red);
            transform.rotation = Quaternion.LookRotation(newdir);*/
        }
        if (RotationNum == 3)
        {
            //transform.Translate(new Vector3(0, 0, 1f));
            //LookAtThis(new Vector3(0, 0, 100));
            /*Vector3 targetDirection = (transform.position + new Vector3(0, 0, 1)) - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(new Vector3(0,0,1), targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newdir, Color.red);
            transform.rotation = Quaternion.LookRotation(newdir);*/
        }
        if (RotationNum == 4)
        {
            //transform.Translate(new Vector3(0, 0, -1f));
            //LookAtThis(new Vector3(0, 0, -100));
            /*Vector3 targetDirection = (transform.position + new Vector3(0, 0, -1)) - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(new Vector3(0, 0, -1), targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newdir, Color.red);
            transform.rotation = Quaternion.LookRotation(newdir);*/
        }
        if (
            transform.localScale.x <=0.2f||
            transform.localScale.y <=0.2f||
            transform.localScale.z <=0.2f            
            )
        {
            FindObjectOfType<BasicManager>().RemoveSphere(this);
        }
       
        //RotAll();
    }
    IEnumerator GetROtationCoroutine()
    {
        while (rotationCount != 0)
        {
            if (RotationNum == 1)
            {
                transform.TweenPosition(0.5f, new Vector3(100f, transform.position.y, transform.position.z));
            }
            if (RotationNum == 2)
            {
                transform.TweenPosition(0.5f,new Vector3(0f, transform.position.y, transform.position.z));
            }
            if (RotationNum == 3)
            {
                transform.TweenPosition(0.5f, new Vector3(transform.position.x, transform.position.y, 100f));
            }
            if (RotationNum == 4)
            {
                transform.TweenPosition(0.5f, new Vector3(transform.position.x, transform.position.y, 0f));
            }            
            yield return null;
            rotationCount++;
        }
    }

    public void RotAll()
    {

        if (RotationNum == 1)
        {
            Vector3 targetDirection = (transform.position + new Vector3(100, 0, 0))- transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(transform.forward,targetDirection,singleStep,0.0f);
            transform.rotation = Quaternion.LookRotation(newdir);

        }
        if (RotationNum == 2)
        {
            Vector3 targetDirection = (transform.position + new Vector3(-100, 0, 0)) - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newdir);

        }
        if (RotationNum == 3)
        {
            Vector3 targetDirection = (transform.position + new Vector3(0, 0, 100)) - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newdir);

        }
        if (RotationNum == 4)
        {
            Vector3 targetDirection = (transform.position + new Vector3(0, 0, -100)) - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newdir, Color.red);
            transform.rotation = Quaternion.LookRotation(newdir);
        }
    }
    private void Rotloader()
    {
        int randload = Random.Range(1, 5);
        if (randload != RotationNum)
        {
            RotationNum = randload;
        }
        else
        {
            Rotloader();
        }
    }
    public void isGem(Gem g)
    {
        GetGem = g;
    }
    public bool isequal(SphereGem hitCandy)
    {
        return hitCandy != null && hitCandy.type == type && hitCandy.type != "ingredient" + 0 && hitCandy.type != "ingredient" + 1;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "wall")
        {
            StopCoroutine(GetROtationCoroutine());
            StartCoroutine(GetROtationCoroutine());
            if (RotationNum == 4)
            {
                RotationNum = 3;
            }
            else if (RotationNum == 3) RotationNum = 4;
            else if (RotationNum == 2) RotationNum = 1;
            else RotationNum = 2;
        }
        if(col.gameObject.tag == "sphere")
        {
            if (
                name!=col.gameObject.name
                )
            {
                transform.localScale -= new Vector3(0.2f,0.2f,0.2f);
            }
            else
            {
                if (RotationNum == 4)
                {
                    RotationNum = 3;
                }
                else if (RotationNum == 3) RotationNum = 4;
                else if (RotationNum == 2) RotationNum = 1;
                else RotationNum = 2;
                //Rotloader();
                
            }
        }
    }
}
