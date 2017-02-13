using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface AchievementFormat<T> {
	string Format(T value);
}

public class Achievement<T> : Option<T>, IComparable where T : IComparable {
	private AchievementFormat<T> formatter;

	public Achievement(string prefsNode, 
	                   ISavable<T> savable, 
	                   AchievementFormat<T> formatter) : base(prefsNode, savable) 
	{
		this.formatter = formatter;	
	}

	public override T Value { 
		set
		{
			if (0 <= CompareTo(value)) {
				return;
			}
			this.value = value;
			savable.Save(prefsNode, value);
		}
	}

	public override string ToString() {
		return formatter.Format(Value);
	}

	public int CompareTo(object obj) {
		return Value.CompareTo(obj);
	}
}