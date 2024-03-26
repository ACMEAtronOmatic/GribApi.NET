using System;

namespace Grib.Api.Interop;

public class GribApiFatalException : Exception
{
    public GribApiFatalException() : this("") { }

    public GribApiFatalException (string msg) : base(msg) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GribApiException"/> class.
    /// </summary>
    /// <param name="msg">The MSG.</param>
    /// <param name="innerException">The inner exception.</param>
    public GribApiFatalException(string msg, Exception innerException = null)
        : base(msg, innerException)
    {
    }
}