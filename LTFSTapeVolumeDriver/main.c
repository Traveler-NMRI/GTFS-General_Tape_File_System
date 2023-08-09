#include <ntddk.h>
#include <ntdddisk.h>
//#include <windows.h>
#define IOCTL_LTFSVOLUME_TAPEINFO CTL_CODE(FILE_DEVICE_DISK,0x800,METHOD_BUFFERED,FILE_READ_DATA | FILE_WRITE_DATA)
#define IOCTL_LTFSVOLUME_CREATE_VOLUME CTL_CODE(FILE_DEVICE_DISK,0x801,METHOD_BUFFERED,FILE_READ_DATA | FILE_WRITE_DATA)
#define IOCTL_LTFSVOLUME_REMOVE_VOLUME CTL_CODE(FILE_DEVICE_DISK,0x802,METHOD_BUFFERED,FILE_READ_DATA | FILE_WRITE_DATA)
NTSTATUS LTFSCreateClose(IN PDEVICE_OBJECT DeviceObject,IN PIRP Irp);
NTSTATUS DriverEntry(PDRIVER_OBJECT driver, PUNICODE_STRING reg_path);
VOID DriverUnload(PDRIVER_OBJECT driver);
NTSTATUS LTFSReadWrite(IN PDEVICE_OBJECT DeviceObject,IN PIRP Irp);
NTSTATUS LTFSDeviceControl(IN PDEVICE_OBJECT DeviceObject,IN PIRP Irp);
NTSTATUS LTFSCreateVolume(IN OUT PDEVICE_OBJECT ltfsobj,IN ULONG tapenum,IN BOOLEAN tapereadonly);
NTSTATUS LTFSRemoveVolume(IN OUT PDEVICE_OBJECT ltfsobj,IN ULONG tapenum);
typedef unsigned long DWORD;
typedef struct _TAPE_SET_DRIVE_PARAMETERS {
  BOOLEAN ECC;
  BOOLEAN Compression;
  BOOLEAN DataPadding;
  BOOLEAN ReportSetmarks;
  DWORD   EOTWarningZoneSize;
} TAPE_SET_DRIVE_PARAMETERS, *PTAPE_SET_DRIVE_PARAMETERS;
typedef struct _TAPE_SET_MEDIA_PARAMETERS {
  DWORD BlockSize;
} TAPE_SET_MEDIA_PARAMETERS, *PTAPE_SET_MEDIA_PARAMETERS;
typedef struct _TAPE_GET_MEDIA_PARAMETERS {
  LARGE_INTEGER Capacity;
  LARGE_INTEGER Remaining;
  DWORD         BlockSize;
  DWORD         PartitionCount;
  BOOLEAN       WriteProtected;
} TAPE_GET_MEDIA_PARAMETERS, *PTAPE_GET_MEDIA_PARAMETERS;
typedef struct _TAPE_GET_DRIVE_PARAMETERS {
  BOOLEAN ECC;
  BOOLEAN Compression;
  BOOLEAN DataPadding;
  BOOLEAN ReportSetmarks;
  DWORD   DefaultBlockSize;
  DWORD   MaximumBlockSize;
  DWORD   MinimumBlockSize;
  DWORD   MaximumPartitionCount;
  DWORD   FeaturesLow;
  DWORD   FeaturesHigh;
  DWORD   EOTWarningZoneSize;
} TAPE_GET_DRIVE_PARAMETERS, *PTAPE_GET_DRIVE_PARAMETERS;
typedef struct _TAPE_GET_INFO{
	unsigned int tape_num;
	DWORD tapedrive_status;
	TAPE_GET_MEDIA_PARAMETERS get_tapemedia_info;
	TAPE_GET_DRIVE_PARAMETERS get_tapedrive_info;
	DWORD tape_part;
	DWORD tape_low;
	DWORD tape_high;
}TAPE_GET_INFO,*PTAPE_GET_INFO;
typedef struct _TAPE_SET_INFO{
	unsigned int tape_code;
	unsigned int tape_num;
	TAPE_SET_MEDIA_PARAMETERS set_tapemedia_info;
	TAPE_SET_DRIVE_PARAMETERS set_tapedrive_info;
	DWORD tape_part;
	DWORD tape_low;
	DWORD tape_high;
}TAPE_SET_INFO,*PTAPE_SET_INFO;
typedef struct _LTFS_VOLUME_CREATE{
	unsigned int isreadonly;
	unsigned int tapenum;
}LTFS_VOLUME_CREATE,*PLTFS_VOLUME_CREATE;
typedef struct _PDEVICE_EXTENSION{
	unsigned int type; //type=0 control,type=1 volume
	unsigned int tape_num; //if the device is a control,tape_num will be 0
	unsigned int device_num; //if the device is a control,device_num will be 0.Else,device_num will be like this "\\Device\\LtfsVolumeN",the 'N' is device_num + 1;
}DEVICE_EXTENSION,*PDEVICE_EXTENSION;
TAPE_SET_INFO tapesetinfo,deviotape;
PTAPE_GET_INFO tapegetinfo;
PDEVICE_OBJECT drivecontrol_object;
UNICODE_STRING drivecontrol_name,drivecontrol_link,drivevol_name,drivevol_link;
BOOLEAN ltfsnum;
PDRIVER_OBJECT driver_all;
VOID DriverUnload(PDRIVER_OBJECT driver)
{	
	NTSTATUS status;
	PDEVICE_OBJECT device_object;
	ULONG temp; 
	DbgPrint("LTFS Volume Driver Unloading!\r\n");
	for(device_object = driver->DeviceObject;device_object!= NULL;device_object = device_object->NextDevice)
    {
		if (((PDEVICE_EXTENSION)device_object->DeviceExtension)->type == 0)
			continue;
		temp=(((PDEVICE_EXTENSION)device_object->DeviceExtension)->device_num)-1;
		status=LTFSRemoveVolume(device_object,temp);
		if(status!=STATUS_SUCCESS){
			DbgPrint("LTFS Driver: Delete ltfs volume faild! %#x\r\n",status);
		}
	}
	status=IoDeleteSymbolicLink(&drivecontrol_link);
	if(status!=STATUS_SUCCESS){
		DbgPrint("LTFS Driver: Delete ltfs volume control link faild! %#x\r\n",status);
	}
	IoDeleteDevice(drivecontrol_object); 
	DbgPrint("LTFS Volume Driver Unloaded!\r\n");
}
 
NTSTATUS DriverEntry(PDRIVER_OBJECT driver, PUNICODE_STRING reg_path)
{	
	NTSTATUS status_drive;
	DbgPrint("LTFS Driver Loading! %#x\r\n",IOCTL_LTFSVOLUME_CREATE_VOLUME);
	RtlInitUnicodeString(&drivecontrol_name,L"\\Device\\LtfsControl");
	RtlInitUnicodeString(&drivecontrol_link,L"\\DosDevices\\LtfsControl");
	status_drive=IoCreateDevice(driver,sizeof(DEVICE_EXTENSION),&drivecontrol_name,FILE_DEVICE_CONTROLLER,0,FALSE,&drivecontrol_object);
	if(status_drive!=STATUS_SUCCESS){
		DbgPrint("LTFS Diver: Create ltfs volume control faild! %#x\r\n",status_drive);
	}
	status_drive=IoCreateSymbolicLink(&drivecontrol_link, &drivecontrol_name);
	if(status_drive!=STATUS_SUCCESS){
		DbgPrint("LTFS Diver: Create ltfs volume control link faild! %#x\r\n",status_drive);
	}
	((PDEVICE_EXTENSION) drivecontrol_object->DeviceExtension)->type=0;
	((PDEVICE_EXTENSION) drivecontrol_object->DeviceExtension)->tape_num=0;
	driver->MajorFunction[IRP_MJ_CREATE]=LTFSCreateClose;
	driver->MajorFunction[IRP_MJ_CLOSE]=LTFSCreateClose;
	driver->MajorFunction[IRP_MJ_READ]=LTFSReadWrite;
	driver->MajorFunction[IRP_MJ_WRITE]=LTFSReadWrite;
	driver->MajorFunction[IRP_MJ_DEVICE_CONTROL]=LTFSDeviceControl;
	driver->DriverUnload = DriverUnload;
	driver_all=driver;
	ltfsnum=0;
	DbgPrint("LTFS Driver Loaded!\r\n");
	return status_drive;
}
NTSTATUS LTFSCreateVolume(IN OUT PDEVICE_OBJECT ltfsobj,IN ULONG tapenum,IN BOOLEAN tapereadonly){
	NTSTATUS status;
	//ULONG device_characteristics;
	UNICODE_STRING udrivevol_name,udrivevol_link;
	WCHAR wdrivevol_name[]=L"\\Device\\LtfsVolumeN",wdrivevol_link[]=L"\\DosDevices\\Global\\LtfsVolumeN";
	DbgPrint("LTFS Contol: Creating new ltfs volume for TAPE%d!\r\n",tapenum);
	wdrivevol_name[18]=48+tapenum;
	wdrivevol_link[29]=48+tapenum;
	RtlInitUnicodeString(&udrivevol_name,wdrivevol_name);
	RtlInitUnicodeString(&udrivevol_link,wdrivevol_link);
	//device_characteristics=FILE_REMOVABLE_MEDIA;
	/*if(tapereadonly){
		device_characteristics |=FILE_READ_ONLY_DEVICE;
	}*/
	status=IoCreateDevice(driver_all,sizeof(DEVICE_EXTENSION),&udrivevol_name,FILE_DEVICE_DISK,0,FALSE,&ltfsobj);
	if(status!=STATUS_SUCCESS){
		DbgPrint("LTFS Contol: Create new ltfs volume faild for TAPE%d! %#x\r\n",tapenum,status);
		return status;
	}
	status=IoCreateSymbolicLink(&udrivevol_link, &udrivevol_name);
	if(status!=STATUS_SUCCESS){
		DbgPrint("LTFS Control: Create new ltfs volume link faild for TAPE%d! %#x\r\n",tapenum,status);
		return status;
	}
	((PDEVICE_EXTENSION)ltfsobj->DeviceExtension)->type=1;
	((PDEVICE_EXTENSION)ltfsobj->DeviceExtension)->tape_num=tapenum;
	((PDEVICE_EXTENSION)ltfsobj->DeviceExtension)->device_num=tapenum+1;
	ltfsnum=ltfsnum+1;
	ltfsobj->Flags &=~DO_DEVICE_INITIALIZING;
	DbgPrint("LTFS Contol: Created new ltfs volume successful for TAPE%d!\r\n",tapenum);
	return status;
}
NTSTATUS LTFSRemoveVolume(IN OUT PDEVICE_OBJECT ltfsobj,IN ULONG tapenum){
	NTSTATUS status;
	UNICODE_STRING udrivevol_link;
	WCHAR wdrivevol_link[]=L"\\DosDevices\\Global\\LtfsVolumeN";
	wdrivevol_link[29]=47+((PDEVICE_EXTENSION)ltfsobj->DeviceExtension)->device_num;
	RtlInitUnicodeString(&udrivevol_link,wdrivevol_link);
	DbgPrint("LTFS Control: Deleting ltfs volume for TAPE%d!\r\n",tapenum);
	status=IoDeleteSymbolicLink(&udrivevol_link);
	if(status!=STATUS_SUCCESS){
		DbgPrint("LTFS Control: Delete ltfs volume link faild! %#x\r\n",status);
		return status;
	}
	IoDeleteDevice(ltfsobj); 
	ltfsnum=ltfsnum-1;
	DbgPrint("LTFS Contol: Deleted new ltfs volume successful for TAPE%d!\r\n",tapenum);
	return status;
}
NTSTATUS LTFSCreateClose(IN PDEVICE_OBJECT DeviceObject,IN PIRP Irp)
{
	PIO_STACK_LOCATION pIrpStack;
	pIrpStack=IoGetCurrentIrpStackLocation(Irp);
	if (pIrpStack->FileObject->FileName.Length != 0)
    {
      DbgPrint("Attempt to open '%.*ws' on device.\r\n",(int)(pIrpStack->FileObject->FileName.Length / sizeof(WCHAR),pIrpStack->FileObject->FileName.Buffer));
      Irp->IoStatus.Status = STATUS_OBJECT_NAME_NOT_FOUND;
      Irp->IoStatus.Information = 0;
      IoCompleteRequest(Irp, IO_NO_INCREMENT);
      return STATUS_OBJECT_NAME_NOT_FOUND;
    }
	DbgPrint("LTFS Successfully create/close! a handle!\r\n");
	Irp->IoStatus.Status = STATUS_SUCCESS;
	Irp->IoStatus.Information = FILE_OPENED;
	IoCompleteRequest(Irp, IO_NO_INCREMENT);
	return STATUS_SUCCESS;
}
NTSTATUS LTFSReadWrite(IN PDEVICE_OBJECT DeviceObject,IN PIRP Irp)
{
	DbgPrint("LTFS Read or Write!\r\n");
	return STATUS_SUCCESS;
}
NTSTATUS LTFSDeviceControl(IN PDEVICE_OBJECT DeviceObject,IN PIRP Irp)
{
	NTSTATUS status;
	PDEVICE_OBJECT ltfsvol;
	//DISK_GEOMETRY TAPEGEOMETRY;
	unsigned __int64 pltfscreate;
	PIO_STACK_LOCATION pIrpStack;
	ULONG IrpCode;
	//ULONG tape_num;
	pIrpStack=IoGetCurrentIrpStackLocation(Irp);
	IrpCode=pIrpStack->Parameters.DeviceIoControl.IoControlCode;
	status=0;
	switch(IrpCode){
	case IOCTL_LTFSVOLUME_CREATE_VOLUME:
		DbgPrint("IOCTL_LTFSVOLUME_CREATE_VOLUME\r\n");
		memset(&ltfsvol,0,sizeof(ltfsvol));
		RtlMoveMemory(&pltfscreate,&Irp->AssociatedIrp.SystemBuffer,8);
		status=LTFSCreateVolume(ltfsvol,((PLTFS_VOLUME_CREATE)pltfscreate)->tapenum,((PLTFS_VOLUME_CREATE)pltfscreate)->isreadonly);
		if(status!=STATUS_SUCCESS){
			DbgPrint("LTFS Contol: Create new ltfs volume faild for TAPE%d! %#x\r\n",((PLTFS_VOLUME_CREATE)pltfscreate)->tapenum,status);
			Irp->IoStatus.Information = 0;
			break;
		}
		status=STATUS_SUCCESS;
		Irp->IoStatus.Information = 0;
		break;
	/*case IOCTL_LTFSVOLUME_TAPEINFO:
		DbgPrint("IOCTL_LTFSVOLUME_TAPEINFO\r\n");
		memset(&tapegetinfo,0,sizeof(tapegetinfo));
		RtlMoveMemory(&tapegetinfo,&Irp->AssociatedIrp.SystemBuffer,sizeof(tapegetinfo));
		memset(&tapesetinfo,0,sizeof(tapesetinfo));
		tapesetinfo.tape_num=0;
		tapesetinfo.tape_code=0;
		RtlMoveMemory(Irp->AssociatedIrp.SystemBuffer,&tapesetinfo,sizeof(tapesetinfo));
		status=STATUS_SUCCESS;
		Irp->IoStatus.Information = sizeof(tapesetinfo);
		break;
	case IOCTL_DISK_GET_DRIVE_GEOMETRY:
		DbgPrint("IOCTL_DISK_GET_DRIVE_GEOMETRY\r\n");
		if (pIrpStack->Parameters.DeviceIoControl.OutputBufferLength < sizeof(DISK_GEOMETRY))
		{
			DbgPrint("LTFS Volume: Buffer too small!\r\n");
			status = STATUS_BUFFER_TOO_SMALL;
			Irp->IoStatus.Information = 0;
			break;
		}
		if(tapegetinfo->tapedrive_status==1112){
			DbgPrint("LTFS Volume: No tape in device.\r\n");
			status=STATUS_NO_MEDIA_IN_DEVICE;
			Irp->IoStatus.Information = 0;
			break;
		}
		memset(&TAPEGEOMETRY,0,sizeof(TAPEGEOMETRY));
		TAPEGEOMETRY.Cylinders.QuadPart=34679;
		TAPEGEOMETRY.BytesPerSector=tapegetinfo->get_tapemedia_info.BlockSize;
		TAPEGEOMETRY.MediaType=RemovableMedia;
		TAPEGEOMETRY.SectorsPerTrack=4;
		TAPEGEOMETRY.TracksPerCylinder=44;
        RtlMoveMemory(Irp->AssociatedIrp.SystemBuffer,&TAPEGEOMETRY,sizeof(DISK_GEOMETRY));
		status = STATUS_SUCCESS;
		Irp->IoStatus.Information = sizeof(TAPEGEOMETRY);
		break;*/
	/*case IOCTL_DISK_GET_PARTITION_INFO:
		DbgPrint("IOCTL_DISK_GET_PARTITION_INFO\r\n");
		break;
	case IOCTL_DISK_SET_PARTITION_INFO:
		DbgPrint("IOCTL_DISK_SET_PARTITION_INFO\r\n");
		break;
	case IOCTL_DISK_GET_DRIVE_LAYOUT:
		DbgPrint("IOCTL_DISK_GET_DRIVE_LAYOUT\r\n");
		break;
	case IOCTL_DISK_SET_DRIVE_LAYOUT:
		DbgPrint("IOCTL_DISK_SET_DRIVE_LAYOUT\r\n");
		break;
	case IOCTL_DISK_VERIFY:
		DbgPrint("IOCTL_DISK_VERIFY\r\n");
		break;
	case IOCTL_DISK_FORMAT_TRACKS:
		DbgPrint("IOCTL_DISK_FORMAT_TRACKS\r\n");
		break;
	case IOCTL_DISK_REASSIGN_BLOCKS:
		DbgPrint("IOCTL_DISK_REASSIGN_BLOCKS\r\n");
		break;
	case IOCTL_DISK_PERFORMANCE:
		DbgPrint("IOCTL_DISK_PERFORMANCE\r\n");
		break;*/
	/*case IOCTL_DISK_IS_WRITABLE:
		DbgPrint("IOCTL_DISK_IS_WRITABLE\r\n");
		if(tapegetinfo->tapedrive_status==1112){
			DbgPrint("LTFS Volume: No tape in device.\r\n");
			status = STATUS_NO_MEDIA_IN_DEVICE;
			Irp->IoStatus.Information = 0;
			break;
		}
		if(tapegetinfo->get_tapemedia_info.WriteProtected==1){
			DbgPrint("LTFS Volume: Tape write protected.\r\n");
			status = STATUS_MEDIA_WRITE_PROTECTED;
			Irp->IoStatus.Information = 0;
			break;
		}
		status = STATUS_SUCCESS;
		Irp->IoStatus.Information = 0;
		break;*/
	/*case IOCTL_DISK_LOGGING:
		DbgPrint("IOCTL_DISK_LOGGING\r\n");
		break;
	case IOCTL_DISK_FORMAT_TRACKS_EX:
		DbgPrint("IOCTL_DISK_FORMAT_TRACKS_EX");
		break;
	case IOCTL_DISK_HISTOGRAM_STRUCTURE:
		DbgPrint("IOCTL_DISK_HISTOGRAM_STRUCTURE\r\n");
		break;
	case IOCTL_DISK_HISTOGRAM_DATA:
		DbgPrint("IOCTL_DISK_HISTOGRAM_DATA\r\n");
		break;
	case IOCTL_DISK_HISTOGRAM_RESET:
		DbgPrint("IOCTL_DISK_HISTOGRAM_RESET\r\n");
		break;
	case IOCTL_DISK_REQUEST_STRUCTURE:
		DbgPrint("IOCTL_DISK_REQUEST_STRUCTURE\r\n");
		break;
	case IOCTL_DISK_REQUEST_DATA:
		DbgPrint("IOCTL_DISK_REQUEST_DATA\r\n");
		break;
	case IOCTL_DISK_PERFORMANCE_OFF:
		DbgPrint("IOCTL_DISK_PERFORMANCE_OFF\r\n");
		break;
	case IOCTL_DISK_CONTROLLER_NUMBER:
		DbgPrint("IOCTL_DISK_CONTROLLER_NUMBER\r\n");
		break;
	case IOCTL_DISK_GET_PARTITION_INFO_EX:
		DbgPrint("IOCTL_DISK_GET_PARTITION_INFO_EX\r\n");
		break;
	case IOCTL_DISK_SET_PARTITION_INFO_EX:
		DbgPrint("IOCTL_DISK_SET_PARTITION_INFO_EX\r\n");
		break;
	case IOCTL_DISK_GET_DRIVE_LAYOUT_EX:
		DbgPrint("IOCTL_DISK_GET_DRIVE_LAYOUT_EX\r\n");
		break;
	case IOCTL_DISK_SET_DRIVE_LAYOUT_EX:
		DbgPrint("IOCTL_DISK_SET_DRIVE_LAYOUT_EX\r\n");
		break;
	case IOCTL_DISK_CREATE_DISK:
		DbgPrint("IOCTL_DISK_CREATE_DISK\r\n");
		break;*/
	/*case IOCTL_DISK_GET_LENGTH_INFO:
		DbgPrint("IOCTL_DISK_GET_LENGTH_INFO\r\n");
		if (pIrpStack->Parameters.DeviceIoControl.OutputBufferLength < sizeof(GET_LENGTH_INFORMATION))
		{
			DbgPrint("LTFS Volume: Buffer too small!\r\n");
			status = STATUS_BUFFER_TOO_SMALL;
			Irp->IoStatus.Information = 0;
			break;
		}
		if (tapegetinfo->tapedrive_status==1112){
			DbgPrint("LTFS Volume: No tape in device.\r\n");
			status = STATUS_NO_MEDIA_IN_DEVICE;
			Irp->IoStatus.Information = 0;
			break;
		}
		((PGET_LENGTH_INFORMATION) Irp->AssociatedIrp.SystemBuffer)->Length.QuadPart =34679;
		status=STATUS_SUCCESS;
		Irp->IoStatus.Information = sizeof(GET_LENGTH_INFORMATION);
		break;*/
	/*case IOCTL_DISK_GET_DRIVE_GEOMETRY_EX:
		DbgPrint("IOCTL_DISK_GET_DRIVE_GEOMETRY_EX\r\n");
		break;
	case IOCTL_DISK_REASSIGN_BLOCKS_EX:
		DbgPrint("IOCTL_DISK_REASSIGN_BLOCKS_EX\r\n");
		break;
	case IOCTL_DISK_UPDATE_DRIVE_SIZE:
		DbgPrint("IOCTL_DISK_UPDATE_DRIVE_SIZE\r\n");
		break;
	case IOCTL_DISK_GROW_PARTITION:
		DbgPrint("IOCTL_DISK_GROW_PARTITION\r\n");
		break;
	case IOCTL_DISK_GET_CACHE_INFORMATION:
		DbgPrint("IOCTL_DISK_GET_CACHE_INFORMATION\r\n");
		break;
	case IOCTL_DISK_SET_CACHE_INFORMATION:
		DbgPrint("IOCTL_DISK_SET_CACHE_INFORMATION\r\n");
		break;
	case OBSOLETE_DISK_GET_WRITE_CACHE_STATE:
		DbgPrint("OBSOLETE_DISK_GET_WRITE_CACHE_STATE\r\n");
		break;
	case IOCTL_DISK_DELETE_DRIVE_LAYOUT:
		DbgPrint("IOCTL_DISK_DELETE_DRIVE_LAYOUT\r\n");
		break;
	case IOCTL_DISK_UPDATE_PROPERTIES:
		DbgPrint("IOCTL_DISK_UPDATE_PROPERTIES\r\n");
		break;
	case IOCTL_DISK_FORMAT_DRIVE:
		DbgPrint("IOCTL_DISK_FORMAT_DRIVE\r\n");
		break;
	case IOCTL_DISK_SENSE_DEVICE:
		DbgPrint("IOCTL_DISK_SENSE_DEVICE\r\n");
		break;
	case IOCTL_DISK_GET_CACHE_SETTING:
		DbgPrint("IOCTL_DISK_GET_CACHE_SETTING\r\n");
		break;
	case IOCTL_DISK_SET_CACHE_SETTING:
		DbgPrint("IOCTL_DISK_SET_CACHE_SETTING\r\n");
		break;
	case IOCTL_DISK_COPY_DATA:
		DbgPrint("IOCTL_DISK_COPY_DATA\r\n");
		break;
	case IOCTL_DISK_INTERNAL_SET_VERIFY:
		DbgPrint("IOCTL_DISK_INTERNAL_SET_VERIFY\r\n");
		break;
	case IOCTL_DISK_INTERNAL_CLEAR_VERIFY:
		DbgPrint("IOCTL_DISK_INTERNAL_CLEAR_VERIFY\r\n");
		break;
	case IOCTL_DISK_INTERNAL_SET_NOTIFY:
		DbgPrint("IOCTL_DISK_INTERNAL_SET_NOTIFY\r\n");
		break;
	case IOCTL_DISK_CHECK_VERIFY:
		DbgPrint("IOCTL_DISK_CHECK_VERIFY\r\n");
		break;*/
	/*case IOCTL_STORAGE_MEDIA_REMOVAL:
	case IOCTL_DISK_MEDIA_REMOVAL:
		DbgPrint("IOCTL_DISK_MEDIA_REMOVAL\r\n");
		status = STATUS_SUCCESS;
		Irp->IoStatus.Information = 0;
		break;*/
	/*case IOCTL_DISK_EJECT_MEDIA:
		DbgPrint("IOCTL_DISK_EJECT_MEDIA\r\n");
		break;
	case IOCTL_DISK_LOAD_MEDIA:
		DbgPrint("IOCTL_DISK_LOAD_MEDIA\r\n");
		break;
	case IOCTL_DISK_RESERVE:
		DbgPrint("IOCTL_DISK_RESERVE\r\n");
		break;
	case IOCTL_DISK_RELEASE:
		DbgPrint("IOCTL_DISK_RELEASE\r\n");
		break;
	case IOCTL_DISK_FIND_NEW_DEVICES:
		DbgPrint("IOCTL_DISK_FIND_NEW_DEVICES\r\n");
		break;
	case IOCTL_DISK_GET_MEDIA_TYPES:
		DbgPrint("IOCTL_DISK_GET_MEDIA_TYPES\r\n");
		break;
	case SMART_GET_VERSION:
		DbgPrint("SMART_GET_VERSION\r\n");
		status=STATUS_INVALID_DEVICE_REQUEST;
		break;
	case IOCTL_DISK_GET_PARTITION_ATTRIBUTES:
		DbgPrint("IOCTL_DISK_GET_PARTITION_ATTRIBUTES\r\n");
		break;
	case IOCTL_DISK_SET_PARTITION_ATTRIBUTES:
		DbgPrint("IOCTL_DISK_SET_PARTITION_ATTRIBUTES\r\n");
		break;
	case IOCTL_DISK_GET_DISK_ATTRIBUTES:
		DbgPrint("IOCTL_DISK_GET_DISK_ATTRIBUTES\r\n");
		break;
	case IOCTL_DISK_SET_DISK_ATTRIBUTES:
		DbgPrint("IOCTL_DISK_SET_DISK_ATTRIBUTES\r\n");
		break;
	case IOCTL_DISK_IS_CLUSTERED:
		DbgPrint("IOCTL_DISK_IS_CLUSTERED\r\n");
		break;
	case IOCTL_DISK_GET_SAN_SETTINGS:
		DbgPrint("IOCTL_DISK_GET_SAN_SETTINGS\r\n");
		break;
	case IOCTL_DISK_SET_SAN_SETTINGS:
		DbgPrint("IOCTL_DISK_SET_SAN_SETTINGS\r\n");
		break;
	case IOCTL_DISK_GET_SNAPSHOT_INFO:
		DbgPrint("IOCTL_DISK_GET_SNAPSHOT_INFO\r\n");
		break;
	case IOCTL_DISK_SET_SNAPSHOT_INFO:
		DbgPrint("IOCTL_DISK_SET_SNAPSHOT_INFO\r\n");
		break;
	case IOCTL_DISK_RESET_SNAPSHOT_INFO:
		DbgPrint("IOCTL_DISK_RESET_SNAPSHOT_INFO\r\n");
		break;
	case IOCTL_DISK_SIMBAD:
		DbgPrint("IOCTL_DISK_SIMBAD\r\n");
		break;
	case IOCTL_STORAGE_BREAK_RESERVATION:
		DbgPrint("IOCTL_STORAGE_BREAK_RESERVATION\r\n");
		break;
	case IOCTL_STORAGE_CHECK_VERIFY:
		DbgPrint("IOCTL_STORAGE_CHECK_VERIFY\r\n");
		break;
	case IOCTL_STORAGE_CHECK_VERIFY2:
		DbgPrint("IOCTL_STORAGE_CHECK_VERIFY2\r\n");
		break;
	case IOCTL_STORAGE_EJECT_MEDIA:
		DbgPrint("IOCTL_STORAGE_EJECT_MEDIA\r\n");
		break;
	case IOCTL_STORAGE_EJECTION_CONTROL:
		DbgPrint("IOCTL_STORAGE_EJECTION_CONTROL\r\n");
		break;
	case IOCTL_STORAGE_FIND_NEW_DEVICES:
		DbgPrint("IOCTL_STORAGE_FIND_NEW_DEVICES\r\n");
		break;
	case IOCTL_STORAGE_GET_DEVICE_NUMBER:
		DbgPrint("IOCTL_STORAGE_GET_DEVICE_NUMBER\r\n");
		break;
	case IOCTL_STORAGE_GET_HOTPLUG_INFO:
		DbgPrint("IOCTL_STORAGE_GET_HOTPLUG_INFO\r\n");
		break;
	case IOCTL_STORAGE_GET_MEDIA_SERIAL_NUMBER:
		DbgPrint("IOCTL_STORAGE_GET_MEDIA_SERIAL_NUMBER\r\n");
		break;
	case IOCTL_STORAGE_GET_MEDIA_TYPES:
		DbgPrint("IOCTL_STORAGE_GET_MEDIA_TYPES\r\n");
		break;
	case IOCTL_STORAGE_GET_MEDIA_TYPES_EX:
		DbgPrint("IOCTL_STORAGE_GET_MEDIA_TYPES_EX\r\n");
		break;
	case IOCTL_STORAGE_LOAD_MEDIA:
		DbgPrint("IOCTL_STORAGE_LOAD_MEDIA\r\n");
		break;
	case IOCTL_STORAGE_LOAD_MEDIA2:
		DbgPrint("IOCTL_STORAGE_LOAD_MEDIA2\r\n");
		break;
	case IOCTL_STORAGE_MANAGE_DATA_SET_ATTRIBUTES:
		DbgPrint("IOCTL_STORAGE_MANAGE_DATA_SET_ATTRIBUTES\r\n");
		break;
	case IOCTL_STORAGE_MCN_CONTROL:
		DbgPrint("IOCTL_STORAGE_MCN_CONTROL\r\n");
		break;
	case IOCTL_STORAGE_PERSISTENT_RESERVE_IN:
		DbgPrint("IOCTL_STORAGE_PERSISTENT_RESERVE_IN\r\n");
		break;
	case IOCTL_STORAGE_PERSISTENT_RESERVE_OUT:
		DbgPrint("IOCTL_STORAGE_PERSISTENT_RESERVE_OUT\r\n");
		break;
	case IOCTL_STORAGE_PREDICT_FAILURE:
		DbgPrint("IOCTL_STORAGE_PREDICT_FAILURE\r\n");
		break;
	case IOCTL_STORAGE_READ_CAPACITY:
		DbgPrint("IOCTL_STORAGE_READ_CAPACITY\r\n");
		break;
	case IOCTL_STORAGE_QUERY_PROPERTY:
		DbgPrint("IOCTL_STORAGE_QUERY_PROPERTY\r\n");
		break;
	case IOCTL_STORAGE_RELEASE:
		DbgPrint("IOCTL_STORAGE_RELEASE\r\n");
		break;	
	case IOCTL_STORAGE_RESERVE:
		DbgPrint("IOCTL_STORAGE_RESERVE\r\n");
		break; 
	case IOCTL_STORAGE_RESET_BUS:
		DbgPrint("IOCTL_STORAGE_RESET_BUS\r\n");
		break;	
	case IOCTL_STORAGE_RESET_DEVICE:
		DbgPrint("IOCTL_STORAGE_RESET_DEVICE\r\n");
		break;	
	case IOCTL_STORAGE_SET_HOTPLUG_INFO:
		DbgPrint("IOCTL_STORAGE_SET_HOTPLUG_INFO\r\n");
		break;*/
	default:
		DbgPrint("Unknown IOCTL code:%08x\r\n",IrpCode);
		Irp->IoStatus.Information = 0;
		status=STATUS_INVALID_DEVICE_REQUEST;
		break;
	}
	Irp->IoStatus.Status = status;
	IoCompleteRequest(Irp, IO_NO_INCREMENT);
	return status;
}