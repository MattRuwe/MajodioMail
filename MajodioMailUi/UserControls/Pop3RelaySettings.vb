Imports Majodio.Mail.Common.Configuration
Imports System.Text.RegularExpressions

Public Class Pop3RelaySettings
    Inherits SettingsControlBase

    Public Overrides Sub Initialize()
        SetSaveButtonState()
        InitializePop3RelayAccounts()
        lstPop3RelayAccounts_SelectedIndexChanged(Me, EventArgs.Empty)
        Main.AcceptButton = btnSave
    End Sub

    Private Sub InitializePop3RelayAccounts()
        Dim pop3Relays As Pop3RelayDetails() = RemoteConfigClient.RemoteDomain.GetPop3Relays()
        lstPop3RelayAccounts.Items.Clear()
        For i As Integer = 0 To pop3Relays.GetUpperBound(0)
            lstPop3RelayAccounts.Items.Add(pop3Relays(i).Username & "@" & pop3Relays(i).ServerAddress)
        Next
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim serverAddress As String
        Dim username As String
        Dim password As String
        Dim intervalSeconds As Integer
        Dim deliveryAccounts As EmailAddressCollection
        Dim relay As Pop3RelayDetails

        serverAddress = txtServerAddress.Text
        username = txtUsername.Text
        password = txtPassword.Text
        intervalSeconds = nudIntervalSeconds.Value

        'add the e-mail addresses into the deliveryAccounts
        deliveryAccounts = GetDeliveryAccountsFromList()

        'Create the pop3RelayDetails object
        relay = New Pop3RelayDetails(serverAddress, username, password, intervalSeconds, 0, deliveryAccounts)

        'Save the details
        RemoteConfigClient.RemoteDomain.AddPop3Relay(relay)

        ClearEntryFields()
        InitializePop3RelayAccounts()
    End Sub

    Private Function GetDeliveryAccountsFromList() As EmailAddressCollection
        Dim rVal As New EmailAddressCollection
        For i As Integer = 0 To lstDeliveryAccounts.Items.Count - 1
            rVal.Add(New EmailAddress(CType(lstDeliveryAccounts.Items(i), String)))
        Next
        Return rVal
    End Function

    Private Sub ClearEntryFields()
        txtServerAddress.Text = String.Empty
        txtUsername.Text = String.Empty
        txtPassword.Text = String.Empty
        nudIntervalSeconds.Value = 0
        txtDeliveryAccount.Text = String.Empty
        lstDeliveryAccounts.Items.Clear()
        SetSaveButtonState()
    End Sub

    Private Sub SetSaveButtonState()
        Dim state As Boolean = True
        If txtServerAddress.Text.Trim.Length = 0 Then
            state = False
        End If
        If txtUsername.Text.Trim.Length = 0 Then
            state = False
        End If
        If txtPassword.Text.Trim.Length = 0 Then
            state = False
        End If
        If nudIntervalSeconds.Value = 0 Then
            state = False
        End If
        If lstDeliveryAccounts.Items.Count = 0 Then
            state = False
        End If

        btnSave.Enabled = state
    End Sub

    Private Sub lstPop3RelayAccounts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPop3RelayAccounts.SelectedIndexChanged
        Dim match As Match
        If lstPop3RelayAccounts.SelectedIndex > -1 Then
            match = Regex.Match(lstPop3RelayAccounts.SelectedItem.ToString, "(?i)(?<username>^.+)@(?<server>.+$)")
            Dim details As Pop3RelayDetails
            details = RemoteConfigClient.RemoteDomain.GetPop3Relay(match.Groups("server").Value, match.Groups("username").Value)
            txtServerAddress.Text = details.ServerAddress
            txtUsername.Text = details.Username
            txtPassword.Text = details.Password
            nudIntervalSeconds.Value = details.IntervalSeconds
            lstDeliveryAccounts.Items.Clear()
            For i As Integer = 0 To details.DeliveryAccounts.Count - 1
                lstDeliveryAccounts.Items.Add(details.DeliveryAccounts(i).Address)
            Next
            SetSaveButtonState()
            btnDelete.Enabled = (lstPop3RelayAccounts.SelectedIndex > -1)
        End If
    End Sub

    Private Sub txtServerAddress_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtServerAddress.TextChanged
        SetSaveButtonState()
    End Sub

    Private Sub txtUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsername.TextChanged
        SetSaveButtonState()
    End Sub

    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged
        SetSaveButtonState()
    End Sub

    Private Sub nudIntervalSeconds_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIntervalSeconds.ValueChanged
        SetSaveButtonState()
    End Sub

    Private Sub txtDeliveryAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDeliveryAccount.GotFocus
        Main.AcceptButton = btnAddEmailAddress
    End Sub

    Private Sub txtDeliveryAccount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDeliveryAccount.LostFocus
        Main.AcceptButton = btnSave
    End Sub

    Private Sub txtDeliveryAccount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDeliveryAccount.TextChanged
        SetSaveButtonState()
    End Sub

    Private Sub btnAddEmailAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddEmailAddress.Click
        If Majodio.Common.EmailAddress.IsValidAddress(txtDeliveryAccount.Text) Then
            lstDeliveryAccounts.Items.Add(txtDeliveryAccount.Text)
            lstDeliveryAccounts.Sorted = True
            txtDeliveryAccount.Text = String.Empty
            SetSaveButtonState()
        Else
            MsgBox("Invalid e-mail address", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Validation")
            txtDeliveryAccount.SelectAll()
        End If
        
        txtDeliveryAccount.Focus()
    End Sub

    Private Sub lstDeliveryAccounts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstDeliveryAccounts.SelectedIndexChanged
        btnDeleteEmailAddress.Enabled = (lstDeliveryAccounts.SelectedIndex > -1)
    End Sub

    Private Sub btnDeleteEmailAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteEmailAddress.Click
        If lstDeliveryAccounts.SelectedIndex > -1 Then
            lstDeliveryAccounts.Items.RemoveAt(lstDeliveryAccounts.SelectedIndex)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearEntryFields()
        lstPop3RelayAccounts.SelectedIndex = -1
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim match As Match

        match = Regex.Match(lstPop3RelayAccounts.SelectedItem.ToString, "(?i)(?<username>^.+)@(?<server>.+$)")
        If match.Success Then
            RemoteConfigClient.RemoteDomain.DeletePop3Relay(match.Groups("server").Value, match.Groups("username").Value)
        Else
            MsgBox("Cannot find that value (" & lstPop3RelayAccounts.SelectedIndex.ToString & ") in the configuration")
        End If
        InitializePop3RelayAccounts()
        ClearEntryFields()
    End Sub
End Class
