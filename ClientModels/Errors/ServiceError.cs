using System.Collections.Generic;

namespace ClientModels.Errors
{
    public class ServiceError
    {
        /// <summary>
        /// Идентификатор ошибки
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Системное сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Ресурс или тип, который инициировал ошибку
        /// </summary>        
        public string Target { get; set; }

        /// <summary>
        /// Дополнительные параметры
        /// </summary>        
        public IDictionary<string, object> Context { get; set; }

        /// <summary>
        /// Вложенные ошибки 
        /// </summary>        
        public ICollection<ServiceError> Errors { get; set; }
    }
}