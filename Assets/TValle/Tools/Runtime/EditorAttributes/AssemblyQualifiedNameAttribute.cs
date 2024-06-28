using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AssemblyQualifiedNameAttribute : PropertyAttribute
{
    public AssemblyQualifiedNameAttribute(Type Implementing)
    {
        implementingClass = Implementing;
    }
    public AssemblyQualifiedNameAttribute(Type ImplementingClass, Type ImplementingInterface)
    {
        implementingClass = ImplementingClass;
        implementingInterface = ImplementingInterface;
    }
    public AssemblyQualifiedNameAttribute()
    {

    }
    public Type implementingClass;
    public Type implementingInterface;
    public bool IsImplementing(Type other)
    {
        if(other == null)
            return false;

        var a_Assigned = implementingClass != null;
        var b_Assigned = implementingInterface != null;

        if(!a_Assigned && !b_Assigned)
            return true;

        if(a_Assigned && b_Assigned)
            return implementingClass.IsAssignableFrom(other) && implementingInterface.IsAssignableFrom(other);

        if(a_Assigned)
            return implementingClass.IsAssignableFrom(other);

        if(b_Assigned)
            return implementingInterface.IsAssignableFrom(other);

        throw new ArgumentOutOfRangeException();
    }
    public string GetError(Type other)
    {
        if(other == null)
            return string.Empty;

        var a_Assigned = implementingClass != null;
        var b_Assigned = implementingInterface != null;

        if(!a_Assigned && !b_Assigned)
            return string.Empty;

        if(a_Assigned && b_Assigned)
            return "The selected script (" + other.Name + ") does not implement the "+ implementingClass.Name + " or "+ implementingInterface.Name + " types.";
        if(a_Assigned)
            return "The selected script (" + other.Name + ") does not implement the type " + implementingClass.Name;
        if(b_Assigned)
            return "The selected script (" + other.Name + ") does not implement the type " + implementingInterface.Name;

        throw new ArgumentOutOfRangeException();
    }
}

