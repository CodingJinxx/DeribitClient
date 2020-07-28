using Newtonsoft.Json;

namespace Deribit.Core.Messages
{
    public interface IResponse<T>
    {
        public static ResponseBase<T> FromJson(string json)
        {
            ResponseBase<T> response = JsonConvert.DeserializeObject<ResponseBase<T>>(json);
            return response;
        }
    }
}
