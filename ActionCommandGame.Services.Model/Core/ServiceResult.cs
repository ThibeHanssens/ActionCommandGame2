
namespace ActionCommandGame.Services.Model.Core
{
    public class ServiceResult
    {
        public IList<ServiceMessage> Messages { get; set; } = new List<ServiceMessage>();

        public bool IsSuccess
        {
            get
            {
                //No error messages means success!
                return Messages.All(m => m.MessagePriority != MessagePriority.Error);
            }
        }
    }

    public class ServiceResult<T>: ServiceResult
    {
        public ServiceResult()
        {
            
        }
        public ServiceResult(T? data)
        {
            Data = data;
        }

        public T? Data { get; set; }
    }
}
