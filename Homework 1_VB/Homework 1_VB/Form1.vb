Public Class Form1
    Private Sub CheckBoxAccept_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAccept.CheckedChanged
        Me.ButtonSend.Enabled = Me.CheckBoxAccept.Checked
    End Sub
    Private Sub ButtonReset_Click(sender As Object, e As EventArgs) Handles ButtonReset.Click
        Me.RichTextBox1.Clear()
    End Sub
    Private Sub ButtonReset_MouseHover(sender As Object, e As EventArgs) Handles ButtonReset.MouseHover
        Me.ToolTip1.SetToolTip(ButtonReset, "Clears the above textbox")
    End Sub
    Private Sub ButtonSend_Click(sender As Object, e As EventArgs) Handles ButtonSend.Click
        Me.RichTextBox1.Enabled = False
        Me.CheckBoxAccept.Enabled = False
        Me.ButtonReset.Enabled = False
        Me.ButtonSend.Enabled = False
    End Sub
    Private Sub ButtonSend_MouseHover(sender As Object, e As EventArgs) Handles ButtonSend.MouseHover
        Me.ToolTip1.SetToolTip(ButtonSend, "Sends the form")
    End Sub
    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Me.Close()
    End Sub
    Private Sub ButtonClose_MouseHover(sender As Object, e As EventArgs) Handles ButtonClose.MouseHover
        Me.ToolTip1.SetToolTip(ButtonClose, "Closes the app")
    End Sub
End Class
