#include <stddef.h>
#include <stdlib.h>
#include <assert.h>
char* Resize(unsigned char * buffer,size_t currentSize,size_t newSize){
	assert(buffer!=0);
	if (currentSize < newSize)
	{
		buffer = (unsigned char *)realloc(buffer, newSize);
	}
	return buffer;
}

char * badResize(unsigned char * buffer,size_t currentSize,size_t newSize)
{
	if(!buffer)
		exit(0);
	if (currentSize < newSize)
	{
		buffer = (unsigned char *)realloc(buffer, newSize);
	}
	return buffer;
}
char * badResize1(unsigned char * buffer,size_t currentSize,size_t newSize)
{
	// BAD: on unsuccessful call to realloc, we will lose a pointer to a valid memory block
	if (currentSize < newSize)
	{
		buffer = (unsigned char *)realloc(buffer, newSize);
	}
	return buffer;
}
int main(){
	
}