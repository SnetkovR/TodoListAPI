using System.Net;

namespace ClientModels.Errors
{
    public class ServiceErrorResponse
    {
        /// <summary>
        /// Статус код ответа
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Ошибка
        /// </summary>
        public ServiceError Error { get; set; }
    }
}