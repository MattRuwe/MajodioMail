Imports Majodio.Mail.Common.Grouping

Namespace Imap4
    Public Class CommandParameterCollection
        Inherits CollectionBase

        Public Sub Add(ByVal Parameter As CommandParameter)
            MyBase.List.Add(Parameter)
        End Sub

        Public Sub Add(ByVal Parameters As String)
            Dim ArrParameters As String()
            Dim Parameter As String
            Dim CurrentIndex As Integer = 0
            Dim PreviousCount As Integer = Count
            'If Not IsNothing(Parameters) AndAlso Parameters.Trim.Length > 0 Then
            '    ArrParameters = Majodio.Mail.Common.MajodioQuoteSplit(Parameters, " ", True)
            '    For i As Integer = 0 To ArrParameters.GetUpperBound(0)
            '        Parameter = ArrParameters(i)
            '        Add(New Imap4CommandParameter(Parameter))
            '    Next
            'End If
            If Not IsNothing(Parameters) AndAlso Parameters.Trim.Length > 0 Then
                Dim EG As EmbeddedGroup
                If EmbeddedGroup.IsValidGroupString(Parameters) Then
                    EG = New EmbeddedGroup(Parameters)
                    If EG.ContainsGroup Then
                        For i As Integer = 0 To EG.InnerGroup.Count - 1
                            If CurrentIndex < EG.InnerGroup(i).StartIndex Then
                                Parameter = Parameters.Substring(CurrentIndex, EG.InnerGroup(i).StartIndex - CurrentIndex)
                                ArrParameters = Split(Parameter, " ")
                                For j As Integer = 0 To ArrParameters.GetUpperBound(0)
                                    If ArrParameters(j).Trim.Length > 0 Then
                                        Add(New CommandParameter(ArrParameters(j)))
                                    End If
                                Next
                            End If
                            Parameter = EG.InnerGroup(i).ToString
                            Add(New CommandParameter(Parameter))
                            CurrentIndex = EG.InnerGroup(i).StartIndex + Parameter.Length + 3
                        Next
                    End If
                End If
                If Count = PreviousCount Then
                    ArrParameters = Majodio.Mail.Common.MajodioQuoteSplit(Parameters, " ", True)
                    For i As Integer = 0 To ArrParameters.GetUpperBound(0)
                        Parameter = ArrParameters(i)
                        Add(New CommandParameter(Parameter))
                    Next
                End If
            End If
        End Sub

        Default Public Property item(ByVal Index As Integer) As CommandParameter
            Get
                Return CType(MyBase.List.Item(Index), CommandParameter)
            End Get
            Set(ByVal Value As CommandParameter)
                MyBase.List.Item(Index) = Value
            End Set
        End Property
    End Class
End Namespace