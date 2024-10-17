namespace RiwiTalent.Domain.Entities
{
    public class Pagination<T> : List<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; }
        public bool PageBefore => Page > 1;
        public bool PageAfter => Page < PageSize;

        public Pagination(List<T> items, int count, int page, int cantRegisters)
        {
            Page = page; 
            PageSize = (int)Math.Ceiling(count / (double)cantRegisters);
            TotalPages = (int)Math.Ceiling(count / (double)cantRegisters);
            this.AddRange(items);
        }

        public static Pagination<T> CreatePagination(List<T> items, int count, int page, int cantRegisters)
        {
            return new Pagination<T>(items, count, page, cantRegisters);
        }
    }
}