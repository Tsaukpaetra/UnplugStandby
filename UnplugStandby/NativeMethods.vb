Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Security.Permissions


Friend NotInheritable Class NativeMethods

#Region " Contructor "

    Private Sub New()
        ' Just for the compiler.
    End Sub

#End Region

    <DllImport("Kernel32.DLL", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Friend Shared Function GetSystemPowerStatus(ByVal mySystemPowerStatus As Form1.SystemPowerStatus) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

End Class
