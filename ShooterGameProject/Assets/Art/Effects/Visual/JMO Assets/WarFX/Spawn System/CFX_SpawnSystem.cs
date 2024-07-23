using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Cartoon FX  - (c) 2012-2016 Jean Moreno

// Spawn System:
// Preload GameObject to reuse them later, avoiding to Instantiate them.
// Very useful for mobile platforms.

public class CfxSpawnSystem : MonoBehaviour
{
	/// <summary>
	/// Get the next available preloaded Object.
	/// </summary>
	/// <returns>
	/// The next available preloaded Object.
	/// </returns>
	/// <param name='sourceObj'>
	/// The source Object from which to get a preloaded copy.
	/// </param>
	/// <param name='activateObject'>
	/// Activates the object before returning it.
	/// </param>
	static public GameObject GetNextObject(GameObject sourceObj, bool activateObject = true)
	{
		int uniqueId = sourceObj.GetInstanceID();
		
		if(!_instance._poolCursors.ContainsKey(uniqueId))
		{
			Debug.LogError("[CFX_SpawnSystem.GetNextObject()] Object hasn't been preloaded: " + sourceObj.name + " (ID:" + uniqueId + ")\n", _instance);
			return null;
		}
		
		int cursor = _instance._poolCursors[uniqueId];
		GameObject returnObj = null;
		if(_instance.onlyGetInactiveObjects)
		{
			int loop = cursor;
			while(true)
			{
				returnObj = _instance._instantiatedObjects[uniqueId][cursor];
				_instance.IncreasePoolCursor(uniqueId);
				cursor = _instance._poolCursors[uniqueId];

				if(returnObj != null && !returnObj.activeSelf)
					break;

				//complete loop: no active instance available
				if(cursor == loop)
				{
					if(_instance.instantiateIfNeeded)
					{
						Debug.Log("[CFX_SpawnSystem.GetNextObject()] A new instance has been created for \"" + sourceObj.name + "\" because no active instance were found in the pool.\n", _instance);
						PreloadObject(sourceObj);
						var list = _instance._instantiatedObjects[uniqueId];
						returnObj = list[list.Count-1];
						break;
					}
					else
					{
						Debug.LogWarning("[CFX_SpawnSystem.GetNextObject()] There are no active instances available in the pool for \"" + sourceObj.name +"\"\nYou may need to increase the preloaded object count for this prefab?", _instance);
						return null;
					}
				}
			}
		}
		else
		{
			returnObj = _instance._instantiatedObjects[uniqueId][cursor];
			_instance.IncreasePoolCursor(uniqueId);
		}

		if(activateObject && returnObj != null)
			returnObj.SetActive(true);

		return returnObj;
	}
	
	/// <summary>
	/// Preloads an object a number of times in the pool.
	/// </summary>
	/// <param name='sourceObj'>
	/// The source Object.
	/// </param>
	/// <param name='poolSize'>
	/// The number of times it will be instantiated in the pool (i.e. the max number of same object that would appear simultaneously in your Scene).
	/// </param>
	static public void PreloadObject(GameObject sourceObj, int poolSize = 1)
	{
		_instance.AddObjectToPool(sourceObj, poolSize);
	}
	
	/// <summary>
	/// Unloads all the preloaded objects from a source Object.
	/// </summary>
	/// <param name='sourceObj'>
	/// Source object.
	/// </param>
	static public void UnloadObjects(GameObject sourceObj)
	{
		_instance.RemoveObjectsFromPool(sourceObj);
	}
	
	/// <summary>
	/// Gets a value indicating whether all objects defined in the Editor are loaded or not.
	/// </summary>
	/// <value>
	/// <c>true</c> if all objects are loaded; otherwise, <c>false</c>.
	/// </value>
	static public bool AllObjectsLoaded
	{
		get
		{
			return _instance._allObjectsLoaded;
		}
	}
	
	// INTERNAL SYSTEM ----------------------------------------------------------------------------------------------------------------------------------------
	
	static private CfxSpawnSystem _instance;
	
	public GameObject[] objectsToPreload = new GameObject[0];
	public int[] objectsToPreloadTimes = new int[0];
	public bool hideObjectsInHierarchy = false;
	public bool spawnAsChildren = true;
	public bool onlyGetInactiveObjects = false;
	public bool instantiateIfNeeded = false;
	
	private bool _allObjectsLoaded;
	private Dictionary<int,List<GameObject>> _instantiatedObjects = new Dictionary<int, List<GameObject>>();
	private Dictionary<int,int> _poolCursors = new Dictionary<int, int>();
	
	private void AddObjectToPool(GameObject sourceObject, int number)
	{
		int uniqueId = sourceObject.GetInstanceID();

		//Add new entry if it doesn't exist
		if(!_instantiatedObjects.ContainsKey(uniqueId))
		{
			_instantiatedObjects.Add(uniqueId, new List<GameObject>());
			_poolCursors.Add(uniqueId, 0);
		}
		
		//Add the new objects
		GameObject newObj;
		for(int i = 0; i < number; i++)
		{
			newObj = (GameObject)Instantiate(sourceObject);
				newObj.SetActive(false);

			//Set flag to not destruct object
			CfxAutoDestructShuriken[] autoDestruct = newObj.GetComponentsInChildren<CfxAutoDestructShuriken>(true);
			foreach(CfxAutoDestructShuriken ad in autoDestruct)
			{
				ad.onlyDeactivate = true;
			}
			//Set flag to not destruct light
			CfxLightIntensityFade[] lightIntensity = newObj.GetComponentsInChildren<CfxLightIntensityFade>(true);
			foreach(CfxLightIntensityFade li in lightIntensity)
			{
				li.autodestruct = false;
			}
			
			_instantiatedObjects[uniqueId].Add(newObj);
			
			if(hideObjectsInHierarchy)
				newObj.hideFlags = HideFlags.HideInHierarchy;

			if(spawnAsChildren)
				newObj.transform.parent = this.transform;
		}
	}
	
	private void RemoveObjectsFromPool(GameObject sourceObject)
	{
		int uniqueId = sourceObject.GetInstanceID();
		
		if(!_instantiatedObjects.ContainsKey(uniqueId))
		{
			Debug.LogWarning("[CFX_SpawnSystem.removeObjectsFromPool()] There aren't any preloaded object for: " + sourceObject.name + " (ID:" + uniqueId + ")\n", this.gameObject);
			return;
		}
		
		//Destroy all objects
		for(int i = _instantiatedObjects[uniqueId].Count - 1; i >= 0; i--)
		{
			GameObject obj = _instantiatedObjects[uniqueId][i];
			_instantiatedObjects[uniqueId].RemoveAt(i);
			GameObject.Destroy(obj);
		}
		
		//Remove pool entry
		_instantiatedObjects.Remove(uniqueId);
		_poolCursors.Remove(uniqueId);
	}

	private void IncreasePoolCursor(int uniqueId)
	{
		_instance._poolCursors[uniqueId]++;
		if(_instance._poolCursors[uniqueId] >= _instance._instantiatedObjects[uniqueId].Count)
		{
			_instance._poolCursors[uniqueId] = 0;
		}
	}

	//--------------------------------

	void Awake()
	{
		if(_instance != null)
			Debug.LogWarning("CFX_SpawnSystem: There should only be one instance of CFX_SpawnSystem per Scene!\n", this.gameObject);
		
		_instance = this;
	}
	
	void Start()
	{
		_allObjectsLoaded = false;
		
		for(int i = 0; i < objectsToPreload.Length; i++)
		{
			PreloadObject(objectsToPreload[i], objectsToPreloadTimes[i]);
		}
		
		_allObjectsLoaded = true;
	}
}
