using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Interfaces
{
    public interface INotification
    {
        string Name{get;}
        object Body{get;set;}
        string Type{get;set;}
    }
}

