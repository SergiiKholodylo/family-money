using System;
using System.Runtime.Serialization;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    [Serializable]
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

        protected ViewModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}