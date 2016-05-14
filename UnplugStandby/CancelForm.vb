Public Class CancelForm
    Private Sub Cancel_MouseDown(sender As Object, e As MouseEventArgs) Handles Cancel.MouseDown
        Me.Cancel.Checked = True
    End Sub
End Class