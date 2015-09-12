%module GribApi
%{
#include "grib_api.h"
%}

%include "swigtype_inout.i" 


%define %$isproperty        "match"="csvarin"  %enddef



%typemap(imtype) size_t * length "ref uint"
%typemap(cstype) size_t * length "ref uint"
%typemap(csin) size_t * length "ref length"

%typemap(imtype) size_t * size "ref uint"
%typemap(cstype) size_t * size "ref uint"
%typemap(csin) size_t * size "ref size"

%typemap(imtype) size_t * offset "out uint"
%typemap(cstype) size_t * offset "out uint"
%typemap(csin) size_t * offset "out offset"

//%typemap(ctype) int * error "int &"
%typemap(imtype) int * error "out int"
%typemap(cstype) int * error "out int"
%typemap(csin) int * error "out error"

%typemap(imtype) int * err "out int"
%typemap(cstype) int * err "out int"
%typemap(csin) int * err "out err"

%typemap(imtype) int * n "out int"
%typemap(cstype) int * n "out int"
%typemap(csin) int * n "out n"

%typemap(imtype) long * value "out int"
%typemap(cstype) long * value "out int"
%typemap(csin) long * value "out value"

%typemap(imtype) double * value "out double"
%typemap(cstype) double * value "out double"
%typemap(csin) double * value "out value"

%typemap(imtype) float * value "out float"
%typemap(cstype) float * value "out float"
%typemap(csin) float * value "out value"

%typemap(csin) long * vals "vals"
%typemap(cstype) long * vals "int[]"
%typemap(imtype) long * vals "int[]"
		 
%typemap(csin) long * values "values"
%typemap(cstype) long * values "int[]"
%typemap(imtype) long * values "int[]"

%typemap(csin) double * vals "vals"
%typemap(cstype) double * vals "double[]"
%typemap(imtype) double * vals "double[]"
		 
%typemap(csin) double * values "values"
%typemap(cstype) double * values "double[]"
%typemap(imtype) double * values "double[]"

%typemap(csin) unsigned char * bytes "bytes"
%typemap(cstype) unsigned char * bytes "byte[]"
%typemap(imtype) unsigned char * bytes "byte[]"

%typemap(csin) unsigned char const * bytes "bytes"
%typemap(cstype) unsigned char const * bytes "byte[]"
%typemap(imtype) unsigned char const * bytes "byte[]"

%typemap(imtype) double * lat "out double"
%typemap(cstype) double * lat "out double"
%typemap(csin) double * lat "out lat"

%typemap(imtype) double * lon "out double"
%typemap(cstype) double * lon "out double"
%typemap(csin) double * lon "out lon"

%typemap(csin) double * lats "lats"
%typemap(cstype) double * lats "double[]"
%typemap(imtype) double * lats "double[]"

%typemap(csin) double * lons "lons"
%typemap(cstype) double * lons "double[]"
%typemap(imtype) double * lons "double[]"

%typemap(csin) double * outlons "outlons"
%typemap(cstype) double * outlons "double[]"
%typemap(imtype) double * outlons "double[]"

%typemap(csin) double * outlats "outlats"
%typemap(cstype) double * outlats "double[]"
%typemap(imtype) double * outlats "double[]"

%typemap(csin) double * inlats "inlats"
%typemap(cstype) double * inlats "double[]"
%typemap(imtype) double * inlats "double[]"

%typemap(csin) double * inlons "inlons"
%typemap(cstype) double * inlons "double[]"
%typemap(imtype) double * inlons "double[]"

%typemap(csin) double * distances "distances"
%typemap(cstype) double * distances "double[]"
%typemap(imtype) double * distances "double[]"

%typemap(csin) int * indexes "indexes"
%typemap(cstype) int * indexes "int[]"
%typemap(imtype) int * indexes "int[]"

%typemap(csin) char * mesg "mesg"
%typemap(imtype) char * mesg "System.Text.StringBuilder"
%typemap(cstype) char * mesg "System.Text.StringBuilder"


%typemap(imtype,         inattributes="[global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPStr)]",
         outattributes="[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.LPStr)]") char * "string"

%rename("%(lowercamelcase)s", %$isvariable) "";
%rename("%(camelcase)s", %$isclass) "";
%rename("%(camelcase)s", %$isaccess) "";
%rename("%(camelcase)s", %$classname) "";
%rename("%(camelcase)s", %$ismemberget) "";
%rename("%(camelcase)s", %$ismemberset) "";
%rename("%(camelcase)s", %$isfunction) "";
%rename("%(camelcase)s", %$isenum) "";
%rename("%(camelcase)s", %$isenumitem) "";
%rename("%(camelcase)s", %$isproperty) "";
%include "grib_api.h"

%rename("%(strip:[grib])s") ""; 
%rename("%(strip:[Grib])s") ""; 