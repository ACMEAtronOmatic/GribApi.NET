# (C) Copyright 1996-2014 ECMWF.
#
# This software is licensed under the terms of the Apache Licence Version 2.0
# which can be obtained at http://www.apache.org/licenses/LICENSE-2.0.
# In applying this licence, ECMWF does not waive the privileges and immunities
# granted to it by virtue of its status as an intergovernmental organisation nor
# does it submit to any jurisdiction.

##############################################################################
# macro for looking for openmp flags

macro( lookup_omp_flags )
  set(_OMP_FLAG_GNU        "-fopenmp")
  set(_OMPSTUBS_FLAG_GNU   "-fno-openmp")

  set(_OMP_FLAG_Cray       "-homp")
  set(_OMPSTUBS_FLAG_Cray  "-hnoomp")

  set(_OMP_FLAG_XL         "-qsmp=omp")
  set(_OMPSTUBS_FLAG_XL    "-qsmp=noomp")

  set(_OMP_FLAG_Intel      "-openmp")
  set(_OMPSTUBS_FLAG_Intel "-openmp-stubs")

  # sample C openmp source code to test
  set(_OMP_C_TEST_SOURCE
  "
  #include <omp.h>
  int main() {
  #ifdef _OPENMP
    #pragma omp parallel
    {
      int id = omp_get_thread_num();
    }
    return 0;
  #else
    breaks_on_purpose
  #endif
  }
  ")
  set( _OMP_CXX_TEST_SOURCE ${_OMP_C_TEST_SOURCE} )


  # sample C openmp source code to test
  set(_OMPSTUBS_C_TEST_SOURCE
  "
  // Include must be found
  #include <omp.h>
  int main() {
  #ifdef _OPENMP
    breaks_on_purpose
  #else
    #pragma omp parallel
    {
      // This pragma should have passed compilation
      int id = 0;
    }
    return 0;
  #endif
  }
  ")
  set( _OMPSTUBS_CXX_TEST_SOURCE ${_OMPSTUBS_C_TEST_SOURCE} )


  # sample Fortran openmp source code to test
  set(_OMP_Fortran_TEST_SOURCE
  "
  program main
    use omp_lib
  end program
  ")
  set( _OMPSTUBS_Fortran_TEST_SOURCE ${_OMP_Fortran_TEST_SOURCE} )

endmacro()


# MACRO ecbuild_find_omp
#
# ecbuild_find_omp( COMPONENTS C CXX Fortran
#                   STUBS )
# Sets following variables
#   - OMP_FOUND
#   - OMP_<lang>_FOUND
#   - OMP_<lang>_FLAGS
#
# If STUBS are available, above flags will still hold TRUE,
# as OMP code will just work.
#
macro( ecbuild_find_omp )

  set( options REQUIRED STUBS )
  set( single_value_args )
  set( multi_value_args COMPONENTS )

  cmake_parse_arguments( _PAR "${options}" "${single_value_args}" "${multi_value_args}"  ${_FIRST_ARG} ${ARGN} )

  if( NOT _PAR_COMPONENTS )
    message( FATAL_ERROR "No COMPONENTS were specified, looking for OMP.\n Please find with COMPONENTS C CXX Fortran " )
  endif()

  set( _STUBS "" )
  if( _PAR_STUBS )
    set( _STUBS "STUBS" )
  endif()

  lookup_omp_flags()

  set( OMP${_STUBS}_FOUND TRUE )

  foreach( _LANG ${_PAR_COMPONENTS} )

    if( NOT OMP${_STUBS}_${_LANG}_FLAGS )

      if( DEFINED _OMP${_STUBS}_FLAG_${CMAKE_${_LANG}_COMPILER_ID} )
        set( _OMP${_STUBS}_${_LANG}_FLAG "${_OMP${_STUBS}_FLAG_${CMAKE_${_LANG}_COMPILER_ID}}" )
      endif()
      if( CMAKE_${_LANG}_COMPILER_LOADED AND _OMP${_STUBS}_${_LANG}_FLAG )
        set(SAVE_CMAKE_REQUIRED_FLAGS "${CMAKE_REQUIRED_FLAGS}")
        set(CMAKE_REQUIRED_FLAGS "${_OMP${_STUBS}_${_LANG}_FLAG}")
        include(Check${_LANG}SourceCompiles)
        set( _SOURCE ${_OMP${_STUBS}_${_LANG}_TEST_SOURCE} )
        set( _FLAG ${_LANG}_COMPILER_SUPPORTS_OMP${_STUBS})
        if( _LANG STREQUAL "C" )
          check_c_source_compiles("${_SOURCE}" ${_FLAG} )
        endif()
        if( _LANG STREQUAL "CXX" )
          check_cxx_source_compiles("${_SOURCE}" ${_FLAG} )
        endif()
        if( _LANG STREQUAL "Fortran" )
          check_fortran_source_compiles("${_SOURCE}" ${_FLAG} )
        endif()
        set(CMAKE_REQUIRED_FLAGS "${SAVE_CMAKE_REQUIRED_FLAGS}")
      endif()

      if( ${_LANG}_COMPILER_SUPPORTS_OMP${_STUBS} )
        set( OMP${_STUBS}_${_LANG}_FLAGS ${_OMP${_STUBS}_${_LANG}_FLAG} )
      endif()

    else()
      set( ${_LANG}_COMPILER_SUPPORTS_OMP${_STUBS} TRUE )
    endif()


    set( OMP${_STUBS}_${_LANG}_FIND_QUIETLY TRUE )
    find_package_handle_standard_args( OMP${_STUBS}_${_LANG} REQUIRED_VARS ${_LANG}_COMPILER_SUPPORTS_OMP${_STUBS}  )

    if( OMP${_STUBS}_FORTRAN_FOUND )
      set( OMP${_STUBS}_Fortran_FOUND TRUE )
    endif()

    if( NOT OMP${_STUBS}_${_LANG}_FOUND )
      set( OMP${_STUBS}_FOUND FALSE )
    endif()

    if( _PAR_STUBS )
      set( OMP_${_LANG}_FOUND ${OMPSTUBS_${_LANG}_FOUND} )
      set( OMP_${_LANG}_FLAGS ${OMPSTUBS_${_LANG}_FLAGS} )
    endif()

  endforeach()

  if( _PAR_STUBS )
    set( OMP_FOUND ${OMPSTUBS_FOUND} )
  endif()

endmacro( ecbuild_find_omp )

##############################################################################
# macro for enabling openmp

macro( ecbuild_enable_omp )

  ecbuild_find_omp( COMPONENTS C CXX Fortran )

  if( OMP_C_FOUND )
    set( CMAKE_C_FLAGS "${CMAKE_C_FLAGS} ${OMP_C_FLAGS}" )
  endif()

  if( OMP_CXX_FOUND )
    set( CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} ${OMP_CXX_FLAGS}" )
  endif()

  if( OMP_Fortran_FOUND )
    set( CMAKE_Fortran_FLAGS "${CMAKE_Fortran_FLAGS} ${OMP_Fortran_FLAGS}" )
  endif()

endmacro( ecbuild_enable_omp )

##############################################################################
# macro for enabling openmp stubs

macro( ecbuild_enable_ompstubs )

  ecbuild_find_omp( COMPONENTS C CXX Fortran STUBS )

  if( OMPSTUBS_C_FOUND )
    set( CMAKE_C_FLAGS "${CMAKE_C_FLAGS} ${OMPSTUBS_C_FLAGS}" )
  endif()

  if( OMPSTUBS_CXX_FOUND )
    set( CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} ${OMPSTUBS_CXX_FLAGS}" )
  endif()

  if( OMPSTUBS_Fortran_FOUND )
    set( CMAKE_Fortran_FLAGS "${CMAKE_Fortran_FLAGS} ${OMPSTUBS_Fortran_FLAGS}" )
  endif()

endmacro( ecbuild_enable_ompstubs )
