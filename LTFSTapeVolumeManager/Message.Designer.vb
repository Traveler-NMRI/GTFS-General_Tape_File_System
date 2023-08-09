<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Message
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Message))
        Me.Icons = New System.Windows.Forms.PictureBox()
        Me.YesOKRetry = New System.Windows.Forms.Button()
        Me.NoCancleIgnore = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Abort = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.Icons, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Icons
        '
        Me.Icons.Image = Global.LTFSTapeVolumeManager.My.Resources.Resources.mountwait
        Me.Icons.Location = New System.Drawing.Point(12, 12)
        Me.Icons.Name = "Icons"
        Me.Icons.Size = New System.Drawing.Size(32, 32)
        Me.Icons.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Icons.TabIndex = 0
        Me.Icons.TabStop = False
        '
        'YesOKRetry
        '
        Me.YesOKRetry.Location = New System.Drawing.Point(179, 54)
        Me.YesOKRetry.Name = "YesOKRetry"
        Me.YesOKRetry.Size = New System.Drawing.Size(70, 20)
        Me.YesOKRetry.TabIndex = 2
        Me.YesOKRetry.Text = "重试"
        Me.YesOKRetry.UseVisualStyleBackColor = True
        '
        'NoCancleIgnore
        '
        Me.NoCancleIgnore.Location = New System.Drawing.Point(255, 54)
        Me.NoCancleIgnore.Name = "NoCancleIgnore"
        Me.NoCancleIgnore.Size = New System.Drawing.Size(70, 20)
        Me.NoCancleIgnore.TabIndex = 3
        Me.NoCancleIgnore.Text = "忽略"
        Me.NoCancleIgnore.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(50, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(131, 36)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "正在启动服务..." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "这可能需要一些时间..." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Abort
        '
        Me.Abort.Location = New System.Drawing.Point(103, 54)
        Me.Abort.Name = "Abort"
        Me.Abort.Size = New System.Drawing.Size(70, 20)
        Me.Abort.TabIndex = 5
        Me.Abort.Text = "终止"
        Me.Abort.UseVisualStyleBackColor = True
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
        'Message
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 86)
        Me.Controls.Add(Me.Abort)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.NoCancleIgnore)
        Me.Controls.Add(Me.YesOKRetry)
        Me.Controls.Add(Me.Icons)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Message"
        Me.Text = "Message"
        CType(Me.Icons, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Icons As System.Windows.Forms.PictureBox
    Friend WithEvents YesOKRetry As System.Windows.Forms.Button
    Friend WithEvents NoCancleIgnore As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Abort As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
