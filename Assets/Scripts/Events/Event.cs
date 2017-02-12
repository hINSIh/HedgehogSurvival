using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event
{
}

public interface Cancellable
{
	void SetCancelled(bool cancel);

	bool IsCancelled();
}