namespace NativeEngine;

internal enum DeviceSpecificInfo_t
{
	DSI_D3D9_DEVICE,                            // get the IDirect3DDevice9 device, can be NULL for n/a devices
	DSI_D3D_PRESENT_PARAMETERS,                 // get the D3DPRESENT_PARAMETERS, can be NULL for n/a devices

	DSI_D3D11_DEVICE,                           // get the ID3D11Device device, can be NULL for n/a devices
	DSI_D3D11_DEVICE_IMMEDIATE_CONTEXT,         // get the immediate ID3D11DeviceContext, can be NULL for n/a devices
	DSI_VULKAN_INSTANCE,                        // get the VkInstance, can be NULL for n/a devices
	DSI_VULKAN_PHYSICAL_DEVICE,                 // get the VkPhysicalDevice, can be NULL for n/a devices
	DSI_VULKAN_DEVICE,                          // get the VkDevice, can be NULL for n/a devices
	DSI_VULKAN_QUEUE,                           // get the VkQueue, can be NULL for n/a devices
	DSI_VULKAN_MEMORY_PROPERTIES,               // get the VkPhysicalDeviceMemoryProperties, can be NULL for n/a devices
	DSI_VULKAN_QUEUE_FAMILY_INDEX,              // get the Vulkan queueFamilyIndex, can be NULL for n/a devices
	DSI_VULKAN_RAY_TRACING_PIPELINE_PROPERTIES  // get the VkPhysicalDeviceRayTracingPipelinePropertiesKHR, can be NULL for n/a devices
};
