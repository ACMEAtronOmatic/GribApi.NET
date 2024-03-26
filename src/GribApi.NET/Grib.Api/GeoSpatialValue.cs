// Copyright 2015 Eric Millin
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
namespace Grib.Api;

/// <summary>
/// A GRIB grid value with coordinates.
/// </summary>
public struct GeoSpatialValue : IGeoCoordinate, IEquatable<GeoSpatialValue>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Value { get; set; }
    public bool IsMissing { get; private set; }

    public GeoSpatialValue(double lat, double lon, double val, bool isMissing) : this()
    {
        Latitude = lat;
        Longitude = lon;
        Value = val;
        IsMissing = isMissing;
    }

    /// <summary>
    /// Equals the specified value.
    /// </summary>
    /// <param name="that">The that.</param>
    /// <returns></returns>
    public readonly bool Equals(GeoSpatialValue that)
    {
        return (Latitude == that.Latitude) &&
               (Longitude == that.Longitude) &&
               (Value == that.Value);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public readonly override bool Equals(object obj)
    {
        return (obj is GeoSpatialValue value) && Equals(value);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public readonly override int GetHashCode()
    {
        return Latitude.GetHashCode() ^ Longitude.GetHashCode() ^ Value.GetHashCode();
    }

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(GeoSpatialValue a, GeoSpatialValue b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(GeoSpatialValue a, GeoSpatialValue b)
    {
        return !(a.Equals(b));
    }
}