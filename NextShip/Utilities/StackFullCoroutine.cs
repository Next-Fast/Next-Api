using System.Collections;
using System.Collections.Generic;


namespace NextShip.Utilities;

// form Nebula
public class StackFullCoroutine
{
    private readonly List<IEnumerator> stack = new();

    public StackFullCoroutine(IEnumerator enumerator)
    {
        stack.Add(enumerator);
    }

    public bool MoveNext() {
        if (stack.Count == 0) return false;

        var current = stack[^1];
        if (!current.MoveNext())
            stack.RemoveAt(stack.Count - 1);
        else if (current.Current is IEnumerator child)
            stack.Add(child);

        return stack.Count > 0;
    }
}