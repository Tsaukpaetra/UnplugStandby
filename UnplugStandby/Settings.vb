Public Class Settings
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Actions.SelectedIndex = 0
        If My.Settings.Timeout > 4 Then Me.NumericUpDown1.Value = My.Settings.Timeout
        If My.Settings.Action <= -1 Then Return
        Actions.SelectedIndex = My.Settings.Action
    End Sub

    Private Sub Actions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Actions.SelectedIndexChanged
        My.Settings.Action = Actions.SelectedIndex
    End Sub
End Class