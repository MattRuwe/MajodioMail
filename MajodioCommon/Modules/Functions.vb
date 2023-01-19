Imports Microsoft.Win32
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices

Namespace Majodio

    Public Module Functions

        <DllImport("kernel32.dll")> _
        Private Function Beep(ByVal dwFreq As UInteger, ByVal dwDuration As UInteger) As Boolean

        End Function

        Public Sub Beep(ByVal frequency As Integer, ByVal duration As Integer)
            Beep(UInteger.Parse(frequency.ToString), UInteger.Parse(duration.ToString))
        End Sub

        Public Function CompareVersion(ByVal V1 As String, ByVal V2 As String) As Int16
            Dim VersionRegExPattern As String = "[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+"
            Dim RVal As Int16 = 0
            Dim ArrV1 As String()
            Dim ArrV2 As String()
            Dim CurrV1 As Int16
            Dim CurrV2 As Int16
            If Regex.IsMatch(V1, VersionRegExPattern) AndAlso Regex.IsMatch(V2, VersionRegExPattern) Then
                ArrV1 = Split(V1, ".")
                ArrV2 = Split(V2, ".")
                For i As Integer = 0 To 3
                    CurrV1 = Int16.Parse(ArrV1(i))
                    CurrV2 = Int16.Parse(ArrV2(i))
                    If CurrV1 > CurrV2 Then
                        RVal = 1
                    ElseIf CurrV1 < CurrV2 Then
                        RVal = -1
                    End If
                Next
            Else
                Throw New ArgumentException("The version given is not valid")
            End If
            Return RVal
        End Function

        Public Function IsEmailAddress(ByVal Address As String) As Boolean
            Dim RVal As Boolean = False
            Dim MC As MatchCollection

            'If Address.IndexOf("<") = 0 And Address.IndexOf(">") = Address.Length - 1 Then
            '    Address = Address.Substring(1, Address.Length - 2)
            '    'Address = Address.Substring(0, Address.Length - 1)
            'End If
            MC = Regex.Matches(Address, "(?i)[^<]+<(?<email>\S+@\S+\.\S+)>|<(?<email>\S+@\S+\.\S+)>[^>]+|<(?<email>\S+@\S+\.\S+)>|(?<email>\S+@\S+\.\S+)")
            If MC.Count > 0 Then
                Address = MC(0).Groups("email").Value
                If Regex.IsMatch(Address, "^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") Then
                    RVal = True
                End If
                'R = New Regex("^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                'If Not IsNothing(Address) Then
                '    RVal = R.IsMatch(Address.ToUpper)
                'End If
                'R = Nothing
            End If
            Return RVal
        End Function

        Public Function IToB(ByVal Int As Integer) As String
            Dim Remainder As Integer
            Dim CurrentValue As Integer = Int
            Dim ReturnValue As String = String.Empty

            If CurrentValue > 0 Then
                While CurrentValue > 0
                    Remainder = CurrentValue Mod 2
                    ReturnValue = Remainder.ToString() & ReturnValue
                    CurrentValue \= 2
                End While
            Else
                ReturnValue = 0
            End If
            Return ReturnValue
        End Function

        Public Function BToI(ByVal Binary As String) As Integer
            Dim CurrentIndex As Integer
            Dim ReturnValue As Integer = 0
            For CurrentIndex = (Binary.Length - 1) To 0 Step -1
                If Binary.Substring(CurrentIndex, 1) = "1" Then
                    ReturnValue += 2 ^ (Binary.Length - CurrentIndex - 1)
                End If
            Next
            Return ReturnValue
        End Function

        Public Function IsPowerOfTwo(ByVal Number As Integer) As Boolean
            If CType(Math.Log(Number, 2), String).IndexOf(".") > -1 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function FixSql(ByVal Sql As String) As String
            Return Sql.Replace("'", "''")
        End Function

        Public Function Join(ByVal StrArr As String(), ByVal Delimeter As String, ByVal StartIndex As Integer, ByVal EndIndex As Integer) As String
            Dim i As Integer
            Dim RVal As String = String.Empty
            If Not IsNothing(StrArr) Then
                If StrArr.GetUpperBound(0) > 0 Then
                    For i = StartIndex To EndIndex
                        RVal &= StrArr(i) & Delimeter
                    Next
                    RVal = Left(RVal, RVal.Length - Delimeter.Length)
                End If
            End If
            Return RVal
        End Function

        Public Function CheckDbNull(ByVal CheckValue As Object, ByVal ExpectedType As Type) As Object
            Dim RVal As Object = CheckValue
            If Not IsNothing(CheckValue) Then
                If CheckValue.GetType Is GetType(DBNull) Then
                    If ExpectedType Is GetType(String) Then
                        RVal = String.Empty
                    ElseIf ExpectedType Is GetType(Int16) Then
                        RVal = Int16.MinValue
                    ElseIf ExpectedType Is GetType(Int32) Then
                        RVal = Int32.MinValue
                    ElseIf ExpectedType Is GetType(Int64) Then
                        RVal = Int64.MinValue
                    ElseIf ExpectedType Is GetType(Single) Then
                        RVal = Single.MinValue
                    ElseIf ExpectedType Is GetType(Double) Then
                        RVal = Double.MinValue
                    ElseIf ExpectedType Is GetType(DateTime) Then
                        RVal = DateTime.MinValue
                    ElseIf ExpectedType Is GetType(Date) Then
                        RVal = Date.MinValue
                    End If
                End If
            End If
            Return RVal
        End Function

        Public Function GetAscString(ByVal InString As String) As String
            Dim RVal As String = String.Empty
            Dim i As Integer
            For i = 0 To InString.Length - 1
                RVal &= InString.Substring(i, 1) & " = " & Asc(InString.Substring(i, 1)) & " "
            Next
            Return RVal
        End Function

        Public Function EncryptString(ByVal ClearText As String) As String
            Dim RVal As String
            Dim RK As RegistryKey
            Dim EncryptionKey As String
            Dim EncryptionIv As String = String.Empty

            Dim IV(31) As Byte
            Dim Key(31) As Byte
            Dim RM As New RijndaelManaged
            Dim Rng As New RNGCryptoServiceProvider
            Dim CS As CryptoStream
            Dim MS As New System.IO.MemoryStream
            Dim ByteClearText As Byte() = System.Text.Encoding.UTF8.GetBytes(ClearText)
            Dim Output As Byte()

            RK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Majodio\Common")
            If IsNothing(RK) Then
                RK = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\Majodio\Common")
            End If
            EncryptionKey = RK.GetValue("EncryptionKey")
            If IsNothing(EncryptionKey) Then
                'The Key was not found in the registry.  Let's generate a new one
                Rng.GetBytes(IV)
                Rng.GetBytes(Key)

                'Store the value into the registry for future use
                EncryptionKey = System.Convert.ToBase64String(Key)
                RK.SetValue("EncryptionKey", EncryptionKey)
            Else
                ' The Key was found, just need to generate an Initialization Vector
                Rng.GetBytes(IV)
                Key = System.Convert.FromBase64String(EncryptionKey)
            End If

            'Setup the Rijndael algorithm
            RM.BlockSize = 256
            RM.KeySize = 256
            RM.IV = IV
            RM.Key = Key

            ' Setup the CryptoStream, which is used to do the encyption
            CS = New CryptoStream(MS, RM.CreateEncryptor(), CryptoStreamMode.Write)
            ' Write the clear text into the CryptoStream
            CS.Write(ByteClearText, 0, ByteClearText.Length)
            CS.FlushFinalBlock()

            'Get the output from the CryptoStream, which has been stored in 
            'the MemoryStream (MS)
            MS.ToArray()
            ReDim Output(31 + MS.ToArray().Length)
            Buffer.BlockCopy(IV, 0, Output, 0, 32)
            Buffer.BlockCopy(MS.ToArray(), 0, Output, 32, MS.ToArray().Length)

            'Convert the output to a base64 encoded string
            RVal = System.Convert.ToBase64String(Output)
            Return RVal

        End Function

        Public Function DecryptString(ByVal EncryptedText As String) As String
            If IsNothing(EncryptedText) Then
                EncryptedText = String.Empty
            End If
            Dim RVal As String
            Dim RK As RegistryKey
            Dim RM As New RijndaelManaged
            Dim CS As CryptoStream
            Dim Ms As New System.IO.MemoryStream
            Dim ByteEncryptedText As Byte() = System.Convert.FromBase64String(EncryptedText)
            Dim BytesToDecrypt As Byte()
            Dim KeyString As String

            'Get the key out of the registry
            RK = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Majodio\Common")
            If IsNothing(RK) Then
                Throw New Exception("The key could not be located in the registry")
            End If
            KeyString = RK.GetValue("EncryptionKey")
            If IsNothing(KeyString) Then
                Throw New Exception("The key could not be located in the registry")
            End If

            Dim Key() As Byte = System.Convert.FromBase64String(KeyString)
            Dim IV(31) As Byte
            ReDim BytesToDecrypt(ByteEncryptedText.Length - 33)
            Buffer.BlockCopy(ByteEncryptedText, 0, IV, 0, 32)
            Buffer.BlockCopy(ByteEncryptedText, 32, BytesToDecrypt, 0, ByteEncryptedText.Length - 32)

            'Setup the Rijndael algorithm
            RM.BlockSize = 256
            RM.KeySize = 256
            RM.Key = Key
            RM.IV = IV

            'Decrypt the string
            CS = New CryptoStream(Ms, RM.CreateDecryptor(), CryptoStreamMode.Write)
            CS.Write(BytesToDecrypt, 0, BytesToDecrypt.Length)
            CS.FlushFinalBlock()

            'Get the string from the bytes
            RVal = System.Text.ASCIIEncoding.UTF8.GetString(Ms.ToArray())
            Return RVal
        End Function

        Public Function GetMajdoioRegKey() As RegistryKey
            Dim RVal As RegistryKey
            RVal = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Majodio", True)
            If IsNothing(RVal) Then
                RVal = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\Majodio")
            End If
            Return RVal
        End Function

        Public Function WriteToEventLog(ByVal Message As String, ByVal Source As String, ByVal Type As EventLogEntryType) As Boolean
            'HKLM\System\CurrentControlSet\Services\EventLog\Application\
            If Not EventLog.SourceExists(Source) Then
                EventLog.CreateEventSource(Source, "Application")
            End If
            Dim Log As New EventLog
            Log.Source = Source
            EventLog.WriteEntry(Source, Message, Type)
        End Function

        Public Sub WriteToEventLog(ByVal Message As String, ByVal Type As EventLogEntryType)
            WriteToEventLog(Message, System.Reflection.Assembly.GetCallingAssembly.FullName, Type)
        End Sub

        Public Function WriteToEventLog(ByVal Message As String, ByVal e As Exception) As Boolean
            Dim LogMessage As String
            LogMessage = Message & vbCrLf
            LogMessage &= e.Source & vbCrLf
            LogMessage &= e.Message & vbCrLf
            LogMessage &= e.StackTrace & vbCrLf
            WriteToEventLog(LogMessage, System.Reflection.Assembly.GetCallingAssembly.FullName, EventLogEntryType.Error)
        End Function

        Public Function GetSerializedDate() As String
            Dim RVal As String = DateTime.Now.ToString("yyyyMMdd")
            'Dim Now As DateTime = DateTime.Now
            'RVal = Now.Year
            'If Now.Month < 10 Then
            '    RVal &= "0" & Now.Month
            'Else
            '    RVal &= Now.Month
            'End If
            'If Now.Day < 10 Then
            '    RVal &= "0" & Now.Day
            'Else
            '    RVal &= Now.Day
            'End If
            Return RVal
        End Function

        Public Function GetSerializedDateTime() As String
            Dim RVal As String
            Dim Now As DateTime = DateTime.Now
            RVal = Now.ToString("yyyyMMddHHmmssfff")
            Return RVal
        End Function

        Public Function GetLocalUtcOffset() As String
            Dim RVal As String
            RVal = System.TimeZone.CurrentTimeZone.GetUtcOffset(Date.Today).Hours
            If CType(RVal, Integer) < 0 And CType(RVal, Integer) > -10 Then
                RVal = "-0" & System.Math.Abs(CType(RVal, Integer)) & "00"
            ElseIf CType(RVal, Integer) <= -10 Then
                RVal = RVal & "00"
            ElseIf CType(RVal, Integer) >= 0 And CType(RVal, Integer) < 10 Then
                RVal = "0" & RVal & "00"
            Else
                RVal &= "00"
            End If
            Return RVal
        End Function

        Public Function MajodioQuoteSplit(ByVal Expression As String, ByVal Delimeter As String, ByVal QuoteDelimeter As String, ByVal RemoveQuotes As Boolean) As String()
            Dim TmpRVal As New ArrayList
            Dim RVal As String() = New String() {}
            Dim StartIndex As Integer = 0
            Dim EndIndex As Integer = 0
            Dim TmpString As String
            For i As Integer = 0 To Expression.Length - 1
                'Check to see if the current char is a quoted character
                If Expression.Substring(i, QuoteDelimeter.Length) = QuoteDelimeter Then
                    'It is, so now go into CurrentlyQuoted mode
                    i = Expression.IndexOf(QuoteDelimeter, i + QuoteDelimeter.Length)
                    If i = -1 Then
                        i = Expression.Length
                    End If
                ElseIf Expression.Substring(i, Delimeter.Length) = Delimeter Then
                    'Found a delimeter
                    EndIndex = i
                    TmpString = Expression.Substring(StartIndex, EndIndex - StartIndex)
                    If RemoveQuotes Then
                        If TmpString.StartsWith(QuoteDelimeter) Then
                            TmpString = TmpString.Substring(QuoteDelimeter.Length)
                        End If
                        If TmpString.EndsWith(QuoteDelimeter) Then
                            TmpString = TmpString.Substring(0, TmpString.Length - QuoteDelimeter.Length)
                        End If
                    End If
                    TmpRVal.Add(TmpString)
                    StartIndex = EndIndex + QuoteDelimeter.Length
                End If
            Next
            If StartIndex < Expression.Length Then '- 1 Then
                TmpString = Expression.Substring(StartIndex, Expression.Length - StartIndex)
                If TmpString.StartsWith(QuoteDelimeter) Then
                    TmpString = TmpString.Substring(QuoteDelimeter.Length)
                End If
                If TmpString.EndsWith(QuoteDelimeter) Then
                    TmpString = TmpString.Substring(0, TmpString.Length - QuoteDelimeter.Length)
                End If
                TmpRVal.Add(TmpString)
            End If
            ReDim RVal(TmpRVal.Count - 1)
            TmpRVal.CopyTo(RVal, 0)
            Return RVal
        End Function

        Public Function MajodioQuoteSplit(ByVal Expression As String, ByVal Delimeter As String, ByVal QuoteDelimeter As String(), ByVal RemoveQuotes As Boolean) As String()
            Dim TmpRVal As New ArrayList
            Dim RVal As String() = New String() {}
            Dim StartIndex As Integer = 0
            Dim EndIndex As Integer = 0
            Dim TmpString As String
            Dim QuoteDelimeterFound As Boolean
            For i As Integer = 0 To Expression.Length - 1
                QuoteDelimeterFound = False
                For j As Integer = 0 To QuoteDelimeter.GetUpperBound(0)
                    If Expression.Substring(i, QuoteDelimeter(j).Length) = QuoteDelimeter(j) Then
                        i = Expression.IndexOf(QuoteDelimeter(j), i + QuoteDelimeter(j).Length)
                        If i = -1 Then
                            i = Expression.Length
                        End If
                        QuoteDelimeterFound = True
                        Exit For
                    End If
                Next
                If Not QuoteDelimeterFound AndAlso Expression.Substring(i, Delimeter.Length) = Delimeter Then
                    'Found a delimeter
                    EndIndex = i
                    TmpString = Expression.Substring(StartIndex, EndIndex - StartIndex)
                    If RemoveQuotes Then
                        For j As Integer = 0 To QuoteDelimeter.GetUpperBound(0)
                            TmpString = MajodioTrim(TmpString, QuoteDelimeter(j))
                        Next

                    End If
                    TmpRVal.Add(TmpString)
                    StartIndex = EndIndex + QuoteDelimeter.Length
                End If
            Next
            If StartIndex < Expression.Length Then '- 1 Then
                TmpString = Expression.Substring(StartIndex, Expression.Length - StartIndex)
                If RemoveQuotes Then
                    For j As Integer = 0 To QuoteDelimeter.GetUpperBound(0)
                        TmpString = MajodioTrim(TmpString, QuoteDelimeter(j))
                    Next
                End If
                TmpRVal.Add(TmpString)
            End If
            ReDim RVal(TmpRVal.Count - 1)
            TmpRVal.CopyTo(RVal, 0)
            Return RVal
        End Function

        Public Function MajodioTrim(ByVal Expression As String, ByVal StringToTrim As String) As String
            Dim RVal As String = Expression
            While RVal.Length > 0 AndAlso RVal.StartsWith(StringToTrim)
                RVal = RVal.Substring(StringToTrim.Length)
            End While
            While RVal.Length > 0 AndAlso RVal.EndsWith(StringToTrim)
                RVal = RVal.Substring(0, RVal.Length - StringToTrim.Length)
            End While
            Return RVal
        End Function

        Public Function GetCharCount(ByVal Expression As String, ByVal C As Char) As Integer
            Dim RVal As Integer = 0
            Dim CurrentIndex As Integer
            CurrentIndex = Expression.IndexOf(C)
            While CurrentIndex > -1 AndAlso CurrentIndex < Expression.Length
                RVal += 1
                CurrentIndex = Expression.IndexOf(C, CurrentIndex + 1)
            End While
            Return RVal
        End Function

        Public Function GetStringCount(ByVal Expression As String, ByVal SearchString As String) As Integer
            Dim RVal As Integer = 0
            Dim CurrentIndex As Integer
            CurrentIndex = Expression.IndexOf(SearchString)
            While CurrentIndex > -1 AndAlso CurrentIndex < Expression.Length
                RVal += 1
                CurrentIndex = Expression.IndexOf(SearchString, CurrentIndex + SearchString.Length)
            End While
            Return RVal
        End Function

        Public Function GetMd5Hash(ByVal Expression As String) As String
            Return GetMd5Hash(System.Text.Encoding.UTF8.GetBytes(Expression))
        End Function

        Public Function GetMd5Hash(ByVal expression As Byte()) As String
            Dim rVal As String = String.Empty

            Dim md5 As New MD5CryptoServiceProvider()

            Dim result As Byte() = md5.ComputeHash(expression)

            rVal = Convert.ToBase64String(result)

            Return rVal
        End Function

        Public Function GetSha1Hash(ByVal Expression As String) As String
            Return GetSha1Hash(System.Text.Encoding.UTF8.GetBytes(Expression))
        End Function

        Public Function GetSha1Hash(ByVal Expression As Byte()) As String
            Dim Hash As HashAlgorithm = New SHA1Managed()
            Return Convert.ToBase64String(Hash.ComputeHash(Expression))
        End Function

        Public Function GetDirectoryName(ByVal expression As String) As String
            Dim rVal As String = expression
            Dim match As Match

            match = Regex.Match(rVal, "(?<=\\)[^\\]+(?=\\?$)")
            If match.Success Then
                rVal = match.Value
            End If

            Return rVal
        End Function

    End Module
End Namespace


'Imports Crypt = System.Security.Cryptography
'Imports Txt = System.Text
'Imports IO = System.IO
'Imports CM = System.ComponentModel

'Namespace Spider.Cryptography
'    Class Crypto
'        Public Shared Function EncryptString(ByVal InputString As String, _
'                  ByVal PassPhrase As String, _
'                  Optional ByVal IVString As String = vbNullString) As String
'            Dim CryptProvider As New Crypt.RijndaelManaged
'            Dim hashMD5 As New Crypt.MD5CryptoServiceProvider
'            Dim hashSHA As New Crypt.SHA1CryptoServiceProvider
'            Dim InputbyteArray() As Byte = Txt.Encoding.UTF8.GetBytes(InputString)

'            CryptProvider.Key = hashMD5.ComputeHash(Txt.ASCIIEncoding.ASCII.GetBytes(PassPhrase))
'            CryptProvider.IV = hashMD5.ComputeHash(Txt.ASCIIEncoding.ASCII.GetBytes(IVString))
'            CryptProvider.Mode = Crypt.CipherMode.CBC

'            Dim ms As IO.MemoryStream = New IO.MemoryStream
'            Dim cs As Crypt.CryptoStream = New Crypt.CryptoStream(ms, _
'              CryptProvider.CreateEncryptor(), Crypt.CryptoStreamMode.Write)
'            cs.Write(InputbyteArray, 0, InputbyteArray.Length)
'            cs.FlushFinalBlock()

'            Dim ret As Txt.StringBuilder = New Txt.StringBuilder
'            Dim b() As Byte = ms.ToArray
'            Dim I As Integer
'            Dim Out As String
'            For I = 0 To UBound(b)
'                'Format as hex
'                ret.AppendFormat("{0:X2}", b(I))
'            Next
'            Return ret.ToString()

'        End Function
'        Public Shared Function DecryptString(ByVal InputString As String, _
'                  ByVal PassPhrase As String, _
'                  ByVal IVString As String) As String
'            Dim DES As New Crypt.RijndaelManaged
'            Dim hashMD5 As New Crypt.MD5CryptoServiceProvider
'            DES.Key = hashMD5.ComputeHash(Txt.ASCIIEncoding.ASCII.GetBytes(PassPhrase))
'            DES.IV = hashMD5.ComputeHash(Txt.ASCIIEncoding.ASCII.GetBytes(IVString))
'            DES.Mode = Crypt.CipherMode.CBC
'            'Put the input string into the byte array
'            Dim inputByteArray(InputString.Length / 2 - 1) As Byte
'            Dim X As Integer
'            For X = 0 To InputString.Length / 2 - 1
'                Dim IJ As Integer = (Convert.ToInt32(InputString.Substring(X * 2, 2), 16))
'                Dim BT As New CM.ByteConverter
'                inputByteArray(X) = New Byte
'                inputByteArray(X) = BT.ConvertTo(IJ, GetType(Byte))
'            Next

'            'Create the crypto objects
'            Dim MS As IO.MemoryStream = New IO.MemoryStream
'            Dim CS As Crypt.CryptoStream = New Crypt.CryptoStream(MS, DES.CreateDecryptor(), Crypt.CryptoStreamMode.Write)

'            'Flush the data through the crypto stream into the memory stream
'            CS.Write(inputByteArray, 0, inputByteArray.Length)
'            CS.FlushFinalBlock()

'            'Get the decrypted data back from the memory stream
'            Dim ret As Txt.StringBuilder = New Txt.StringBuilder
'            Dim B() As Byte = MS.ToArray
'            Dim I As Integer
'            For I = 0 To UBound(B)
'                ret.Append(Chr(B(I)))
'            Next
'            Return ret.ToString()

'        End Function
'    End Class
'End Namespace
