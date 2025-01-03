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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		material = Object.Instantiate<Material>(WalkSim.Plugin.Plugin.Instance.bundle.LoadAsset<Material>("m_xRay"));
		material.color = color;
		((Renderer)((Component)this).GetComponent<MeshRenderer>()).material = material;
	}

	private void FixedUpdate()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		material.color = color;
		((Component)this).transform.localScale = Vector3.one * size * Player.Instance.scale;
	}

	private void OnDestroy()
	{
		points.Remove(((Object)this).name);
	}

	public static Transform Get(string name, Vector3 position, Color color = default(Color), float size = 0.1f)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (points.ContainsKey(name))
		{
			points[name].color = color;
			((Component)points[name]).transform.position = position;
			points[name].size = size;
			return ((Component)points[name]).transform;
		}
		return Create(name, position, color);
	}

	private static Transform Create(string name, Vector3 position, Color color)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = GameObject.CreatePrimitive((PrimitiveType)0).transform;
		((Object)transform).name = "Cipher Debugger (" + name + ")";
		transform.localScale = Vector3.one * 0.2f;
		transform.position = position;
		((Component)transform).GetComponent<Collider>().enabled = false;
		Material val = Object.Instantiate<Material>(((Renderer)GorillaTagger.Instance.offlineVRRig.mainSkin).material);
		((Component)transform).GetComponent<Renderer>().material = val;
		DebugPoint debugPoint = ((Component)transform).gameObject.AddComponent<DebugPoint>();
		debugPoint.color = color;
		points.Add(name, debugPoint);
		return ((Component)transform).transform;
	}
}
