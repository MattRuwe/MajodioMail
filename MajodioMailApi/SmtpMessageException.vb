''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class SmtpMessageException
    Inherits System.Exception

    ''' <summary>
    ''' Creates a new instance of the exception with only a message
    ''' </summary>
    ''' <param name="message">A message indicating the reason for the exception</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' Cretea a new instance of the exception with a message an inner exception
    ''' </summary>
    ''' <param name="message">A message indicating the reason for the exception</param>
    ''' <param name="innerException">An instance of the exception that caused the current exception</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class
