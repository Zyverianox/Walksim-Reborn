using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;
using WalkSim.Plugin;

namespace WalkSim.Tools;

public class DebugPoint : MonoBehaviour
{
    public static Dictionary<string, DebugPoint> points = new Dictionary<string, DebugPoint>();

    public float size = 0.1f;

    public Color color = Color.white;

    private Material material;

    private void Awake()
    {
        material = Object.Instantiate(WalkSim.Plugin.Plugin.Instance.bundle.LoadAsset<Material>("m_xRay"));
        material.color = color;
        GetComponent<MeshRenderer>().material = material;
    }

    private void FixedUpdate()
    {
        material.color = color;
        transform.localScale = Vector3.one * size * Player.Instance.scale;
    }

    private void OnDestroy()
    {
        points.Remove(name);
    }

    public static Transform Get(string name, Vector3 position, Color color = default, float size = 0.1f)
    {
        if (points.ContainsKey(name))
        {
            points[name].color = color;
            points[name].transform.position = position;
            points[name].size = size;
            return points[name].transform;
        }
        return Create(name, position, color);
    }

    private static Transform Create(string name, Vector3 position, Color color)
    {
        Transform transform = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        transform.name = "Cipher Debugger (" + name + ")";
        transform.localScale = Vector3.one * 0.2f;
        transform.position = position;
        transform.GetComponent<Collider>().enabled = false;
        Material material = Object.Instantiate(((Renderer)GorillaTagger.Instance.offlineVRRig.mainSkin).material);
        transform.GetComponent<Renderer>().material = material;
        DebugPoint debugPoint = transform.gameObject.AddComponent<DebugPoint>();
        debugPoint.color = color;
        points.Add(name, debugPoint);
        return transform;
    }
}
