using System;
using System.Runtime.Serialization;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{

    internal class ViewModelException : Exception
    {
        public ViewModelException()
        {
        }

        public ViewModelException(string message) : base(message)
        {
        }

        public ViewModelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}