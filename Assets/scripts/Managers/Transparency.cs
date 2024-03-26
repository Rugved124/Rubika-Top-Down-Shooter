using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour
{
	private static int LAYER_OBSTACLES = 6;

	public Transform pcTransform;
	public LayerMask mask;


	Renderer rend;

	public Material transparentMaterial;

	private void Update()
	{
		//float dist = Vector3.Distance(transform.position, pcTransform.position);

		//RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.position + pcTransform.position, dist, mask);


		//foreach (RaycastHit hit in hits)
		//{
		//	GameObject goHit = hit.collider.gameObject;
		//	if (goHit.layer == LAYER_OBSTACLES)
		//	{
		//		MakeTransparent(goHit);
		//	}
		//}


		Vector3 dir = pcTransform.position - transform.position;

		dir = dir.normalized;
		Debug.DrawRay(transform.position, dir * Vector3.Distance(transform.position, pcTransform.position), Color.red);

		Ray ray = new Ray(transform.position, dir * Vector3.Distance(transform.position, pcTransform.position));

		RaycastHit[] hits = Physics.RaycastAll(ray, mask);

		Array.Sort(hits, (x, y) =>
		x.distance.CompareTo(y.distance));

		foreach (RaycastHit hit in hits)
		{
			GameObject goHit = hit.collider.gameObject;
			MakeTransparent(goHit);
		}

	}

	private void MakeTransparent(GameObject goHit)
	{
		// Change the material of all hit colliders
		// to use a transparent shader.
		if (goHit.GetComponent<Renderer>() != null)
		{
			rend = goHit.GetComponentInChildren<Renderer>();
		}

		else
		{
			rend = goHit.GetComponent<Renderer>();
		}
		rend.material = transparentMaterial;
		Color tempColor = rend.material.color;
		tempColor.a = 0.3F;
		rend.material.color = tempColor;
		Debug.Log(goHit.name + " is Transparent");
	}


}
