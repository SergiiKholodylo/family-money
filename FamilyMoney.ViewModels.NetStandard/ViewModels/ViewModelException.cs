using System;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{

    public class ViewModelException : Exception
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