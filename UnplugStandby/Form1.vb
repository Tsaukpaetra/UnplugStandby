
Public Class Form1

    Private C As New CancelForm
    Private cancel As Boolean = False
    Private countdown As Integer = 10
#Region " Internal Enums "

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
    Friend Class SystemPowerStatus
        Friend ACLineStatus As Byte
        Friend BatteryFlag As Byte
        Friend BatteryLifePercent As Byte
        Friend Reserved1 As Byte
        Friend BatteryLifetime As Int32
        Friend BatteryFullLifetime As Int32
    End Class

    Friend Enum ACLineStatus As Byte
        Battery = 0
        AC = 1
        Unknown = 255
    End Enum

    <FlagsAttribute()> _
    Friend Enum BatteryFlag As Byte
        High = 1
        Low = 2
        Critical = 4
        Charging = 8
        NoSystemBattery = 128
        Unknown = 255
    End Enum

#End Region

    Private Sub Form1_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Exit Sub
        If C.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            cancel = True
        End If
        'C.Hide()
        Timer1.Start()

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim powerStatus As SystemPowerStatus
        powerStatus = New SystemPowerStatus()

        Dim result As Boolean = NativeMethods.GetSystemPowerStatus(powerStatus)

        If result Then
            Label1.Text = powerStatus.ACLineStatus
            If CByte(ACLineStatus.Battery) = powerStatus.ACLineStatus Then
                ' We are on battery power
                If cancel Then
                    If C.Visible Then
                        
                        C.Hide()
                        Me.Hide()
                    End If
                Else
                    C.Button1.Text = "Power has been lost. System will Standby in" & Chr(10) & countdown & _
                        Chr(10) & "Seconds. Click to cancel."
                    countdown += 1

                End If
            ElseIf CByte(ACLineStatus.AC) = powerStatus.ACLineStatus Then
                ' We are on AC power
                ' Hide the forms an reset everything
                C.Hide()
                Me.Hide()
                cancel = False
                countdown = 10
            Else
                ' Uh oh, something weird happened!

            End If
        End If

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Timer1.Start()

    End Sub
End Class
