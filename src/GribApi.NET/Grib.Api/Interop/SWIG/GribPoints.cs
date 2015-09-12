/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.2
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace Grib.Api.Interop.SWIG {

public class GribPoints : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal GribPoints(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(GribPoints obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~GribPoints() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          GribApiProxyPINVOKE.delete_GribPoints(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public SWIGTYPE_p_grib_context context {
    set {
      GribApiProxyPINVOKE.GribPoints_context_set(swigCPtr, SWIGTYPE_p_grib_context.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = GribApiProxyPINVOKE.GribPoints_context_get(swigCPtr);
      SWIGTYPE_p_grib_context ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_grib_context(cPtr, false);
      return ret;
    } 
  }

  public SWIGTYPE_p_double latitudes {
    set {
      GribApiProxyPINVOKE.GribPoints_latitudes_set(swigCPtr, SWIGTYPE_p_double.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = GribApiProxyPINVOKE.GribPoints_latitudes_get(swigCPtr);
      SWIGTYPE_p_double ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_double(cPtr, false);
      return ret;
    } 
  }

  public SWIGTYPE_p_double longitudes {
    set {
      GribApiProxyPINVOKE.GribPoints_longitudes_set(swigCPtr, SWIGTYPE_p_double.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = GribApiProxyPINVOKE.GribPoints_longitudes_get(swigCPtr);
      SWIGTYPE_p_double ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_double(cPtr, false);
      return ret;
    } 
  }

  public SWIGTYPE_p_size_t indexes {
    set {
      GribApiProxyPINVOKE.GribPoints_indexes_set(swigCPtr, SWIGTYPE_p_size_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = GribApiProxyPINVOKE.GribPoints_indexes_get(swigCPtr);
      SWIGTYPE_p_size_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_size_t(cPtr, false);
      return ret;
    } 
  }

  public SWIGTYPE_p_size_t groupStart {
    set {
      GribApiProxyPINVOKE.GribPoints_groupStart_set(swigCPtr, SWIGTYPE_p_size_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = GribApiProxyPINVOKE.GribPoints_groupStart_get(swigCPtr);
      SWIGTYPE_p_size_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_size_t(cPtr, false);
      return ret;
    } 
  }

  public SWIGTYPE_p_size_t groupLen {
    set {
      GribApiProxyPINVOKE.GribPoints_groupLen_set(swigCPtr, SWIGTYPE_p_size_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = GribApiProxyPINVOKE.GribPoints_groupLen_get(swigCPtr);
      SWIGTYPE_p_size_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_size_t(cPtr, false);
      return ret;
    } 
  }

  public uint nGroups {
    set {
      GribApiProxyPINVOKE.GribPoints_nGroups_set(swigCPtr, value);
    } 
    get {
      uint ret = GribApiProxyPINVOKE.GribPoints_nGroups_get(swigCPtr);
      return ret;
    } 
  }

  public uint n {
    set {
      GribApiProxyPINVOKE.GribPoints_n_set(swigCPtr, value);
    } 
    get {
      uint ret = GribApiProxyPINVOKE.GribPoints_n_get(swigCPtr);
      return ret;
    } 
  }

  public uint size {
    set {
      GribApiProxyPINVOKE.GribPoints_size_set(swigCPtr, value);
    } 
    get {
      uint ret = GribApiProxyPINVOKE.GribPoints_size_get(swigCPtr);
      return ret;
    } 
  }

  public GribPoints() : this(GribApiProxyPINVOKE.new_GribPoints(), true) {
  }

}

}
