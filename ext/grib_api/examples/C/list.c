/*
 * Copyright 2005-2016 ECMWF.
 *
 * This software is licensed under the terms of the Apache Licence Version 2.0
 * which can be obtained at http://www.apache.org/licenses/LICENSE-2.0.
 *
 * In applying this licence, ECMWF does not waive the privileges and immunities granted to it by
 * virtue of its status as an intergovernmental organisation nor does it submit to any jurisdiction.
 */

/*
 * C Implementation: list
 *
 * Description: how to get values using keys.
 *
 */
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>

#include "grib_api.h"

int main(int argc, char** argv)
{
	int err = 0;

	size_t i = 0;
	size_t count;
	size_t size;

	long numberOfContributingSpectralBands;
	long values[1024];
	long new_values[1024];

	FILE* in = NULL;
	char* filename = "../../data/satellite.grib";
	grib_handle *h = NULL;

	in = fopen(filename,"r");
	if(!in) {
		printf("ERROR: unable to open input file %s\n",filename);
		return 1;
	}

	/* create new handle from a message in a file*/
	h = grib_handle_new_from_file(0,in,&err);
	if (h == NULL) {
		printf("Error: unable to create handle from file %s\n",filename);
	}
	
	GRIB_CHECK(grib_get_long(h,"numberOfContributingSpectralBands",&numberOfContributingSpectralBands),0);
	assert(numberOfContributingSpectralBands == 3);

	/* Shrink NB to 2 */
	numberOfContributingSpectralBands = 2;
	GRIB_CHECK(grib_set_long(h,"numberOfContributingSpectralBands",numberOfContributingSpectralBands),0);

	/* Expand NB to 9 */
	numberOfContributingSpectralBands = 9;
	GRIB_CHECK(grib_set_long(h,"numberOfContributingSpectralBands",numberOfContributingSpectralBands),0);

	/* get as a long*/
	GRIB_CHECK(grib_get_long(h,"numberOfContributingSpectralBands",&numberOfContributingSpectralBands),0);
	printf("numberOfContributingSpectralBands=%ld\n",numberOfContributingSpectralBands);

	/* get as a long*/
	GRIB_CHECK(grib_get_size(h,"scaledValueOfCentralWaveNumber",&count),0);
	printf("count=%ld\n",(long)count);

	assert(count < sizeof(values)/sizeof(values[0]));

	size = count;
	GRIB_CHECK(grib_get_long_array(h,"scaledValueOfCentralWaveNumber",values,&size),0);
	assert(size == count);

	for(i=0;i<count;i++) {
		printf("scaledValueOfCentralWaveNumber %lu = %ld\n",(unsigned long)i,values[i]);
		if (i == 0) assert( values[i] == 26870 );
		if (i == 1) assert( values[i] == 9272  );
	}

	for(i=0;i<count;i++)
		values[i] = i+1000;

	size = count;
	/* size--; */
	GRIB_CHECK(grib_set_long_array(h,"scaledValueOfCentralWaveNumber",values,size),0);
	assert(size == count);
	
	/* check what we set */
	GRIB_CHECK(grib_get_long_array(h,"scaledValueOfCentralWaveNumber",new_values,&size),0);
	assert(size == count);
	for(i=0;i<count;i++) {
		printf("Now scaledValueOfCentralWaveNumber %lu = %ld\n",(unsigned long)i,new_values[i]);
		assert( new_values[i] == (i+1000) );
	}

	grib_handle_delete(h);

	fclose(in);
	return 0;
}
