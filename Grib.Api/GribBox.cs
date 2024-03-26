using Grib.Api.Interop;
using Grib.Api.Interop.SWIG;

namespace Grib.Api;

/// <summary>
/// A subdomain of field measurements.
/// </summary>
public class GribBox
{
    private readonly GribPoints points;

    /// <summary>
    /// Initializes a new instance of the <see cref="GribBox"/> class.
    /// </summary>
    /// <param name="msgHandle">The MSG handle.</param>
    /// <param name="nw">The nw.</param>
    /// <param name="se">The se.</param>
    public GribBox (GribHandle msgHandle, GeoCoordinate nw, GeoCoordinate se)
    {
        var box = GribApiProxy.GribBoxNew(msgHandle, out var err);

        if (err != 0)
        {
            throw GribApiException.Create(err);
        }

        var pts = GribApiProxy.GribBoxGetPoints(box, nw.Latitude, nw.Longitude, se.Latitude, se.Longitude, out err);

        if (err != 0)
        {
            throw GribApiException.Create(err);
        }

        points = new GribPoints(SWIGTYPE_p_grib_points.getCPtr(pts).Handle, false);
    }

    /// <summary>
    /// Gets or sets the latitudes.
    /// </summary>
    /// <value>
    /// The latitudes.
    /// </value>
    public double[] Latitudes
    {
        set => points.latitudes = value;
        get => points.latitudes;
    }

    /// <summary>
    /// Gets or sets the longitudes.
    /// </summary>
    /// <value>
    /// The longitudes.
    /// </value>
    public double[] Longitudes
    {
        set => points.longitudes = value;
        get => points.longitudes;
    }

    /// <summary>
    /// Gets or sets the indexes.
    /// </summary>
    /// <value>
    /// The indexes.
    /// </value>
    public uint Indexes
    {
        set => points.indexes = value;
        get => points.indexes;
    }

    /// <summary>
    /// Gets or sets the group start.
    /// </summary>
    /// <value>
    /// The group start.
    /// </value>
    public uint GroupStart
    {
        set => points.groupStart = value;
        get => points.groupStart;
    }

    /// <summary>
    /// Gets or sets the length of the group.
    /// </summary>
    /// <value>
    /// The length of the group.
    /// </value>
    public uint GroupLength
    {
        set => points.groupLen = value;
        get => points.groupLen;
    }

    /// <summary>
    /// Gets or sets the group count.
    /// </summary>
    /// <value>
    /// The group count.
    /// </value>
    public uint GroupCount
    {
        set => points.nGroups = value;
        get => points.nGroups;
    }

    /// <summary>
    /// Gets or sets the count.
    /// </summary>
    /// <value>
    /// The count.
    /// </value>
    public uint Count
    {
        set => points.n = value;
        get => points.n;
    }

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    /// <value>
    /// The size.
    /// </value>
    public uint Size
    {
        set => points.size = value;
        get => points.size;
    }
}