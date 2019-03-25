using System.Collections.Generic;

namespace ClientModels.Notes
{
    public class NoteList
    {
        /// <summary>
        /// Краткая информация о заметках
        /// </summary>
        public IReadOnlyCollection<NoteInfo> Notes { get; set; }
    }
}