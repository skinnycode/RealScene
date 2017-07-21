using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChar : MonoBehaviour {

    // Use this for initialization
    GameObject shadowobj = null;
    GameObject roleobj = null;
    GameObject avatarobj = null;
    GameObject weaponobj = null;
    GameObject shoulderobj = null;
    GameObject terrainObj = null;
    Animator roleanim = null;

	void Start () {
        string strParentName = this.transform.name;
        shadowobj = GameObject.Find(strParentName+"/HardShadowProjector");
        roleobj = GameObject.Find(strParentName + "/role");
        avatarobj = GameObject.Find(strParentName + "/Avatar");
        roleanim = roleobj.GetComponent<Animator>();
        
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
	}

    void Update()
    {
        Ray ray = new Ray(new Vector3(this.transform.position.x, 900f, this.transform.position.z), new Vector3(0f, -1f, 0f));
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 1000f, -100787713))
        {
            Vector3 vPos = this.transform.position;
            vPos.y = raycastHit.point.y;
            this.transform.position = vPos;
        }
    }
    }
