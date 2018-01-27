using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** represent a task that the blinded man should do */
public abstract class Task {

    /** return true if the task is ended */
    public abstract bool Update(Blinded blinded);

}
