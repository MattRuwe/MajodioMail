Imports System.Threading
Imports System.IO

Public Class ExceptionClient

    Private Shared _instanceLock As New Object
    Private Shared _instance As ExceptionClient = Nothing
    Public Shared Function Instance() As ExceptionClient
        If _instance Is Nothing Then
            Monitor.Enter(_instanceLock)
            If _instance Is Nothing Then
                _instance = New ExceptionClient
            End If
        End If
        Return _instance
    End Function

    Private Sub New()

    End Sub

    Public Sub SaveAndThrow(ByVal Ex As Exception)
        Save(Ex)
        Throw Ex
    End Sub

    Public Sub Save(ByVal Ex As Exception)
        Dim currentException As Exception
        Dim SW As StreamWriter = Nothing
        Dim exceptionCount As Integer = 0
        Try
            If Not Directory.Exists(GetApplicationDirectory() & "\" & ERROR_FILE_DIRECTORY) Then
                Directory.CreateDirectory(GetApplicationDirectory() & "\" & ERROR_FILE_DIRECTORY)
            End If
            SW = New StreamWriter(GetApplicationDirectory() & "\" & ERROR_FILE_DIRECTORY & "\" & GetSerializedDateTime() & Guid.NewGuid.ToString() & ".txt")

            currentException = Ex

            SW.WriteLine("Date/Time: " & DateTime.Now.ToString("yyyy/MMM/dd HH:mm:ss"))
            While Not IsNothing(currentException)
                exceptionCount += 1
                SW.WriteLine("----Exception Number " & exceptionCount & "----")
                SW.WriteLine("Message:     " & vbCrLf & currentException.Message)
                SW.WriteLine("Source:      " & vbCrLf & currentException.Source)
                SW.WriteLine("Stack Trace: " & currentException.StackTrace)

                If currentException.GetType Is GetType(MailException) Then
                    SW.WriteLine("Method Name: " + CType(currentException, MailException).MethodName)
                    If CType(currentException, MailException).ExceptionItems.Count > 0 Then
                        SW.WriteLine(vbCrLf & "Exception Items:")
                        For i As Integer = 0 To CType(currentException, MailException).ExceptionItems.Count - 1
                            SW.WriteLine("Category: " & CType(currentException, MailException).ExceptionItems(i).Category)
                            SW.WriteLine("Name:     " & CType(currentException, MailException).ExceptionItems(i).Name)
                            SW.WriteLine("Value:    " & CType(currentException, MailException).ExceptionItems(i).Value)
                        Next
                    End If
                End If
                SW.WriteLine()

                currentException = currentException.InnerException
            End While
        Catch

        Finally
            If Not IsNothing(SW) Then
                SW.Close()
                SW = Nothing
            End If
        End Try
    End Sub

End Class
