Imports System.Threading.Thread
Imports System.Runtime.InteropServices
Public Class LTFSManage
    Dim isserviceopened As Boolean
    Dim hTape As ULong
    Dim hControl As ULong
    Structure SERVICE_STATUS
        Dim dwServiceType As ULong
        Dim dwCurrentState As ULong
        Dim dwControlsAccepted As ULong
        Dim dwWin32ExitCode As ULong
        Dim dwServiceSpecificExitCode As ULong
        Dim dwCheckPoint As ULong
        Dim dwWaitHint As ULong
    End Structure
    Structure SECURITY_ATTRIBUTES
        Dim nLength As ULong
        Dim lpSecurityDescriptor As ULong
        Dim bInheritHandle As UInteger
    End Structure
    Structure TAPE_SET_DRIVE_PARAMETERS
        Dim ECC As Byte
        Dim Compression As Byte
        Dim DataPadding As Byte
        Dim ReportSetmarks As Byte
        Dim EOTWarningZoneSize As ULong
    End Structure
    Structure TAPE_SET_MEDIA_PARAMETERS
        Dim BlockSize As ULong
    End Structure
    Structure TAPE_GET_MEDIA_PARAMETERS
        Dim Capacity As ULong
        Dim Remaining As ULong
        Dim BlockSize As ULong
        Dim PartitionCount As ULong
        Dim WriteProtected As Byte
    End Structure
    Structure TAPE_GET_DRIVE_PARAMETERS
        Dim ECC As Byte
        Dim Compression As Byte
        Dim DataPadding As Byte
        Dim ReportSetmarks As Byte
        Dim DefaultBlockSize As ULong
    End Structure
    Structure TAPE_GET_INFO
        Dim tape_num As UInteger
        Dim tapedrive_status As ULong
        Dim get_tapemedia_info As TAPE_GET_MEDIA_PARAMETERS
        Dim get_tapedrive_info As TAPE_GET_DRIVE_PARAMETERS
        Dim tape_part As ULong
        Dim tape_low As ULong
        Dim tape_high As ULong
    End Structure
    Structure TAPE_SET_INFO
        Dim tape_code As UInteger
        Dim tape_num As UInteger
        Dim set_tapemedia_info As TAPE_SET_MEDIA_PARAMETERS
        Dim set_tapedrive_info As TAPE_SET_DRIVE_PARAMETERS
        Dim tape_part As ULong
        Dim tape_low As ULong
        Dim tape_high As ULong
    End Structure
    Structure LTFS_VOLUME_CREATE
        Dim isreadonly As UInteger
        Dim tapenum As UInteger
    End Structure
    Public Const GENERIC_READ As ULong = &H80000000UL
    Public Const GENERIC_WRITE As ULong = &H40000000UL
    'Public Const IOCTL_LTFSVOLUME_TAPEINFO As ULong = &H7E004UL
    Public Const IOCTL_LTFSVOLUME_CREATE_VOLUME As ULong = &H7E004UL
    'Public Const IOCTL_LTFSVOLUME_REMOVE_VOLUME As ULong = &HE2023UL
    Public Const INVALID_HANDLE_VALUE As ULong = 18446744073709551615UL
    Public Declare Function OpenSCManager Lib "advapi32.dll" Alias "OpenSCManagerA" (ByVal lpMachineName As String, ByVal lpDatabaseName As String, ByVal dwDesiredAccess As ULong) As ULong
    Public Declare Function GetLastError Lib "kernel32" Alias "GetLastError" () As ULong
    Public Declare Function CreateService Lib "advapi32.dll" Alias "CreateServiceA" (ByVal hSCManager As ULong, ByVal lpServiceName As String, ByVal lpDisplayName As String, ByVal dwDesiredAccess As ULong, ByVal dwServiceType As ULong, ByVal dwStartType As ULong, ByVal dwErrorControl As ULong, ByVal lpBinaryPathName As String, ByVal lpLoadOrderGroup As String, ByVal lpdwTagId As ULong, ByVal lpDependencies As String, ByVal lpServiceStartName As ULong, ByVal lpPassword As ULong) As ULong
    Public Declare Function CloseServiceHandle Lib "advapi32.dll" (ByVal hSCObject As ULong) As ULong
    Public Declare Function OpenService Lib "advapi32.dll" Alias "OpenServiceA" (ByVal hSCManager As ULong, ByVal lpServiceName As String, ByVal dwDesiredAccess As ULong) As ULong
    Public Declare Function StartServiceA Lib "advapi32.dll" (ByVal hService As ULong, ByVal dwNumServiceArgs As ULong, ByVal lpServiceArgVectors As ULong) As ULong
    Public Declare Function ControlService Lib "advapi32.dll" (ByVal hService As ULong, ByVal dwControl As ULong, ByVal lpServiceStatus As SERVICE_STATUS) As ULong
    Public Declare Function DeviceIoControl Lib "kernel32.dll" (ByVal hdevice As ULong, ByVal dwiocontrolcode As ULong, ByVal lpinbuffer As Object, ByVal ninbuffersize As ULong, ByVal lpoutbuffer As Object, ByVal noutbuffersize As ULong, ByRef lpbytesreturned As ULong, ByVal lpoverlapped As ULong) As ULong
    Public Declare Function CreateFile Lib "kernel32.dll" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As ULong, ByVal dwShareMode As ULong, ByVal lpSecurityAttributes As SECURITY_ATTRIBUTES, ByVal dwCreationDisposition As ULong, ByVal dwFlagsAndAttributes As ULong, ByVal hTemplateFile As ULong) As ULong
    Public Declare Function DeleteService Lib "advapi32.dll" (ByVal hService As ULong) As ULong
    Public Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As ULong) As ULong
    Public Declare Function GetTapeStatus Lib "kernel32.dll" (ByVal hDevice As ULong) As ULong
    Public Declare Function EraseTape Lib "kernel32.dll" (ByVal hDevice As ULong, ByVal dwEraseType As ULong, ByVal bImmediate As UInteger) As ULong
    Public Declare Function GetTapeParameters Lib "kernel32.dll" (ByVal hDevice As ULong, ByVal dwOperation As ULong, ByVal lpdwSize As ULong, ByVal lpTapeInformation As ULong) As ULong
    Public Declare Function GetTapePosition Lib "kernel32.dll" (ByVal hDevice As ULong, ByVal dwPositionType As ULong, ByVal lpdwPartition As ULong, ByVal lpdwOffsetLow As ULong, ByVal lpdwOffsetHigh As ULong) As ULong
    Public Declare Function PrepareTape Lib "kernel32.dll" (ByVal hDevice As ULong, ByVal dwOperation As ULong, ByVal hDevice As ULong, ByVal dwOperation As ULong, ByVal bImmediate As UInteger) As ULong
    Public Declare Function DefineDosDevice Lib "kernel32.dll" Alias "DefineDosDeviceA" (ByVal dwFlags As ULong, ByVal lpDeviceName As String, ByVal lpTargetPath As String) As ULong
    Private Sub LTFSManage_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Message.ShowMessage(2UI, "您真的要退出吗?", 4UI)
        Do While Message.pressbutton = 0UI
            Application.DoEvents()
        Loop
        If Message.pressbutton = 1UI Then
            e.Cancel = True
            Message.Hide()
        Else
            End
        End If
    End Sub
    Private Sub LTFSManage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As UInteger, tapenums As UInteger
        DriverLoad.Enabled = True
        DriverUnload.Enabled = False
        ServiceStop.Enabled = False
        ServiceStart.Enabled = False
        isLTOLOCATE.Enabled = False
        isLTOReadOnly.Enabled = False
        SelectDiskDrives.Enabled = False
        SelectTapeDrives.Enabled = False
        MountButton.Enabled = False
        UnmountButton.Enabled = False
        isserviceopened = False
        tapenums = TapeDriveSearch()
        For i = 1 To tapenums
            SelectTapeDrives.Items.Add("TAPE" + CStr(i - 1UI))
        Next
        If tapenums > 0 Then
            SelectTapeDrives.SelectedIndex = 0
        End If

    End Sub

    Private Sub DriverLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DriverLoad.Click
        Message.ShowMessage(7UI, "准备启动服务..." + vbCrLf + vbCrLf + "这可能需要一些时间...", 1UI)
        Application.DoEvents()
        Sleep(100)
        If isserviceopened = True Then
            Message.SetDialogInfo(27UI, "服务已经被启动过一次,无法再次启动!" + vbCrLf + vbCrLf + "如要再次启动,请重新运行本程序!", 2UI)
            Exit Sub
        End If
        Message.SetDialogInfo(8UI, "正在启动服务..." + vbCrLf + vbCrLf + "这可能需要一些时间...", 1UI)
        DriverStatus.Text = "驱动状态: 驱动正启动"
        Application.DoEvents()
        If LoadNTDriver("LTFSTapeVolumeDriver", "G:\LTFSTapeVolume\x64\DriverManage\LTFSTapeVolumeDriver.sys") = True Then
            DriverUnload.Enabled = True
            DriverLoad.Enabled = False
            isLTOLOCATE.Enabled = True
            isLTOReadOnly.Enabled = True
            SelectDiskDrives.Enabled = True
            SelectTapeDrives.Enabled = True
            MountButton.Enabled = True
            UnmountButton.Enabled = False
            Message.SetDialogInfo(6UI, "启动服务成功!" + vbCrLf + vbCrLf + "对话框自动关闭", 1UI)
            DriverStatus.Text = "驱动状态: 驱动正运行"
            Application.DoEvents()
            Sleep(100)
            Message.Hide()
            Exit Sub
        End If
        DriverStatus.Text = "驱动状态: 驱动未运行"
        Message.SetDialogInfo(1UI, "打开服务失败!" + vbCrLf + vbCrLf + "具体参见调试讯息!", 2UI)
    End Sub
    Public Function LoadNTDriver(ByVal DriverName As String, ByVal DriverPath As String) As Boolean
        Dim hSrvMng As ULong = 0UL
        Dim hSrvDDK As ULong = 0UL
        Dim bret As ULong
        hSrvMng = OpenSCManager(vbNullString, vbNullString, &HF003FUL)
        If hSrvMng = 0UL Then
            DbgTextPut("Open services manager faild! Error code:" + CStr(GetLastError()), 5UI)
            Return False
        Else
            DbgTextPut("Open services manager successful!", 4UI)
        End If
        hSrvDDK = CreateService(hSrvMng, DriverName, DriverName, &HF003FUL, &H1UL, &H3UL, &H1UL, DriverPath, Chr(0), 0UL, Chr(0), 0UL, 0UL)
        If hSrvDDK = 0UL Then
            DbgTextPut("Create sevice faild! Error code:" + CStr(GetLastError()), 5UI)
            If CloseServiceHandle(hSrvMng) = 0UL Then
                DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            Return False
        Else
            DbgTextPut("Create service successful!", 4UI)
        End If
        hSrvDDK = OpenService(hSrvMng, DriverName, &HF003FUL)
        If hSrvDDK = 0UL Then
            DbgTextPut("Open service faild! Error code:" + CStr(GetLastError()), 5UI)
            If CloseServiceHandle(hSrvMng) = 0UL Then
                DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            Return False
        Else
            DbgTextPut("Open service successful!", 4UI)
        End If
        bret = StartServiceA(hSrvDDK, 0UL, 0UL)
        If bret = 0UL Then
            Dim lrtn As ULong
            lrtn = GetLastError()
            If lrtn <> &H997UL And lrtn <> &H1056UL Then
                DbgTextPut("Start service faild! Error code:" + CStr(lrtn), 5UI)
                If CloseServiceHandle(hSrvMng) = 0UL Then
                    DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
                End If
                If CloseServiceHandle(hSrvDDK) = 0UL Then
                    DbgTextPut("Close service faild! Error code:" + CStr(GetLastError()), 5UI)
                End If
                Return False
            Else
                If bret = &H997UL Then
                    DbgTextPut("Start service faild! Because ERROR_IO_PENDING", 5UI)
                    If CloseServiceHandle(hSrvMng) = 0UL Then
                        DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
                    End If
                    If CloseServiceHandle(hSrvDDK) = 0UL Then
                        DbgTextPut("Close service faild! Error code:" + CStr(GetLastError()), 5UI)
                    End If
                    Return False
                Else
                    DbgTextPut("Start service faild! Because ERROR_SERVICE_ALREADY_RUNNING", 5UI)
                    If CloseServiceHandle(hSrvMng) = 0UL Then
                        DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
                    End If
                    If CloseServiceHandle(hSrvDDK) = 0UL Then
                        DbgTextPut("Close service faild! Error code:" + CStr(GetLastError()), 5UI)
                    End If
                    Return True
                End If
            End If
        Else
            DbgTextPut("Start service successful!", 4UI)
        End If
        If CloseServiceHandle(hSrvMng) = 0UL Then
            DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
        End If
        If CloseServiceHandle(hSrvDDK) = 0UL Then
            DbgTextPut("Close service faild! Error code:" + CStr(GetLastError()), 5UI)
        End If
        Return True
    End Function
    Public Function UnloadNTDriver(ByVal DriverName As String) As Boolean
        Dim hSrvMng As ULong = 0UL
        Dim hSrvDDK As ULong = 0UL
        Dim Svrsta As SERVICE_STATUS
        hSrvMng = OpenSCManager(vbNullString, vbNullString, &HF003FUL)
        If hSrvMng = 0UL Then
            DbgTextPut("Open services manager faild! Error code:" + CStr(GetLastError()), 5UI)
            Return False
        Else
            DbgTextPut("Open services manager successful!", 4UI)
        End If
        hSrvDDK = OpenService(hSrvMng, DriverName, &HF003FUL)
        If hSrvDDK = 0UL Then
            DbgTextPut("Open service faild! Error code:" + CStr(GetLastError()), 5UI)
            If CloseServiceHandle(hSrvMng) = 0UL Then
                DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            Return False
        Else
            DbgTextPut("Open service successful!", 4UI)
        End If
        If ControlService(hSrvDDK, &H1UL, Svrsta) = 0UL Then
            DbgTextPut("Stop service faild! Error code:" + CStr(GetLastError()), 5UI)
            If CloseServiceHandle(hSrvMng) = 0UL Then
                DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            If CloseServiceHandle(hSrvDDK) = 0UL Then
                DbgTextPut("Close service faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            Return False
        Else
            DbgTextPut("Stop service successful!", 4UI)
        End If
        If DeleteService(hSrvDDK) = 0UL Then
            DbgTextPut("Delete service faild! Error code:" + CStr(GetLastError()), 5UI)
            If CloseServiceHandle(hSrvMng) = 0UL Then
                DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            If CloseServiceHandle(hSrvDDK) = 0UL Then
                DbgTextPut("Close service faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            Return False
        Else
            DbgTextPut("Delete service successful!", 4UI)
        End If
        If CloseServiceHandle(hSrvMng) = 0UL Then
            DbgTextPut("Close services manager faild! Error code:" + CStr(GetLastError()), 5UI)
        End If
        If CloseServiceHandle(hSrvDDK) = 0UL Then
            DbgTextPut("Close service faild! Error code:" + CStr(GetLastError()), 5UI)
        End If
        Return True
    End Function
    Public Function DbgTextPut(ByVal info As String, ByVal opt As UInteger) As Boolean
        Dim times As String, infos As String
        If opt = 1UI Then
            infos = "[INFO]LTFS Manager: "
        ElseIf opt = 2UI Then
            infos = "[WARN]LTFS Manager: "
        ElseIf opt = 3UI Then
            infos = "[ERRO]LTFS Manager: "
        ElseIf opt = 4UI Then
            infos = "[SUCC]LTFS Manager: "
        Else
            infos = "[FAIL]LTFS Manager: "
        End If
        times = Format(Now(), "[yyyy/MM/dd hh:mm:ss]")
        DbgText.Text = DbgText.Text + times + infos + info + vbCrLf
        Return True
    End Function

    Private Sub DriverUnload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DriverUnload.Click
        Message.ShowMessage(6UI, "准备停止服务..." + vbCrLf + vbCrLf + "这可能需要一些时间...", 1UI)
        Application.DoEvents()
        Sleep(100)
        Message.SetDialogInfo(8UI, "正在停止服务..." + vbCrLf + vbCrLf + "这可能需要一些时间...", 1UI)
        DriverStatus.Text = "驱动状态: 驱动正停止"
        Application.DoEvents()
        If RemoveMountPoint("W:") = 0 Then
            DbgTextPut("Remove volume mountpoint faild! Error code:" + CStr(GetLastError()), 4UL)
        Else
            DbgTextPut("Remove volume mountpoint successful!", 4UL)
        End If
        If UnloadNTDriver("LTFSTapeVolumeDriver") = True Then
            DriverLoad.Enabled = True
            DriverUnload.Enabled = False
            isLTOLOCATE.Enabled = False
            isLTOReadOnly.Enabled = False
            SelectDiskDrives.Enabled = False
            SelectTapeDrives.Enabled = False
            MountButton.Enabled = False
            UnmountButton.Enabled = False
            isserviceopened = True
            Message.SetDialogInfo(7UI, "启动停止成功!" + vbCrLf + vbCrLf + "对话框自动关闭", 1UI)
            DriverStatus.Text = "驱动状态: 驱动未运行"
            Application.DoEvents()
            Sleep(100)
            Message.Hide()
            Exit Sub
        End If
        Message.SetDialogInfo(1UI, "停止服务失败!" + vbCrLf + vbCrLf + "具体参见调试讯息!", 2UI)
        DriverStatus.Text = "驱动状态: 驱动正运行"
    End Sub
    Public Function LTFSAddVolume(ByVal tapenum As UInteger, ByVal isreadonly As Boolean) As Boolean
        Dim stemp As SECURITY_ATTRIBUTES
        Dim ltfscreateinfo As LTFS_VOLUME_CREATE
        Dim nbytereturn As ULong, nreturn As ULong
        Dim pltfscreateinfo As IntPtr, pnreturn As IntPtr
        hTape = CreateFile("\\.\TAPE" + CStr(tapenum), (GENERIC_READ Or GENERIC_WRITE), 0UL, stemp, &H3UL, 0UL, 0UL)
        If hTape = INVALID_HANDLE_VALUE Then
            DbgTextPut("Tape drive \\.\TAPE" + CStr(tapenum) + " open faild! Error code:" + CStr(GetLastError()), 5UI)
            Return False
        Else
            DbgTextPut("Tape drive \\.\TAPE" + CStr(tapenum) + " opened successful!", 4UI)
        End If
        hControl = CreateFile("\\.\LtfsControl", (GENERIC_READ Or GENERIC_WRITE), 0UL, stemp, &H3UL, 0UL, 0UL)
        If hControl = INVALID_HANDLE_VALUE Then
            DbgTextPut("LTFS Control \\.\LtfsControl open faild! Error code:" + CStr(GetLastError()), 5UI)
            If CloseHandle(hTape) = 0UL Then
                DbgTextPut("Tape drive \\.\TAPE" + CStr(tapenum) + " close faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            Return False
        Else
            DbgTextPut("LTFS Control \\.\LtfsControl opened successful!", 4UI)
        End If
        ltfscreateinfo.isreadonly = 0UI
        ltfscreateinfo.tapenum = tapenum
        If isreadonly Then
            ltfscreateinfo.isreadonly = 1UI
        End If
        pltfscreateinfo = Marshal.AllocHGlobal(Marshal.SizeOf(ltfscreateinfo))
        Marshal.StructureToPtr(ltfscreateinfo, pltfscreateinfo, True)
        pnreturn = Marshal.AllocHGlobal(Marshal.SizeOf(nreturn))
        If DeviceIoControl(hControl, IOCTL_LTFSVOLUME_CREATE_VOLUME, pltfscreateinfo, CULng(Marshal.SizeOf(pltfscreateinfo)), 0&, 0, nbytereturn, 0&) = 0UL Then
            DbgTextPut("Send create volume command to ltfs control faild! Error code:" + CStr(GetLastError()), 5UI)
            If CloseHandle(hControl) = 0UL Then
                DbgTextPut("LTFS Control \\.\LtfsControl close faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            If CloseHandle(hTape) = 0UL Then
                DbgTextPut("Tape drive \\.\TAPE" + CStr(tapenum) + " close faild! Error code:" + CStr(GetLastError()), 5UI)
            End If
            Marshal.FreeHGlobal(pltfscreateinfo)
            Marshal.FreeHGlobal(pnreturn)
            Return False
        Else
            DbgTextPut("Send create volume command to ltfs control successful!", 4UL)
        End If
        If AddMountPoint("W:", "\\.\LtfsVolume0") = 0 Then
            DbgTextPut("Create volume mountpoint faild! Error code:" + CStr(GetLastError()), 4UL)
        Else
            DbgTextPut("Create volume mountpoint successful!", 4UL)
        End If
        Return True
    End Function
    Public Function TapeDriveSearch() As UInteger
        Dim tapenum As UInteger = 0UI
        Dim tapeh As ULong
        Dim temp As SECURITY_ATTRIBUTES
        Do
            tapeh = CreateFile("\\.\TAPE" + CStr(tapenum), GENERIC_READ, 0UL, temp, &H3UL, 0UL, 0UL)
            If tapeh = INVALID_HANDLE_VALUE Then
                CloseHandle(tapeh)
                Exit Do
            End If
            CloseHandle(tapeh)
            tapenum = tapenum + 1UI
        Loop
        Return tapenum
    End Function
    Private Sub MountButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MountButton.Click
        LTFSAddVolume(CUInt(SelectTapeDrives.SelectedIndex), False)
    End Sub
    'e.g. MountPoint="K:" DeviceName="\Device\LTFSVolume0"
    Public Function AddMountPoint(ByVal MountPoint As String, ByVal DeviceName As String) As ULong
        If DefineDosDevice(&H1UL, "Global\" + MountPoint, DeviceName) <> 0 Then
            Return 1UL
        Else
            Return DefineDosDevice(&H1UL, MountPoint, DeviceName)
        End If
    End Function
    Public Function RemoveMountPoint(ByVal MountPoint As String) As ULong
        Return DefineDosDevice(&H2UL, "Global\" + MountPoint, Chr(0)) Or DefineDosDevice(&H2UL, MountPoint, Chr(0))
    End Function
End Class
