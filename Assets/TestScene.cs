using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour {

    // Use this for initialization
    GameObject shadowobj = null;
    GameObject roleobj = null;
    GameObject avatarobj = null;
    GameObject weaponobj = null;
    GameObject shoulderobj = null;
    GameObject terrainObj = null;
    Animator roleanim = null;
    public float moveSpeed = 4.65f;
    public float RunAnimSpeed = 1f;
    public bool ViewAngle45 = false;
    Vector3 vTapPosition = Vector3.zero;
    GameObject testChar = null;

    public float fpsMeasuringDelta = 2.0f;

    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;

    UnityEngine.UI.Text fps = null;
    UnityEngine.UI.Text btnScreen = null;
    UnityEngine.UI.Text btnQuality = null;
    UnityEngine.UI.Dropdown animList = null;

    GameObject SceneObj = null;

    public GameObject effect = null;

    public void SwitchObjShow()
    {
        if (SceneObj)
        {
            SceneObj.SetActive(!SceneObj.activeSelf);
        }
    }

    public void SwitchRoleShow()
    {
        if (roleobj)
        {
            roleobj.SetActive(!roleobj.activeSelf);
        }
    }

    public void SwitchTerrainShow()
    {
        if (terrainObj)
        {
            terrainObj.SetActive(!terrainObj.activeSelf);
        }
    }

    int nType = 0;
    public void SwitchScreenSize()
    {
        switch(nType)
        {
            case 0:
                Screen.SetResolution(1600, 900, true);
                btnScreen.text = "1600X900";
                break;
            case 1:
                Screen.SetResolution(1280, 720, true);
                btnScreen.text = "1280X720";
                break;
            case 2:
                Screen.SetResolution(1920, 1080, true);
                btnScreen.text = "1920X1080";
                break;
        }
        nType += 1;
        if (nType > 2)
        {
            nType = 0;
        }
        Debug.Log("SwitchScreenSize");
    }

    int nQuality = 0;
    public void SwitchQuality()
    {
        nQuality++;
        if (nQuality >= QualitySettings.names.Length)
        {
            nQuality = 0;
        }
        QualitySettings.SetQualityLevel(nQuality, true);
        btnQuality.text = "画质:" + QualitySettings.names[nQuality];
    }

    public void SwitchView()
    {
        ViewAngle45 = !ViewAngle45;
        testChar.transform.localEulerAngles = new Vector3(ViewAngle45 ? 45f : 30f, testChar.transform.localEulerAngles.y, 0f);
    }

        public void SwitchShadow()
    {
        
        if (shadowobj != null)
        {
            shadowobj.SetActive(!shadowobj.activeSelf);
        }
    }

    GameObject EffectObj = null;
    public void PlayEffect()
    {
        roleanim.Play(animList.options[animList.value].text);

        if (EffectObj != null)
        {
            GameObject.Destroy(EffectObj);
        }

        EffectObj = GameObject.Instantiate(effect);
        EffectObj.transform.parent = roleobj.transform;
        EffectObj.transform.localPosition = EffectObj.transform.position;
    }

	void Start () {
        timePassed = 0f;
        string strParentName = this.transform.name;
        shadowobj = GameObject.Find(strParentName+"/HardShadowProjector");
        roleobj = GameObject.Find(strParentName + "/role");
        avatarobj = GameObject.Find(strParentName + "/Avatar");
        //weaponobj = GameObject.Find("weapon");
        //shoulderobj = GameObject.Find("shoulder");
        testChar = GameObject.Find("TestScene");
        roleanim = roleobj.GetComponent<Animator>();
        terrainObj = GameObject.Find("Terrain ");
        GameObject fpsObj = GameObject.Find("fps");
        if (fpsObj)
        {
            fps = fpsObj.GetComponent<UnityEngine.UI.Text>();
        }

        GameObject btnScreenObj = GameObject.Find("btnScreen");
        if (btnScreenObj)
        {
            btnScreen = btnScreenObj.GetComponentInChildren<UnityEngine.UI.Text>();
        }

        GameObject btnQualityObj = GameObject.Find("btnQuality");
        if (btnQualityObj)
        {
            btnQuality = btnQualityObj.GetComponentInChildren<UnityEngine.UI.Text>();
        }

        //GameObject animListObj = GameObject.Find("animList");
        //if (animListObj)
        //{
        //    animList = animListObj.GetComponentInChildren<UnityEngine.UI.Dropdown>();
        //}

        //nQuality = QualitySettings.GetQualityLevel();
        //btnQuality.text = "画质:" + QualitySettings.names[nQuality];

        SceneObj = GameObject.Find("OBJS");

        List<CombineInstance> list = new List<CombineInstance>();
        List<Material> list2 = new List<Material>();
        List<Transform> list3 = new List<Transform>();
        
        SkinnedMeshRenderer[] componentsInChildrens = avatarobj.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildrens)
        {
            if (!(null == skinnedMeshRenderer.sharedMesh))
            {
                for (int i = 0; i < skinnedMeshRenderer.sharedMesh.subMeshCount; i++)
                {
                    CombineInstance item = default(CombineInstance);
                    item.mesh = (skinnedMeshRenderer.sharedMesh);
                    item.subMeshIndex = (i);
                    list.Add(item);
                }
                list2.AddRange(skinnedMeshRenderer.materials);
                Transform[] bones = skinnedMeshRenderer.bones;
                for (int j = 0; j < bones.Length; j++)
                {
                    Transform transform = bones[j];
                    Transform[] array = roleobj.GetComponentsInChildren<Transform>();
                    for (int k = 0; k < array.Length; k++)
                    {
                        Transform transform2 = array[k];
                        if (!(transform2.name != transform.name))
                        {
                            list3.Add(transform2);
                            break;
                        }
                    }
                }
            }
        }
        SkinnedMeshRenderer component = roleobj.AddComponent<SkinnedMeshRenderer>();
        component.updateWhenOffscreen = (true);
        component.sharedMesh = (new Mesh());
        component.sharedMesh.CombineMeshes(list.ToArray(), false, false);
        component.bones = (list3.ToArray());
        component.materials = (list2.ToArray());

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildrens)
        {
            GameObject.Destroy(skinnedMeshRenderer.transform.gameObject);
        }

        roleanim.Play("idle", 0, 0);
        //animList.ClearOptions();
//         List<string> lOption = new List<string>();
//         foreach (var anim in roleanim.runtimeAnimatorController.animationClips)
//         {
//             lOption.Add(anim.name);
//             
//         }
//         animList.AddOptions(lOption);   
	}

    public float AngleBetweenForward2Vector(Vector3 vectorAim)
    {
        vectorAim.y = 0;
        float angleBetweenForward2Vector = 0;
        if (vectorAim.x > 0)
        {
            angleBetweenForward2Vector = Vector3.Angle(testChar.transform.forward, vectorAim);
        }
        else
        {
            angleBetweenForward2Vector = 360 - Vector3.Angle(testChar.transform.forward, vectorAim);
        }
        return angleBetweenForward2Vector;
    }

    Vector2 vStartPosition = Vector2.zero;
	// Update is called once per frame
	void Update () {
        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;

            timePassed = 0.0f;
            m_FrameCount = 0;

            fps.text = m_FPS.ToString("F1");
        }

        bool bMove = false;
        Vector3 vDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            //             Vector3 vPos = this.transform.position;
            //             vPos.x += (moveSpeed * Time.deltaTime);
            //             this.transform.position = vPos;
            vDir.z += 1f;
            //roleobj.transform.LookAt(vPos);
            bMove = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //Vector3 vPos = this.transform.position;
            //vPos.x -= (moveSpeed * Time.deltaTime);
            //this.transform.position = vPos;
            vDir.z -= 1f;
            //roleobj.transform.LookAt(vPos);
            bMove = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //Vector3 vPos = this.transform.position;
            //vPos.z += (moveSpeed * Time.deltaTime);
            //this.transform.position = vPos;
            vDir.x -= 1f;
            //roleobj.transform.LookAt(vPos);
            bMove = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Vector3 vPos = this.transform.position;
            //vPos.z -= (moveSpeed * Time.deltaTime);
            //this.transform.position = vPos;
            vDir.x += 1f;
            //roleobj.transform.LookAt(vPos);
            bMove = true;
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                vStartPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 _currentSwipe = new Vector2(touch.position.x - vStartPosition.x, touch.position.y - vStartPosition.y);
                _currentSwipe.Normalize();

                vDir.x = _currentSwipe.x;
                vDir.z = _currentSwipe.y;

                if (Mathf.Abs(vDir.x) > 0.1f || Mathf.Abs(vDir.z) > 0.1f)
                {
                    bMove = true;
                }
            }
        }

        if (bMove)
        {
            Vector3 vInit = new Vector3(0f, 0f, 1f);
            float dir = (Vector3.Dot(Vector3.up, Vector3.Cross(testChar.transform.forward, vInit)) < 0 ? 1 : -1);
            float fA = Vector3.Angle(testChar.transform.forward, vInit);
            //Debug.Log("vDir:" + vDir.ToString() + "forward:" + testChar.transform.forward.ToString() + " Angle:" + fA + " Dot:" + dir);
            vDir = Quaternion.AngleAxis(dir * fA, new Vector3(0f, 1f, 0f)) * vDir;

            //Debug.Log("vDir1:" + vDir.ToString());
            //vDir = testChar.transform.TransformPoint(vDir.x, vDir.y, vDir.z);
            //roleobj.transform.LookAt(vDir);
            vDir.Normalize();
            //Debug.Log("vDir2:" + vDir.ToString());
            Vector3 vPos = this.transform.position;
            vPos += vDir * (moveSpeed * Time.deltaTime);
            this.transform.position = vPos;
            vPos += vDir * (moveSpeed * Time.deltaTime);
            roleobj.transform.LookAt(vPos);

            roleanim.speed = RunAnimSpeed;
        }
        else
        {
            roleanim.speed = 1f;
        }


        roleanim.SetBool("move", bMove);

        Ray ray = new Ray(new Vector3(this.transform.position.x, 900f, this.transform.position.z), new Vector3(0f, -1f, 0f));
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 1000f, -100787713))
        {
            Vector3 vPos = this.transform.position;
            vPos.y = raycastHit.point.y;
            this.transform.position = vPos;    
        }

        if (Input.GetMouseButtonDown(1))
        {
            vTapPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 vMouseMove = Input.mousePosition - vTapPosition;
            vTapPosition = Input.mousePosition;

            if (testChar != null)
            {
                testChar.transform.localEulerAngles = new Vector3(ViewAngle45?45f:30f, testChar.transform.localEulerAngles.y + vMouseMove.x, 0f);
                //Debug.Log("Forward:" + testChar.transform.forward.ToString());
            }

        }
	}
}
