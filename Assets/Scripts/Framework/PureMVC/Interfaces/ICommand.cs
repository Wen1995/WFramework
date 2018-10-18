using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PureMVC.Interfaces
{
    public interface ICommand
    {
        /// <summary>
        /// Excute command's logic with giving notification
        /// </summary>
        void Excute(INotification notification);
    }
}

