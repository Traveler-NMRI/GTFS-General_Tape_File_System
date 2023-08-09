<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LTFSManage
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LTFSManage))
        Me.SelectTapeDrives = New System.Windows.Forms.ComboBox()
        Me.DriverUnload = New System.Windows.Forms.Button()
        Me.DriverLoad = New System.Windows.Forms.Button()
        Me.DriverStatus = New System.Windows.Forms.Label()
        Me.ServiceStop = New System.Windows.Forms.Button()
        Me.ServiceStart = New System.Windows.Forms.Button()
        Me.ServiceStatus = New System.Windows.Forms.Label()
        Me.TapeDriveLable = New System.Windows.Forms.Label()
        Me.DiskLable = New System.Windows.Forms.Label()
        Me.SelectDiskDrives = New System.Windows.Forms.ComboBox()
        Me.MountButton = New System.Windows.Forms.Button()
        Me.UnmountButton = New System.Windows.Forms.Button()
        Me.isLTOLOCATE = New System.Windows.Forms.CheckBox()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.MountedTape = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MountedDisk = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MountedDiskStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MountedDiskmemo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.isLTOReadOnly = New System.Windows.Forms.CheckBox()
        Me.MountedLable = New System.Windows.Forms.Label()
        Me.DbgText = New System.Windows.Forms.RichTextBox()
        Me.DebugLable = New System.Windows.Forms.Label()
        Me.SendTapeInfoTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'SelectTapeDrives
        '
        Me.SelectTapeDrives.FormattingEnabled = True
        Me.SelectTapeDrives.Location = New System.Drawing.Point(183, 64)
        Me.SelectTapeDrives.Name = "SelectTapeDrives"
        Me.SelectTapeDrives.Size = New System.Drawing.Size(101, 20)
        Me.SelectTapeDrives.TabIndex = 7
        '
        'DriverUnload
        '
        Me.DriverUnload.Location = New System.Drawing.Point(290, 12)
        Me.DriverUnload.Name = "DriverUnload"
        Me.DriverUnload.Size = New System.Drawing.Size(70, 20)
        Me.DriverUnload.TabIndex = 2
        Me.DriverUnload.Text = "卸载驱动"
        Me.DriverUnload.UseVisualStyleBackColor = True
        '
        'DriverLoad
        '
        Me.DriverLoad.Location = New System.Drawing.Point(214, 12)
        Me.DriverLoad.Name = "DriverLoad"
        Me.DriverLoad.Size = New System.Drawing.Size(70, 20)
        Me.DriverLoad.TabIndex = 1
        Me.DriverLoad.Text = "加载驱动"
        Me.DriverLoad.UseVisualStyleBackColor = True
        '
        'DriverStatus
        '
        Me.DriverStatus.AutoSize = True
        Me.DriverStatus.Location = New System.Drawing.Point(12, 16)
        Me.DriverStatus.Name = "DriverStatus"
        Me.DriverStatus.Size = New System.Drawing.Size(125, 12)
        Me.DriverStatus.TabIndex = 0
        Me.DriverStatus.Text = "驱动状态: 驱动未运行"
        '
        'ServiceStop
        '
        Me.ServiceStop.Location = New System.Drawing.Point(290, 38)
        Me.ServiceStop.Name = "ServiceStop"
        Me.ServiceStop.Size = New System.Drawing.Size(70, 20)
        Me.ServiceStop.TabIndex = 5
        Me.ServiceStop.Text = "停止服务"
        Me.ServiceStop.UseVisualStyleBackColor = True
        '
        'ServiceStart
        '
        Me.ServiceStart.Location = New System.Drawing.Point(214, 38)
        Me.ServiceStart.Name = "ServiceStart"
        Me.ServiceStart.Size = New System.Drawing.Size(70, 20)
        Me.ServiceStart.TabIndex = 4
        Me.ServiceStart.Text = "启动服务"
        Me.ServiceStart.UseVisualStyleBackColor = True
        '
        'ServiceStatus
        '
        Me.ServiceStatus.AutoSize = True
        Me.ServiceStatus.Location = New System.Drawing.Point(12, 42)
        Me.ServiceStatus.Name = "ServiceStatus"
        Me.ServiceStatus.Size = New System.Drawing.Size(125, 12)
        Me.ServiceStatus.TabIndex = 3
        Me.ServiceStatus.Text = "服务状态: 服务未运行"
        '
        'TapeDriveLable
        '
        Me.TapeDriveLable.AutoSize = True
        Me.TapeDriveLable.Location = New System.Drawing.Point(12, 67)
        Me.TapeDriveLable.Name = "TapeDriveLable"
        Me.TapeDriveLable.Size = New System.Drawing.Size(173, 12)
        Me.TapeDriveLable.TabIndex = 6
        Me.TapeDriveLable.Text = "选择要挂载/卸除的磁带驱动器:"
        '
        'DiskLable
        '
        Me.DiskLable.AutoSize = True
        Me.DiskLable.Location = New System.Drawing.Point(12, 94)
        Me.DiskLable.Name = "DiskLable"
        Me.DiskLable.Size = New System.Drawing.Size(137, 12)
        Me.DiskLable.TabIndex = 9
        Me.DiskLable.Text = "选择要挂载/卸除的盘符:"
        '
        'SelectDiskDrives
        '
        Me.SelectDiskDrives.FormattingEnabled = True
        Me.SelectDiskDrives.Location = New System.Drawing.Point(155, 90)
        Me.SelectDiskDrives.Name = "SelectDiskDrives"
        Me.SelectDiskDrives.Size = New System.Drawing.Size(129, 20)
        Me.SelectDiskDrives.TabIndex = 10
        '
        'MountButton
        '
        Me.MountButton.Location = New System.Drawing.Point(290, 64)
        Me.MountButton.Name = "MountButton"
        Me.MountButton.Size = New System.Drawing.Size(70, 20)
        Me.MountButton.TabIndex = 8
        Me.MountButton.Text = "挂载"
        Me.MountButton.UseVisualStyleBackColor = True
        '
        'UnmountButton
        '
        Me.UnmountButton.Location = New System.Drawing.Point(290, 90)
        Me.UnmountButton.Name = "UnmountButton"
        Me.UnmountButton.Size = New System.Drawing.Size(70, 20)
        Me.UnmountButton.TabIndex = 11
        Me.UnmountButton.Text = "卸除"
        Me.UnmountButton.UseVisualStyleBackColor = True
        '
        'isLTOLOCATE
        '
        Me.isLTOLOCATE.AutoSize = True
        Me.isLTOLOCATE.Location = New System.Drawing.Point(14, 116)
        Me.isLTOLOCATE.Name = "isLTOLOCATE"
        Me.isLTOLOCATE.Size = New System.Drawing.Size(216, 16)
        Me.isLTOLOCATE.TabIndex = 12
        Me.isLTOLOCATE.Text = "兼容旧Locate指令(LTO3及以下必选)"
        Me.isLTOLOCATE.UseVisualStyleBackColor = True
        Me.isLTOLOCATE.Visible = False
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.MountedTape, Me.MountedDisk, Me.MountedDiskStatus, Me.MountedDiskmemo})
        Me.ListView1.Location = New System.Drawing.Point(12, 150)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(348, 94)
        Me.ListView1.StateImageList = Me.ImageList1
        Me.ListView1.TabIndex = 15
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'MountedTape
        '
        Me.MountedTape.Text = "磁带机"
        '
        'MountedDisk
        '
        Me.MountedDisk.Text = "挂载盘符"
        '
        'MountedDiskStatus
        '
        Me.MountedDiskStatus.Text = "状态"
        '
        'MountedDiskmemo
        '
        Me.MountedDiskmemo.Text = "备注"
        Me.MountedDiskmemo.Width = 164
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "CONTROL.ico")
        Me.ImageList1.Images.SetKeyName(1, "error.ico")
        Me.ImageList1.Images.SetKeyName(2, "exit.ico")
        Me.ImageList1.Images.SetKeyName(3, "FAILD.ico")
        Me.ImageList1.Images.SetKeyName(4, "filettape.ico")
        Me.ImageList1.Images.SetKeyName(5, "LTFS.ico")
        Me.ImageList1.Images.SetKeyName(6, "mountrunning.ico")
        Me.ImageList1.Images.SetKeyName(7, "mountstop.ico")
        Me.ImageList1.Images.SetKeyName(8, "mountwait.ico")
        Me.ImageList1.Images.SetKeyName(9, "no.ico")
        Me.ImageList1.Images.SetKeyName(10, "nowarningerr.ico")
        Me.ImageList1.Images.SetKeyName(11, "question.ico")
        Me.ImageList1.Images.SetKeyName(12, "SERVICES.ico")
        Me.ImageList1.Images.SetKeyName(13, "SUCCESS.ico")
        Me.ImageList1.Images.SetKeyName(14, "TAPE1.ico")
        Me.ImageList1.Images.SetKeyName(15, "TAPE2.ICO")
        Me.ImageList1.Images.SetKeyName(16, "TAPE3.ico")
        Me.ImageList1.Images.SetKeyName(17, "TAPEDrive.ico")
        Me.ImageList1.Images.SetKeyName(18, "TapeDriveConnect.ICO")
        Me.ImageList1.Images.SetKeyName(19, "TapeDriveNotConnect.ico")
        Me.ImageList1.Images.SetKeyName(20, "TAPEEJECT.ico")
        Me.ImageList1.Images.SetKeyName(21, "TAPEERROR.ico")
        Me.ImageList1.Images.SetKeyName(22, "TAPEOFFLINE.ico")
        Me.ImageList1.Images.SetKeyName(23, "tapetofile.ico")
        Me.ImageList1.Images.SetKeyName(24, "trustalert.ico")
        Me.ImageList1.Images.SetKeyName(25, "trusted.ico")
        Me.ImageList1.Images.SetKeyName(26, "wait.ico")
        Me.ImageList1.Images.SetKeyName(27, "warning.ico")
        Me.ImageList1.Images.SetKeyName(28, "warningerr.ico")
        '
        'isLTOReadOnly
        '
        Me.isLTOReadOnly.AutoSize = True
        Me.isLTOReadOnly.Location = New System.Drawing.Point(236, 116)
        Me.isLTOReadOnly.Name = "isLTOReadOnly"
        Me.isLTOReadOnly.Size = New System.Drawing.Size(108, 16)
        Me.isLTOReadOnly.TabIndex = 13
        Me.isLTOReadOnly.Text = "以只读方式挂载"
        Me.isLTOReadOnly.UseVisualStyleBackColor = True
        '
        'MountedLable
        '
        Me.MountedLable.AutoSize = True
        Me.MountedLable.Location = New System.Drawing.Point(12, 135)
        Me.MountedLable.Name = "MountedLable"
        Me.MountedLable.Size = New System.Drawing.Size(95, 12)
        Me.MountedLable.TabIndex = 14
        Me.MountedLable.Text = "已挂载的磁带机:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'DbgText
        '
        Me.DbgText.Location = New System.Drawing.Point(12, 262)
        Me.DbgText.Name = "DbgText"
        Me.DbgText.ReadOnly = True
        Me.DbgText.Size = New System.Drawing.Size(348, 97)
        Me.DbgText.TabIndex = 17
        Me.DbgText.Text = ""
        '
        'DebugLable
        '
        Me.DebugLable.AutoSize = True
        Me.DebugLable.Location = New System.Drawing.Point(10, 247)
        Me.DebugLable.Name = "DebugLable"
        Me.DebugLable.Size = New System.Drawing.Size(59, 12)
        Me.DebugLable.TabIndex = 16
        Me.DebugLable.Text = "调试讯息:"
        '
        'SendTapeInfoTimer
        '
        Me.SendTapeInfoTimer.Interval = 125
        '
        'LTFSManage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(372, 371)
        Me.Controls.Add(Me.DebugLable)
        Me.Controls.Add(Me.DbgText)
        Me.Controls.Add(Me.MountedLable)
        Me.Controls.Add(Me.isLTOReadOnly)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.isLTOLOCATE)
        Me.Controls.Add(Me.UnmountButton)
        Me.Controls.Add(Me.MountButton)
        Me.Controls.Add(Me.SelectDiskDrives)
        Me.Controls.Add(Me.DiskLable)
        Me.Controls.Add(Me.TapeDriveLable)
        Me.Controls.Add(Me.ServiceStatus)
        Me.Controls.Add(Me.ServiceStart)
        Me.Controls.Add(Me.ServiceStop)
        Me.Controls.Add(Me.DriverStatus)
        Me.Controls.Add(Me.DriverLoad)
        Me.Controls.Add(Me.DriverUnload)
        Me.Controls.Add(Me.SelectTapeDrives)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "LTFSManage"
        Me.Text = "LTFS Tape Volume Manager"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SelectTapeDrives As System.Windows.Forms.ComboBox
    Friend WithEvents DriverUnload As System.Windows.Forms.Button
    Friend WithEvents DriverLoad As System.Windows.Forms.Button
    Friend WithEvents DriverStatus As System.Windows.Forms.Label
    Friend WithEvents ServiceStop As System.Windows.Forms.Button
    Friend WithEvents ServiceStart As System.Windows.Forms.Button
    Friend WithEvents ServiceStatus As System.Windows.Forms.Label
    Friend WithEvents TapeDriveLable As System.Windows.Forms.Label
    Friend WithEvents DiskLable As System.Windows.Forms.Label
    Friend WithEvents SelectDiskDrives As System.Windows.Forms.ComboBox
    Friend WithEvents MountButton As System.Windows.Forms.Button
    Friend WithEvents UnmountButton As System.Windows.Forms.Button
    Friend WithEvents isLTOLOCATE As System.Windows.Forms.CheckBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents isLTOReadOnly As System.Windows.Forms.CheckBox
    Friend WithEvents MountedLable As System.Windows.Forms.Label
    Friend WithEvents MountedTape As System.Windows.Forms.ColumnHeader
    Friend WithEvents MountedDisk As System.Windows.Forms.ColumnHeader
    Friend WithEvents MountedDiskStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents MountedDiskmemo As System.Windows.Forms.ColumnHeader
    Friend WithEvents DbgText As System.Windows.Forms.RichTextBox
    Friend WithEvents DebugLable As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents SendTapeInfoTimer As System.Windows.Forms.Timer

End Class
