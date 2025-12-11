namespace RealEstateListingApi.Application.Exceptions
{
    public class ServiceException : Exception
    {
        public string Component { get; }
        public ServiceExceptionSeverity Severity { get; }

        public ServiceException(string message, string component, ServiceExceptionSeverity severity = ServiceExceptionSeverity.Business)
            : base(message)
        {
            Component = component;
            Severity = severity;
        }

        public ServiceException(string message, Exception innerException, string component, ServiceExceptionSeverity severity = ServiceExceptionSeverity.Technical)
            : base(message, innerException)
        {
            Component = component;
            Severity = severity;
        }
    }
}
