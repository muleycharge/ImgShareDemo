namespace ImgShareDemo.BO
{
    using System;

    public interface ITrackable
    {
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}
