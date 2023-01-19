Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Reflection
Imports System.Runtime.Serialization

Namespace Majodio.Data

    Public MustInherit Class DataAccess

        Protected MustOverride Function GetConnection() As DbConnection
        Public MustOverride ReadOnly Property GetDbType() As DbType

        Protected Overridable Function GetDataAdapter(ByVal Command As DbCommand) As DbDataAdapter
            If GetDbType = DbType.SqlServer Then
                Dim RVal As SqlDataAdapter
                RVal = New SqlDataAdapter(Command)
                Return RVal
            Else
                Throw New InvalidOperationException("The database type " & GetDbType.ToString() & " is not recognized")
            End If
        End Function

        Public Function CreateNewParameter(ByVal name As String, ByVal value As Object) As DbParameter
            If GetDbType = DbType.SqlServer Then
                Return New SqlParameter(name, value)
            Else
                Throw New InvalidOperationException("The database type " & GetDbType.ToString & " is not recognized")
            End If
        End Function

        Public Function CreateNewParameterCollection() As DbParameterCollection
            Dim rVal As DbParameterCollection
            If GetDbType = DbType.SqlServer Then
                'Create a new instance of the SqlParameterCollection object
                'The constructor's access level is set to Friend so we have to
                'use reflection to instatiate a new instance
                rVal = FormatterServices.GetUninitializedObject(GetType(SqlParameterCollection))
                Dim cInfo As ConstructorInfo = GetType(SqlParameterCollection).GetConstructor(BindingFlags.CreateInstance Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Type() {}, Nothing)
                cInfo.Invoke(rVal, Nothing)
            Else
                Throw New InvalidOperationException("Unrecognized database type: " & GetDbType.ToString)
            End If
            Return rVal
        End Function


        Public Sub CommitTransaction(ByRef transaction As DbTransaction)
            If IsNothing(transaction) Then
                Throw New ArgumentNullException("transaction")
            End If
            Try
                If Not IsNothing(transaction.Connection) AndAlso transaction.Connection.State = ConnectionState.Open Then
                    transaction.Commit()
                End If
            Finally
                If Not IsNothing(transaction) AndAlso Not IsNothing(transaction.Connection) Then
                    transaction.Connection.Dispose()
                End If
                If Not IsNothing(transaction) Then
                    transaction.Dispose()
                    transaction = Nothing
                End If
            End Try
        End Sub

        Public Sub RollbackTransaction(ByRef transaction As DbTransaction)
            If IsNothing(transaction) Then
                Throw New ArgumentNullException("transaction")
            End If
            Try
                If Not IsNothing(transaction.Connection) AndAlso transaction.Connection.State = ConnectionState.Open Then
                    transaction.Rollback()
                End If
            Finally
                If Not IsNothing(transaction) Then
                    transaction.Dispose()
                    transaction = Nothing
                End If
            End Try
        End Sub

        Public Function GetDataTable(ByVal sql As String) As DataTable
            Try
                Return GetDataTable(sql, Nothing)
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Function GetDataTable(ByVal sql As String, ByVal parameters As DbParameterCollection) As DataTable
            Dim transaction As DbTransaction = Nothing
            Try
                Return GetDataTable(sql, parameters, transaction)
            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(transaction) Then
                    CommitTransaction(transaction)
                End If
            End Try
        End Function

        Public Function GetDataTable(ByVal Sql As String, ByVal parameters As DbParameterCollection, ByRef transaction As DbTransaction) As DataTable
            Dim connection As DbConnection = Nothing
            Dim Command As DbCommand = Nothing
            Dim tmpParameter As DbParameter
            Try
                If IsNothing(transaction) Then
                    connection = GetConnection()
                    transaction = connection.BeginTransaction
                Else
                    connection = transaction.Connection
                End If
                Command = connection.CreateCommand
                Command.Transaction = transaction
                Command.CommandType = CommandType.Text
                Command.CommandText = Sql
                If Not IsNothing(parameters) Then
                    For i As Integer = 0 To parameters.Count - 1
                        tmpParameter = parameters(0)
                        parameters.RemoveAt(0)
                        Command.Parameters.Add(tmpParameter)
                    Next
                End If

                Return GetDataTable(Command)
            Catch E As System.Exception
                Throw New Majodio.Data.DataException(Sql, E)
            Finally

            End Try
        End Function

        Private Function GetDataTable(ByRef command As DbCommand) As DataTable
            Dim rVal As DataTable = Nothing
            Dim dataAdapter As DbDataAdapter = Nothing
            Dim dataSet As New DataSet

            If IsNothing(command) Then
                Throw New ArgumentNullException("command")
            End If
            Try
                If IsNothing(command.Connection) OrElse command.Connection.State <> ConnectionState.Open Then
                    command.Connection = GetConnection()
                End If
                dataAdapter = GetDataAdapter(command)
                dataAdapter.Fill(dataSet)
                If Not IsNothing(dataSet) AndAlso dataSet.Tables.Count > 0 Then
                    rVal = dataSet.Tables(0)
                End If

                Return rVal
            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(dataAdapter) Then
                    dataAdapter.Dispose()
                    dataAdapter = Nothing
                End If
            End Try
        End Function

        Public Overridable Function ExecuteScalar(ByVal Sql As String) As Object
            Dim RVal As Object = Nothing
            Dim Command As DbCommand = Nothing
            Try
                Command = GetConnection.CreateCommand()
                Command.CommandType = CommandType.Text
                Command.CommandText = Sql
                RVal = Command.ExecuteScalar()
            Catch E As System.Exception
                Throw New Majodio.Data.DataException(Sql, E)
            Finally
                If Not IsNothing(Command.Connection) Then
                    Command.Connection.Close()
                    Command.Connection.Dispose()
                    Command.Connection = Nothing
                End If
                If Not IsNothing(Command) Then
                    Command.Dispose()
                    Command = Nothing
                End If
            End Try
            Return RVal
        End Function

        Public Overridable Function ExecuteScalar(ByVal sql As String, ByVal parameters As DbParameterCollection) As Object
            Dim transaction As DbTransaction = Nothing
            Try
                Return ExecuteScalar(sql, parameters, transaction)
            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(transaction) Then
                    CommitTransaction(transaction)
                End If
            End Try
        End Function

        Public Overridable Function ExecuteScalar(ByVal sql As String, ByVal parameters As DbParameterCollection, ByRef transaction As DbTransaction) As Object
            Dim command As DbCommand
            Dim connection As DbConnection
            Dim tmpparameter As DbParameter
            Try

                If IsNothing(transaction) Then
                    connection = GetConnection()
                    transaction = connection.BeginTransaction
                Else
                    connection = transaction.Connection
                End If
                command = GetConnection.CreateCommand()
                command.CommandType = CommandType.Text
                command.CommandText = sql
                If Not IsNothing(parameters) Then
                    For i As Integer = 0 To parameters.Count - 1
                        tmpParameter = parameters(0)
                        parameters.RemoveAt(0)
                        command.Parameters.Add(tmpParameter)
                    Next
                End If

                Return ExecuteScalar(command)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Protected Overridable Function ExecuteScalar(ByVal command As DbCommand) As Object
            Dim rVal As Object = Nothing
            If IsNothing(command) Then
                Throw New ArgumentNullException("command")
            End If
            Try
                If IsNothing(command.Connection) OrElse command.Connection.State <> ConnectionState.Open Then
                    command.Connection = GetConnection()
                End If
                rVal = command.ExecuteScalar()

                Return rVal
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Overridable Function ExecuteNonQuery(ByVal Sql As String) As Integer
            Try
                Return ExecuteNonQuery(Sql, Nothing)
            Catch E As System.Exception
                Throw New Majodio.Data.DataException(Sql, E)
            Finally

            End Try
        End Function

        Public Overridable Function ExecuteNonQuery(ByVal sql As String, ByVal parameters As DbParameterCollection) As Integer
            Dim transaction As DbTransaction = Nothing
            Try
                Return ExecuteNonQuery(sql, parameters, transaction)
            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(transaction) Then
                    CommitTransaction(transaction)
                End If
            End Try
        End Function

        Public Overridable Function ExecuteNonQuery(ByVal sql As String, ByVal parameters As DbParameterCollection, ByRef transaction As DbTransaction) As Long
            Dim command As DbCommand = Nothing
            Dim connection As DbConnection = Nothing
            Dim tmpParameter As SqlParameter
            Try
                If IsNothing(transaction) Then
                    connection = GetConnection()
                    transaction = connection.BeginTransaction
                Else
                    connection = transaction.Connection
                End If
                command = connection.CreateCommand
                command.Transaction = transaction
                command.CommandText = sql
                command.CommandTimeout = 200

                If Not IsNothing(parameters) Then
                    For i As Integer = 0 To parameters.Count - 1
                        tmpParameter = parameters(0)
                        parameters.RemoveAt(0)
                        command.Parameters.Add(tmpParameter)
                    Next
                End If

                Return ExecuteNonQuery(command)
            Catch ex As Exception
                If Not IsNothing(command) AndAlso Not IsNothing(transaction) Then
                    RollbackTransaction(transaction)
                End If
                Throw ex
            Finally

            End Try
        End Function

        Protected Overridable Function ExecuteNonQuery(ByRef command As DbCommand) As Integer
            Dim rVal As Integer = -1
            If IsNothing(command) Then
                Throw New ArgumentNullException("command")
            End If
            Try
                'Check if the connection in the command object is established and available
                If IsNothing(command.Connection) OrElse command.Connection.State <> ConnectionState.Open Then
                    command.Connection = GetConnection()
                End If
                If IsNothing(command.Transaction) Then
                    command.Transaction = command.Connection.BeginTransaction
                End If

                rVal = command.ExecuteNonQuery()

                Return rVal
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function
    End Class
End Namespace
