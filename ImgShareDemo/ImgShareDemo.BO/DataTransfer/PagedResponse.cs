namespace ImgShareDemo.BO.DataTransfer
{
    using System.Collections.Generic;

    public class PagedResponse<T>
    {
        public bool LastPage { get; set; } = false;
        public IList<T> Items { get; set; }
    }
}
