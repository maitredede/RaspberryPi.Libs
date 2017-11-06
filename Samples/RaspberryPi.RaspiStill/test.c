#include <stdio.h>
#include <bcm_host.h>
#include <interface/mmal/mmal.h>
#include <interface/mmal/util/mmal_default_components.h>

int main(int argc, char** argv) {
	printf("sizeof(MMAL_COMPONENT_T)=%d\n", sizeof(MMAL_COMPONENT_T));
	MMAL_COMPONENT_T *camera = 0;
	int i = 0;
	mmal_component_create(MMAL_COMPONENT_DEFAULT_CAMERA, &camera);

	printf("Control port name : %s\n", camera->control->name);

	long int camPtr = (long int)camera;
	long int controlPtr = (long int)(&camera->control);
	long int namePtr = (long int)(&camera->control->name);

	printf("Control ptr, controlptr, nameptr : %p, %p, %p %d %d\n", camPtr, controlPtr, namePtr, controlPtr - camPtr, camera->control - namePtr);
	printf("Output ports : %d\n", camera->output_num);

	for (i; i < camera->output_num; i++) {
		printf("port %d: %s\n", i, camera->output[i]->name);
	}
	if (camera)
		mmal_component_destroy(camera);
}
