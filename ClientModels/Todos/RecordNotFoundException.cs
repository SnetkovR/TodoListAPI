using System;

namespace ClientModels.Todos
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string message)
            : base(message)
        {
        }

        public RecordNotFoundException(Guid recordId)
            : base($"Note \"{recordId}\" is not found.")
        {
        }
    }
}