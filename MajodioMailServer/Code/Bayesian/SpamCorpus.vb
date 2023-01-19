Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

Namespace Bayesian
    <Serializable()> _
    Public Class SpamCorpus

        Private _spam As New Hashtable
        Private _nonSpam As New Hashtable
        Private _probabilities As New Hashtable

        'Constants
        Private Const TOKEN_SEPERATOR_REGEX As String = "(?i)(?:\"".{" & MIN_TOKEN_LENGTH_STRING & "," & MAX_TOKEN_LENGTH_STRING & "}?\"")|(?:\<.{" & MIN_TOKEN_LENGTH_STRING & "," & MAX_TOKEN_LENGTH_STRING & "}?\>)|(?:\{.{" & MIN_TOKEN_LENGTH_STRING & "," & MAX_TOKEN_LENGTH_STRING & "}?\})|(?:\[.{" & MIN_TOKEN_LENGTH_STRING & "," & MAX_TOKEN_LENGTH_STRING & "}?\])|(?:\<.{" & MIN_TOKEN_LENGTH_STRING & "," & MAX_TOKEN_LENGTH_STRING & "}?\>)|(?:[a-z0-9\-'\$]{" & MIN_TOKEN_LENGTH_STRING & "," & MAX_TOKEN_LENGTH_STRING & "})" '"(?i)[a-z0-9\-'\$]"
        Private Const NEUTRAL_PROBABILITY As Double = 0.4
        Private Const MAX_TOKEN_LENGTH As Integer = 30
        Private Const MAX_TOKEN_LENGTH_STRING As String = "30"
        Private Const MIN_TOKEN_LENGTH As Integer = 3
        Private Const MIN_TOKEN_LENGTH_STRING As String = "3"
        Private Const MINIMUM_MESSAGE_PROBABILITY As Double = 0.01
        Private Const MAXIMUM_MESSAGE_PROBABILITY As Double = 0.99
        Private Const TOP_TOKENS_PER_MESSAGE As Integer = 15

        Public Sub ParseMessage(ByVal message As String, ByVal isSpam As Boolean)
            Dim mc As MatchCollection
            Dim newMessage As String

            Try
                Dim mimeMessage As New Mime.Message(message)
                mimeMessage.Normalize()
                newMessage = mimeMessage.RawMessage()
            Catch ex As Exception
                newMessage = message
            End Try

            mc = Regex.Matches(newMessage, TOKEN_SEPERATOR_REGEX, RegexOptions.None)

            For i As Integer = 0 To mc.Count - 1
                AddToken(mc(i).Value.ToLower, isSpam)
            Next
        End Sub

        Public Sub AddToken(ByVal token As String, ByVal isSpam As Boolean)
            If token.Length > MAX_TOKEN_LENGTH Then
                token = token.Substring(0, MAX_TOKEN_LENGTH)
            End If
            If token.Length < MIN_TOKEN_LENGTH Then
                Exit Sub
            End If
            If isSpam Then
                If _spam.ContainsKey(token) Then
                    _spam(token) = CType(_spam(token), Integer) + 1
                Else
                    _spam.Add(token, 1)
                End If

                If Not _nonSpam.ContainsKey(token) Then
                    _nonSpam.Add(token, 0)
                End If
            Else
                If _nonSpam.ContainsKey(token) Then
                    _nonSpam(token) = CType(_nonSpam(token), Integer) + 1
                Else
                    _nonSpam.Add(token, 1)
                End If

                If Not _spam.ContainsKey(token) Then
                    _spam.Add(token, 0)
                End If
            End If

            Dim probability As Double = GetTokenProbability(token)
            If _probabilities.ContainsKey(token) Then
                _probabilities(token) = probability
            Else
                _probabilities.Add(token, probability)
            End If
        End Sub

        Private Function GetTokenProbability(ByVal token As String) As Double
            Dim rVal As Double

            Dim goodInstances As Integer = CType(_nonSpam(token), Integer)
            Dim badInstances As Integer = CType(_spam(token), Integer)

            goodInstances *= 2
            If goodInstances + badInstances < 5 Then
                goodInstances = 0
                badInstances = 0
            End If

            If goodInstances > 0 And badInstances = 0 Then
                'There are only good instances of this word
                rVal = MINIMUM_MESSAGE_PROBABILITY
            ElseIf goodInstances = 0 And badInstances > 0 Then
                'There are only bad instances of this word
                rVal = MAXIMUM_MESSAGE_PROBABILITY
            ElseIf goodInstances = 0 And badInstances = 0 Then
                'There are neither good nor bad instances of this word
                rVal = NEUTRAL_PROBABILITY
            Else
                'Determine the probability based on the good and bad instances
                rVal = badInstances / (goodInstances + badInstances)
            End If

            'If rVal < 0.01 Then
            '    rVal = 0.01
            'ElseIf rVal > 0.99 Then
            '    rVal = 0.99
            'End If
            Return Math.Round(rVal, 5)
        End Function

        Public Function GetTopWords(ByVal message As String) As String()
            Return GetTopWords(message, TOP_TOKENS_PER_MESSAGE)
        End Function

        Public Function GetTopWords(ByVal message As String, ByVal numberOfTokens As Integer) As String()
            Dim rVal As New ArrayList
            Dim token As String
            Dim messageTokens As MatchCollection
            Dim comparer As New NeutralComparer(_probabilities, NEUTRAL_PROBABILITY)

            messageTokens = Regex.Matches(message, TOKEN_SEPERATOR_REGEX, RegexOptions.None)
            For i As Integer = 0 To messageTokens.Count - 1
                token = messageTokens(i).Value.ToLower
                If rVal.IndexOf(token) = -1 Then
                    If rVal.Count < numberOfTokens Then
                        rVal.Add(token)
                    ElseIf comparer.Compare(rVal(0), token) < 0 Then
                        rVal(0) = token
                    End If
                    rVal.Sort(comparer)
                End If
            Next

            Return rVal.ToArray(GetType(String))
        End Function

        Public Function GetMessageProbability(ByVal message As String) As Double
            Dim topWords As String()
            Dim comparer As New NeutralComparer(_probabilities, NEUTRAL_PROBABILITY)
            Dim rVal As Double

            Dim numerator As Single = 1
            Dim denominator As Single = 1

            topWords = GetTopWords(message, TOP_TOKENS_PER_MESSAGE)
            For i As Integer = 0 To topWords.GetUpperBound(0)
                If _probabilities.ContainsKey(topWords(i)) Then
                    numerator *= CType(_probabilities(topWords(i)), Double)
                    denominator *= (1 - CType(_probabilities(topWords(i)), Double))
                Else
                    numerator *= NEUTRAL_PROBABILITY
                    denominator *= NEUTRAL_PROBABILITY
                End If

            Next

            rVal = numerator / (numerator + denominator)

            rVal = Math.Round(rVal, 2)
            If rVal < MINIMUM_MESSAGE_PROBABILITY OrElse Double.IsNegativeInfinity(rVal) OrElse Double.IsNaN(rVal) Then
                rVal = MINIMUM_MESSAGE_PROBABILITY
            ElseIf rVal > MAXIMUM_MESSAGE_PROBABILITY Then
                rVal = MAXIMUM_MESSAGE_PROBABILITY
            End If

            Return rVal
        End Function

        Public Sub Serialize(ByVal path As String)
            Dim formatter As IFormatter = New BinaryFormatter()
            Dim s As Stream = New FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None)

            formatter.Serialize(s, Me)
            s.Close()
        End Sub

        Public Shared Function Deserialize(ByVal path As String) As SpamCorpus
            Dim rVal As SpamCorpus
            Dim formatter As IFormatter = New BinaryFormatter()
            Dim s As Stream = New FileStream(path, FileMode.Open, FileAccess.Read)

            rVal = CType(formatter.Deserialize(s), SpamCorpus)

            Return rVal
        End Function

        Public Overrides Function ToString() As String
            Dim rVal As New System.Text.StringBuilder()

            rVal.Append("Word                          : Probability : Spam  : Non Spam" & vbCrLf)
            For Each key As String In _probabilities.Keys
                rVal.Append(key.PadRight(30))
                rVal.Append(": ")
                rVal.Append(_probabilities(key).ToString.PadLeft(11))
                rVal.Append(" : ")
                rVal.Append(_spam(key).ToString.PadLeft(5))
                rVal.Append(" : ")
                rVal.Append(_nonSpam(key))
                rVal.Append(vbCrLf)

            Next

            Return rVal.ToString
        End Function

        Private Class NeutralComparer
            Implements IComparer
            Private _probabilities As Hashtable
            Private _neutralProbability As Double

            Public Sub New(ByVal probabilities As Hashtable, ByVal neutralProbability As Double)
                _probabilities = probabilities
                _neutralProbability = neutralProbability
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim rVal As Integer

                Dim xString As String = CType(x, String)
                Dim yString As String = CType(y, String)

                Dim xProbability As Double
                Dim yProbability As Double

                If _probabilities.ContainsKey(xString) Then
                    xProbability = CType(_probabilities(xString), Double)
                Else
                    xProbability = _neutralProbability
                End If

                If _probabilities.ContainsKey(yString) Then
                    yProbability = CType(_probabilities(yString), Double)
                Else
                    yProbability = _neutralProbability
                End If

                Dim xDifference As Double
                Dim yDifference As Double
                xDifference = Math.Abs(0.5 - xProbability)
                yDifference = Math.Abs(0.5 - yProbability)

                rVal = xDifference.CompareTo(yDifference)

                Return rVal
            End Function
        End Class
    End Class
End Namespace