using System.Collections.Generic;

namespace phonebook_server.Classes
{
    public class SearchFilter
    {
        public IEnumerable<FilterProperty> FilterProperties { get; set; }
        public string OrderBy { get; set; }
        public int OrderByDirection { get; set; }
    }

    public class FilterProperty
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}