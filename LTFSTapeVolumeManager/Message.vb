Public Class Message
    Dim asbutton As Integer
    Public pressbutton As Integer
    Public Function ShowMessage(ByVal showimage As Integer, ByVal showstring As String, ByVal showbutton As Integer) As Boolean
        pressbutton = 0
        Icons.Image = ImageList1.Images(showimage)
        Label1.Text = showstring
        asbutton = showbutton
        YesOKRetry.Visible = False
        NoCancleIgnore.Visible = False
        Abort.Visible = False
        If showbutton = 1 Then
            Me.Show(LTFSManage)
        ElseIf showbutton = 2 Then
            NoCancleIgnore.Visible = True
            NoCancleIgnore.Text = "确定"
            Me.Show(LTFSManage)
        ElseIf showbutton = 3 Then
            YesOKRetry.Visible = True
            NoCancleIgnore.Visible = True
            YesOKRetry.Text = "确定"
            NoCancleIgnore.Text = "取消"
            Me.Show(LTFSManage)
        ElseIf showbutton = 4 Then
            YesOKRetry.Visible = True
            NoCancleIgnore.Visible = True
            YesOKRetry.Text = "是"
            NoCancleIgnore.Text = "否"
            Me.Show(LTFSManage)
        ElseIf showbutton = 5 Then
            YesOKRetry.Visible = True
            NoCancleIgnore.Visible = True
            Abort.Visible = True
            YesOKRetry.Text = "重试"
            NoCancleIgnore.Text = "忽略"
            Abort.Text = "放弃'"
            Me.Show(LTFSManage)
        End If
        Return True
    End Function
    Public Function SetDialogInfo(ByVal showimage As Integer, ByVal showstring As String, ByVal showbutton As Integer) As Boolean
        pressbutton = 0
        Icons.Image = ImageList1.Images(showimage)
        Label1.Text = showstring
        asbutton = showbutton
        YesOKRetry.Visible = False
        NoCancleIgnore.Visible = False
        Abort.Visible = False
        If showbutton = 2 Then
            NoCancleIgnore.Visible = True
            NoCancleIgnore.Text = "确定"
        ElseIf showbutton = 3 Then
            YesOKRetry.Visible = True
            NoCancleIgnore.Visible = True
            YesOKRetry.Text = "确定"
            NoCancleIgnore.Text = "取消"
        ElseIf showbutton = 4 Then
            YesOKRetry.Visible = True
            NoCancleIgnore.Visible = True
            YesOKRetry.Text = "是"
            NoCancleIgnore.Text = "否"
        ElseIf showbutton = 5 Then
            YesOKRetry.Visible = True
            NoCancleIgnore.Visible = True
            Abort.Visible = True
            YesOKRetry.Text = "重试"
            NoCancleIgnore.Text = "忽略"
            Abort.Text = "放弃"
        End If
        Return True
    End Function

    Private Sub NoCancleIgnore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NoCancleIgnore.Click
        pressbutton = 1
       Select asbutton
            Case 2
                Me.Hide()
                Exit Sub
        End Select
    End Sub

    Private Sub YesOKRetry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YesOKRetry.Click
        pressbutton = 2
    End Sub

    Private Sub Abort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Abort.Click
        pressbutton = 3
    End Sub
End Class