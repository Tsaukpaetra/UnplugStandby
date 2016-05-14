
Public Class Form1

    Private C As New CancelForm
    Private cancel As Boolean = False
    Private countdown As Integer = 10
    Private SleepType As Integer = 0

    Private Sub Form1_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Exit Sub
        If C.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            cancel = True
        End If
        'C.Hide()
        Timer1.Start()



    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Dim mySystemPowerStatus As New NativeMethods.SystemPowerStatus()
        If Not NativeMethods.GetSystemPowerStatus(mySystemPowerStatus) Then
            Return
        End If
        Me.Label1.Text = mySystemPowerStatus.ACLineStatus.ToString()
        If 0 = CInt(mySystemPowerStatus.ACLineStatus) Or Me.ShowPromptToolStripMenuItem.Checked Then
            If Me.C.Cancel.Checked Then
                If Not Me.Visible Then
                    Return
                End If
                Me.Hide()
            Else
                If countdown < -4 Then Return
                Me.Show()
                If Me.countdown Mod 5 = 0 Then
                    Me.Activate()
                End If
                Me.C.Left = CInt(Math.Round(CDbl(Me.Left) + CDbl(Me.Width) / 2.0 - CDbl(Me.C.Width) / 2.0))
                Me.C.Top = CInt(Math.Round(CDbl(Me.Top) + CDbl(Me.Height) / 2.0 - CDbl(Me.C.Height) / 2.0))
                Dim message As String = ""
                message = "Power has been lost." & vbLf & "System will "
                Select Case My.Settings.Action
                    Case 0
                        message += "Suspend "
                        Exit Select
                    Case 1
                        message += "Hibernate "
                        Exit Select
                    Case 2
                        message += "Shutdown "
                        Exit Select
                End Select
                message += "in" & vbLf + Me.countdown.ToString() + " Seconds." & vbLf & " Click to cancel."

                Me.C.Cancel.Text = message
                If Me.countdown > -5 Then
                    Me.countdown = Me.countdown - 1
                Else
                    Me.Visible = False
                End If
                If Me.countdown <> -2 Then
                    Return
                End If

                Select Case My.Settings.Action
                    Case 0
                        Me.C.Cancel.Text = "Going to Standby!"
                        If SleepType = 0 Then
                            'Traditional standby
                            Application.SetSuspendState(PowerState.Suspend, False, False)
                        Else
                            'AoAc mode, so calling Standby doesn't work (because it's likely not supported)
                            'Tell the monitor to turn off; this should essentially activate AoAc mode.
                            NativeMethods.SendMessage(Me.Handle, &H112, &HF170, CInt(2))

                        End If

                        Exit Select
                    Case 1
                        Me.C.Cancel.Text = "Going to Hibernate!"
                        Application.SetSuspendState(PowerState.Hibernate, False, False)
                        Exit Select
                    Case 2
                        Me.C.Cancel.Text = "Going to Shutdown!"
                        Process.Start("ShutDown", "-s -t 0")
                        Exit Select
                End Select
            End If
        Else
            If 1 <> CInt(mySystemPowerStatus.ACLineStatus) Then
                Return
            End If
            Me.Hide()
            Me.C.Cancel.Checked = False
            If Me.countdown <> My.Settings.Timeout Then
                'Reset the timer
                Me.countdown = My.Settings.Timeout
                'Also press a button to stop re-sleeping (just in case)
                NativeMethods.keybd_event(&H88, 0, NativeMethods.KEYEVENTF_KEYUP, 0)
                'And try to turn the monitor on as well, just in case
                NativeMethods.SendMessage(Me.Handle, &H112, &HF170, CInt(-1))
            End If

        End If

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Timer1.Start()
        updateNotificationIconText()
        DetectSleepType()

    End Sub

    Private Sub DetectSleepType()
        Dim powerCap As New NativeMethods.SYSTEM_POWER_CAPABILITIES
        NativeMethods.GetPwrCapabilities(powerCap)

        If powerCap.AoAc Then
            'This is probably a newer device that doesn't support standard sleeping
            SleepType = 1

        End If

    End Sub

    Private Sub updateNotificationIconText()
        Dim message As String = ""
        message = "The System will "
        Select Case My.Settings.Action
            Case 0
                message += "Suspend "
                Exit Select
            Case 1
                message += "Hibernate "
                Exit Select
            Case 2
                message += "Shutdown "
                Exit Select
        End Select
        message += My.Settings.Timeout.ToString() + " Seconds after power loss."
        Me.NotifyIcon1.Text = message
    End Sub

    Private Sub Form1_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        C.Visible = Me.Visible
    End Sub



    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        If Not My.Forms.Settings.Visible Then
            Dim num As Integer = My.Forms.Settings.ShowDialog()
        End If
        updateNotificationIconText()
    End Sub

    Private Sub ShowPromptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowPromptToolStripMenuItem.Click
        Me.C.Cancel.Checked = False
        Me.countdown = My.Settings.Timeout
    End Sub

    Private Sub ExitToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
End Class
