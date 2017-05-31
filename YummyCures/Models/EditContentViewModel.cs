using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YummyCures.Models
{
    public class EditContentViewModel
    {
        public EditContentViewModel()
        {
            Content = new Content();
            Content.Tags = new List<Tag>();
        }

        public Content Content { get; set; }
        public IEnumerable<SelectListItem> AllTags { get; set; }

        public IEnumerable<SelectListItem> ContentTypesSelectItems { get; set; }

        private List<int> _selectedTags;
        public List<int> SelectedTags
        {
            get

            {
                if (_selectedTags == null)
                {
                    _selectedTags = Content.Tags.Select(m => m.TagID).ToList();
                }
                return _selectedTags;
            }
            set { _selectedTags = value; }
        }

    }
}