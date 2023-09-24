using System.Collections.Generic;

public class AbilityKnown
{
    public List<string> list;
    
    public AbilityKnown(List<string> _list = null)
    {
        if (_list != null) list = _list;
        else list = new List<string>();
    }
    
    public void Add(string known)
    {
        list.Add(known);
    }
    
    public void Remove(string known)
    {
        if(list.Contains(known)) list.Remove(known);
    }
}