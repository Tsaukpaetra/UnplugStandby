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

#Region " Internal Enums "

    Public Const KEYEVENTF_KEYUP As UInteger = &H2

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)>
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

    <FlagsAttribute()>
    Friend Enum BatteryFlag As Byte
        High = 1
        Low = 2
        Critical = 4
        Charging = 8
        NoSystemBattery = 128
        Unknown = 255
    End Enum



    Structure SYSTEM_POWER_CAPABILITIES
        <MarshalAs(UnmanagedType.U1)>
        Public PowerButtonPresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public SleepButtonPresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public LidPresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public SystemS1 As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public SystemS2 As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public SystemS3 As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public SystemS4 As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public SystemS5 As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public HiberFilePresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public FullWake As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public VideoDimPresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public ApmPresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public UpsPresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public ThermalControl As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public ProcessorThrottle As Boolean
        Public ProcessorMinThrottle As Byte
        Public ProcessorMaxThrottle As Byte
        ' Also known as ProcessorThrottleScale before Windows XP
        <MarshalAs(UnmanagedType.U1)>
        Public FastSystemS4 As Boolean
        ' Ignore if earlier than Windows XP
        <MarshalAs(UnmanagedType.U1)>
        Public Hiberboot As Boolean
        ' Ignore if earlier than Windows XP
        <MarshalAs(UnmanagedType.U1)>
        Public WakeAlarmPresent As Boolean
        ' Ignore if earlier than Windows XP
        <MarshalAs(UnmanagedType.U1)>
        Public AoAc As Boolean
        ' Ignore if earlier than Windows XP
        <MarshalAs(UnmanagedType.U1)>
        Public DiskSpinDown As Boolean
        Public HiberFileType As Byte
        ' Ignore if earlier than Windows 10 (10.0.10240.0)
        <MarshalAs(UnmanagedType.U1)>
        Public AoAcConnectivitySupported As Boolean
        ' Ignore if earlier than Windows 10 (10.0.10240.0)
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=6)>
        Private ReadOnly spare3 As Byte()
        <MarshalAs(UnmanagedType.U1)>
        Public SystemBatteriesPresent As Boolean
        <MarshalAs(UnmanagedType.U1)>
        Public BatteriesAreShortTerm As Boolean
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)>
        Public BatteryScale As BATTERY_REPORTING_SCALE()
        Public AcOnLineWake As SYSTEM_POWER_STATE
        Public SoftLidWake As SYSTEM_POWER_STATE
        Public RtcWake As SYSTEM_POWER_STATE
        Public MinDeviceWakeState As SYSTEM_POWER_STATE
        Public DefaultLowLatencyWake As SYSTEM_POWER_STATE
    End Structure

    Structure BATTERY_REPORTING_SCALE
        Public Granularity As ULong
        Public Capacity As ULong
    End Structure
    Enum SYSTEM_POWER_STATE
        PowerSystemUnspecified = 0
        PowerSystemWorking = 1
        PowerSystemSleeping1 = 2
        PowerSystemSleeping2 = 3
        PowerSystemSleeping3 = 4
        PowerSystemHibernate = 5
        PowerSystemShutdown = 6
        PowerSystemMaximum = 7
    End Enum


#End Region

    <DllImport("Kernel32.DLL", CharSet:=CharSet.Auto, SetLastError:=True)>
    Friend Shared Function GetSystemPowerStatus(ByVal mySystemPowerStatus As NativeMethods.SystemPowerStatus) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("powrprof.dll", SetLastError:=True)>
    Friend Shared Function GetPwrCapabilities(ByRef systemPowerCapabilities As SYSTEM_POWER_CAPABILITIES) As <MarshalAs(UnmanagedType.U1)> Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Friend Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Friend Shared Sub keybd_event(bVk As Byte, bScan As Byte, dwFlags As UInteger, dwExtraInfo As UIntPtr)
    End Sub
End Class
