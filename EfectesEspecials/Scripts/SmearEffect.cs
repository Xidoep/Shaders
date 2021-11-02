using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XS_Utils;

public class SmearEffect : MonoBehaviour
{
	[SerializeField] int _frameLag = 0;


	Vector3[] _recentVPositionsNoQueue;
	[SerializeField] Mesh bakedMesh = null;
	Vector3[] _tmp;
	Vector3[] _getFromWorldVertices;
	Vector3[] _dequeued;
	Queue<Vector3[]> _recentVPositions;

	MeshFilter meshFilter = null;
	MeshFilter GetMeshFilter
    {
        get
        {
			if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
			return meshFilter;
        }
    }

	[SerializeField] SkinnedMeshRenderer GetSkinnedMeshRenderer = null;
	/*SkinnedMeshRenderer GetSkinnedMeshRenderer
    {
        get
        {
			if (!skinnedMeshRenderer) skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			return skinnedMeshRenderer;
        }
    }*/

	Color[] colors = new Color[0];
	Color[] Colors
    {
        get
        {
			if (colors.Length == 0) colors = new Color[GetMeshFilter.mesh.vertices.Length];
			return colors;
        }
    }

	Color[] ColorsSkinned(Mesh mesh)
    {
		if (colors.Length == 0) colors = new Color[mesh.vertices.Length];
		return colors;
	}

	//Vector3[] vertex;

	[SerializeField] Matrix4x4 m;

	[SerializeField] bool skinned;
	[SerializeField] float valor;
	[SerializeField] AnimatorControllerParameter animatorControllerParameter;

    private void OnEnable()
    {
		//vertex = GetMeshFilter.mesh.vertices;

	}

    void LateUpdate()
	{
		//valor = GetComponent<Animator>().GetFloat("Curve");

		//BY POSITION
		/*Vector3 prova = Vector3.zero;
		if(_recentPositions.Count > _frameLag)
        {
			prova = _recentPositions.Dequeue();
			//myMaterial.SetVector("_PrevPosition", prova);
		}

		//myMaterial.SetVector("_Position", transform.position);
		_recentPositions.Enqueue(transform.position);
		myMaterial.SetVector("_Substraccio", prova - transform.position);
		*/

		//BY VERTEX

		if(_frameLag > 0)
        {
			if (!skinned)
				DragMultipleFrames();
			else DragMultipleFramesSkinned();

		}
        else
        {
			if (!skinned)
				DragOneFrame();
			else DragOneFramSkinned();
        }

	}
    /*private void FixedUpdate()
    {
		if (_frameLag > 0)
		{
			DragMultipleFrames();
		}
		else
		{
			if (!skinned)
				DragOneFrame();
			else DragOneFramSkinned();
		}
	}*/

    void DragMultipleFrames()
    {
		if(_dequeued == null)_dequeued = new Vector3[GetMeshFilter.mesh.vertices.Length];
		if (_recentVPositions == null) _recentVPositions = new Queue<Vector3[]>();

		if (_recentVPositions.Count > _frameLag) _dequeued = _recentVPositions.Dequeue();

		_getFromWorldVertices = WorldVertices();

		_recentVPositions.Enqueue(_getFromWorldVertices);

		for (int i = 0; i < _dequeued.Length; i++) { Colors[i] = (_dequeued[i] - _getFromWorldVertices[i]).ToColor(); }
		GetMeshFilter.mesh.colors = Colors;
	}

	void DragMultipleFramesSkinned()
    {
		if (_dequeued == null) 
		{
			if (bakedMesh == null) bakedMesh = new Mesh();
			GetSkinnedMeshRenderer.BakeMesh(bakedMesh);
			_dequeued = new Vector3[bakedMesh.vertices.Length];
		}
		if (_recentVPositions == null) 
		{
			if (bakedMesh == null) bakedMesh = new Mesh();
			GetSkinnedMeshRenderer.BakeMesh(bakedMesh);
			_recentVPositions = new Queue<Vector3[]>();
		} 

		if (_recentVPositions.Count > _frameLag) _dequeued = _recentVPositions.Dequeue();

		_getFromWorldVertices = WorldVerticesSkinned();

		_recentVPositions.Enqueue(_getFromWorldVertices);

		for (int i = 0; i < _dequeued.Length; i++) { ColorsSkinned(bakedMesh)[i] = (_dequeued[i] - _getFromWorldVertices[i]).ToColor(); }
		GetSkinnedMeshRenderer.sharedMesh.colors = ColorsSkinned(bakedMesh);
		//GetMeshFilter.mesh.colors = Colors;
	}

	void DragOneFrame()
    {
		if(_recentVPositionsNoQueue == null) _recentVPositionsNoQueue = new Vector3[GetMeshFilter.mesh.vertices.Length];

		_dequeued = _recentVPositionsNoQueue;

		_getFromWorldVertices = WorldVertices();

		_recentVPositionsNoQueue = _getFromWorldVertices;

		for (int i = 0; i < _dequeued.Length; i++) { Colors[i] = (_dequeued[i] - _getFromWorldVertices[i]).ToColor(); }
		GetMeshFilter.mesh.colors = Colors;

		m = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
		GetComponent<MeshRenderer>().material.SetMatrix("_Matrix4x4", m);
		//Prova
		//GetMeshFilter.mesh.normals
	}

	void DragOneFramSkinned()
    {
		if (_recentVPositionsNoQueue == null) 
		{
			if (bakedMesh == null) bakedMesh = new Mesh();
			GetSkinnedMeshRenderer.BakeMesh(bakedMesh);
			_recentVPositionsNoQueue = new Vector3[bakedMesh.vertices.Length];
		} 
		
		_dequeued = _recentVPositionsNoQueue;

		_getFromWorldVertices = WorldVerticesSkinned();

		_recentVPositionsNoQueue = _getFromWorldVertices;

        for (int i = 0; i < _dequeued.Length; i++) { ColorsSkinned(bakedMesh)[i] = (_dequeued[i] - _getFromWorldVertices[i]).ToColor(); }

		//GetSkinnedMeshRenderer.BakeMesh(bakedMesh);
		GetSkinnedMeshRenderer.sharedMesh.colors = ColorsSkinned(bakedMesh);
		//bakedMesh.colors = ColorsSkinned(bakedMesh);
	}

	Vector3[] WorldVertices()
    {
		/*_tmp = GetMeshFilter.mesh.vertices;
		for (int i = 0; i < GetMeshFilter.mesh.vertices.Length; i++)
		{
			_tmp[i] = transform.localToWorldMatrix.MultiplyPoint3x4(GetMeshFilter.mesh.vertices[i]);
		}
		return _tmp;*/

		/*_tmp = mesh.vertices;
		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			_tmp[i] = transform.localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[i]);
		}
		return _tmp;*/

		_tmp = GetMeshFilter.mesh.vertices;
		//_tmp = vertex;
		for (int i = 0; i < _tmp.Length; i++)
		{
			_tmp[i] = transform.localToWorldMatrix.MultiplyPoint3x4(_tmp[i]);
		}
		return _tmp;
	}

	Vector3[] WorldVerticesSkinned()
    {
		GetSkinnedMeshRenderer.BakeMesh(bakedMesh);
		_tmp = bakedMesh.vertices;
		//_tmp = vertex;
		for (int i = 0; i < _tmp.Length; i++)
		{
			//_tmp[i] = transform.localToWorldMatrix;
			_tmp[i] = transform.localToWorldMatrix.MultiplyPoint3x4(_tmp[i]);
		}
		return _tmp;
	}
}
