using System;
namespace MaicoLand.Models.StructureType
{
    public class PagingParameter
    {
        const int maxPageSize = 50;
        public int pageNumber { set; get; } = 1;
        private int _pageSize = 10;
        public int pageSize
        {
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value; 
            }
            get
            {
                return _pageSize; 
            }
        }
    }
}
