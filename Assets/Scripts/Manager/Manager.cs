using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Manager : MonoBehaviour {
	private static Manager instance;

	public MonoBehaviour[] managerScripts;

	private Dictionary<Type, object> managerMap = new Dictionary<Type, object>();

	public static T Get<T>() {
		if (instance.managerMap.ContainsKey(typeof(T))) {
			return (T) instance.managerMap[typeof(T)];
		}

		throw new KeyNotFoundException();
	}

	void Awake() {
		if (instance) {
			throw new UnityException(
				string.Format("Already instance: {0}.cs", GetType().Name)
			);
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
		InitManagerScripts();
	}

	private void InitManagerScripts() {
		foreach (object manager in managerScripts) {
			managerMap.Add(manager.GetType(), manager);
		}
	}
}
