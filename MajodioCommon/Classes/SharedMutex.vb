'Imports System.Runtime.InteropServices

'Public Structure SECURITY_ATTRIBUTES
'    Public nLength As Integer
'    Public lpSecurityDescriptor As Integer
'    Public bInheritHandle As Boolean
'End Structure

'Public Class SharedMutex
'    Declare Function OpenMutex Lib "kernel32" Alias "OpenMutexA" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal lpName As String) As Integer 

'    Private Sub PrivateConstruct(ByVal initiallyOwned As Boolean, ByVal name As String, ByRef createdNew As Boolean)
'        ' It's faster to first try OpenMutex
'        Dim SYNCHRONIZE As Integer = &H100000
'        Dim hMutex As Integer = OpenMutex(SYNCHRONIZE, 1, name)
'        createdNew = False
'        If hMutex = 0 Then
'            Dim sa As SECURITY_ATTRIBUTES = New SECURITY_ATTRIBUTES
'            sa.nLength = System.Runtime.InteropServices.Marshal.SizeOf(sa)
'            sa.bInheritHandle = 1
'            sa.lpSecurityDescriptor = 0

'            Dim securityDescriptorSize As Integer = 0 'Dummy
'                Dim result as Integer = ConvertStringSecurityDescriptorToSecurityDescriptor("D:(A;NP;0x001f0001;;;WD)", _
'                    SDDL_REVISION_1, _
'                    sa.lpSecurityDescriptor, _
'                    securityDescriptorSize)
'                if (result == 0)
'                {
'                    throw new Exception("Failure while creating security
'descriptor for new mutex");
'                }

'                hMutex = CreateMutex(ref sa, initiallyOwned ? 1 : 0, ToSystemMutexName(name));
'                createdNew = (Marshal.GetLastWin32Error() != ERROR_ALREADY_EXISTS);
'                LocalFree(sa.lpSecurityDescriptor);

'                if (hMutex == 0)
'                {
'                    ' If we get here, some sort of unrecoverable error has presumably occurred.
'                    ' However, we will try one last time, in case it was merely some sort of race condition.
'                    ' Note that I do not believe that there is any opening for a race condition in the above code.
'                    ' Nevertheless, we have nothing to lose by giving it one last try.
'                    hMutex = OpenMutex(SYNCHRONIZE, 1, ToSystemMutexName(name));
'                    createdNew = false;
'                    if (hMutex == 0)
'                    {
'                        throw new Exception("Unable to create or open mutex.");
'                    }
'                }
'                    End If

'            Handle = (IntPtr)hMutex;
'    End Sub




'End Class
